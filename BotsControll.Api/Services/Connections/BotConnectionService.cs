using BotsControll.Api.Hubs;
using BotsControll.Api.Web.Connections;
using BotsControll.Core.Dtos.Bots;
using BotsControll.Core.Models.Base;
using BotsControll.Core.Repositories;
using BotsControll.Core.Web;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using BotsControll.Core.Models;
using BotsControll.Core.Services.Time;

namespace BotsControll.Api.Services.Connections;

public class BotConnectionService : IBotConnectionService
{
    private readonly IBotConnectionRepository _connectionRepository;
    private readonly IHubContext<ClientHub> _clientsHubContext;
    private readonly ILogger<BotConnectionService> _logger;
    private readonly IBotsRepository _botsRepository;
    private readonly IJournalRepository _journalRepository;
    private readonly ITimeService _timeService;

    public BotConnectionService(
        IBotConnectionRepository connectionRepository,
        IHubContext<ClientHub> clientsHubContext,
        ILogger<BotConnectionService> logger,
        IBotsRepository botsRepository,
        IJournalRepository journalRepository,
        ITimeService timeService
    )
    {
        _connectionRepository = connectionRepository;
        _clientsHubContext = clientsHubContext;
        _logger = logger;
        _botsRepository = botsRepository;
        _journalRepository = journalRepository;
        _timeService = timeService;
    }

    public async Task AcceptConnectionAsync(BotConnection connection)
    {
        var id = connection.Id;
        _connectionRepository.Add(id, connection);
        
        var botFromDb = await _botsRepository.GetById(id);
        botFromDb.Journals.Add(
            new Journal
            {
                Id = connection.JournalId,
                CreatedAt = _timeService.GetTime()
            });

        botFromDb.IsActive = true;
        await _botsRepository.Update(botFromDb);

        await NotifyOnNewConnection((BotDto)botFromDb);
    }

    public async Task DisconnectAsync(IBotIdentity bot, string? reason = null)
    {
        var id = bot.Id;
        var botConnection = _connectionRepository.Remove(id);
        if (botConnection is null)
            throw new InvalidOperationException($"No connected bot with id {id}");

        if (IsInClosableState(botConnection))
            await CloseConnectionAsync(botConnection);

        var botFromDb = await DeactivateBot(id);
        await CloseJournal(botConnection.JournalId);

        await NotifyOnDisconnection((BotDto)botFromDb);

        _logger.LogInformation(
            "{name} was disconnected. Reason: {reason}",
            botConnection.Bot.Name, reason ?? "Undefined"
        );
    }

    private static bool IsInClosableState(BotConnection botConnection)
    {
        return botConnection.Connection.State is
            WebSocketState.Open or WebSocketState.CloseReceived or WebSocketState.CloseSent;
    }
    private static async Task CloseConnectionAsync(BotConnection botConnection)
    {
        await botConnection.Connection.CloseAsync(
            closeStatus: WebSocketCloseStatus.NormalClosure,
            statusDescription: "Closed by the ConnectionManager",
            cancellationToken: CancellationToken.None
        );
    }
    private async Task<Bot> DeactivateBot(string id)
    {
        var botFromDb = await _botsRepository.GetById(id);
        botFromDb.IsActive = false;
        await _botsRepository.Update(botFromDb);
        return botFromDb;
    }
    private async Task CloseJournal(string id)
    {
        var journal = await _journalRepository.GetById(id);
        journal.ClosedAt = _timeService.GetTime();
        await _journalRepository.Update(journal);
    }

    private async Task NotifyOnNewConnection(BotDto bot)
    {
        await _clientsHubContext.Clients.All.SendCoreAsync(
            ClientHub.Actions.OnNewBotConnection,
            new object?[] { bot }
        );
    }
    private async Task NotifyOnDisconnection(BotDto bot)
    {
        await _clientsHubContext.Clients.All.SendCoreAsync(
            ClientHub.Actions.OnBotDisconnection,
            new object?[] { bot }
        );
    }
}