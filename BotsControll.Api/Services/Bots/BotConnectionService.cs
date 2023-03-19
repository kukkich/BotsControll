using BotsControll.Api.Hubs;
using BotsControll.Api.Web.Connections;
using BotsControll.Core.Web;
using BotsControll.Core.Web.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace BotsControll.Api.Services.Bots;

public class BotConnectionService : IBotConnectionService
{
    private readonly IBotConnectionRepository _connectionRepository;
    private readonly IHubContext<BotsHub> _botsHubContext;
    private readonly ILogger<BotConnectionService> _logger;

    public BotConnectionService(
        IBotConnectionRepository connectionRepository,
        IHubContext<BotsHub> botsHubContext,
        ILogger<BotConnectionService> logger
    )
    {
        _connectionRepository = connectionRepository;
        _botsHubContext = botsHubContext;
        _logger = logger;
    }

    public async Task<string> AcceptConnectionAsync(BotConnection connection)
    {
        var id = _connectionRepository.Add(connection);

        await NotifyOnNewConnection(new BotConnectionDto(id, connection.Bot));

        return id;
    }

    private async Task NotifyOnNewConnection(BotConnectionDto newConnection)
    {
        await _botsHubContext.Clients.All.SendCoreAsync(
            ClientHub.Actions.OnNewBotConnection,
            new object?[] { newConnection }
        );
    }
    private async Task NotifyOnNewDisconnection(BotConnectionDto newConnection)
    {
        await _botsHubContext.Clients.All.SendCoreAsync(
            ClientHub.Actions.OnBotDisconnection,
            new object?[] { newConnection }
        );
    }

    public async Task DisconnectAsync(string id, string? reason = null)
    {
        _logger.LogInformation("Start disconnection {id}", id);

        var botConnection = _connectionRepository.Remove(id);
        _logger.LogInformation("Removed Connection {id} ", id);

        if (botConnection.Connection.State is WebSocketState.Open or WebSocketState.CloseReceived or WebSocketState.CloseSent)
            await botConnection.Connection.CloseAsync(
                closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: "Closed by the ConnectionManager",
                cancellationToken: CancellationToken.None
            );

        _logger.LogInformation(
            "Connection {connectionId} associated with {name} was successfully disconnected. \n      Reason: {reason}",
            id, botConnection.Bot.Name, reason ?? "Undefined"
        );
    }

    public async Task SendToAll(string message)
    {
        await Task.WhenAll(_connectionRepository.AllConnection
            .Select(botConnection => botConnection.SendAsync(message))
            .ToArray()
        );
    }

    public async Task SendAllExcept(string message, IEnumerable<string> exceptedIds)
    {
        await Task.WhenAll(
            _connectionRepository.All
            .Where(kv => exceptedIds.All(x => x != kv.Key))
            .Select(x => x.Value)
            .Select(connection => connection.SendAsync(message))
        );
    }

    public async Task SendTo(string connectionId, string message)
    {
        var botConnection = _connectionRepository.GetById(connectionId);

        await botConnection.SendAsync(message);
    }
}