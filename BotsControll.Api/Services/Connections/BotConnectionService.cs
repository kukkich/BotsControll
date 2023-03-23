using System;
using BotsControll.Api.Hubs;
using BotsControll.Api.Web.Connections;
using BotsControll.Core.Web;
using BotsControll.Core.Web.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace BotsControll.Api.Services.Connections;

public class BotConnectionService : IBotConnectionService
{
    private readonly IBotConnectionRepository _connectionRepository;
    private readonly IHubContext<ClientHub> _clientsHubContext;
    private readonly ILogger<BotConnectionService> _logger;

    public BotConnectionService(
        IBotConnectionRepository connectionRepository,
        IHubContext<ClientHub> clientsHubContext,
        ILogger<BotConnectionService> logger
    )
    {
        _connectionRepository = connectionRepository;
        _clientsHubContext = clientsHubContext;
        _logger = logger;
    }

    public async Task<string> AcceptConnectionAsync(BotConnection connection)
    {
        var id = _connectionRepository.Add(connection);

        await NotifyOnNewConnection(new BotConnectionDto(id, connection.Bot));

        return id;
    }
    public async Task DisconnectAsync(string id, string? reason = null)
    {
        var botConnection = _connectionRepository.Remove(id);
        if (botConnection is null)
        {
            throw new InvalidOperationException($"No bot connection with id {id}");
        }

        if (botConnection.Connection.State is
            WebSocketState.Open or WebSocketState.CloseReceived or WebSocketState.CloseSent)
        {
            await botConnection.Connection.CloseAsync(
                closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: "Closed by the ConnectionManager",
                cancellationToken: CancellationToken.None
            );
        }

        _logger.LogInformation(
            "Connection {connectionId} associated with {name} was disconnected. \n      Reason: {reason}",
            id, botConnection.Bot.Name, reason ?? "Undefined"
        );
    }

    private async Task NotifyOnNewConnection(BotConnectionDto newConnection)
    {
        await _clientsHubContext.Clients.All.SendCoreAsync(
            ClientHub.Actions.OnNewBotConnection,
            new object?[] { newConnection }
        );
    }
    private async Task NotifyOnNewDisconnection(BotConnectionDto removedConnection)
    {
        await _clientsHubContext.Clients.All.SendCoreAsync(
            ClientHub.Actions.OnBotDisconnection,
            new object?[] { removedConnection }
        );
    }
}