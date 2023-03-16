using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using BotsControll.Api.Hubs;
using BotsControll.Api.Web.Connections;
using BotsControll.Core.Web;
using BotsControll.Core.Web.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace BotsControll.Api.Services.Bots;

public class BotConnectionService : IBotConnectionService
{
    private readonly IBotConnectionRepository _connectionRepository;
    private readonly IHubContext<BotsHub> _botsHubContext;

    public BotConnectionService(
        IBotConnectionRepository connectionRepository,
        IHubContext<BotsHub> botsHubContext
        )
    {
        _connectionRepository = connectionRepository;
        _botsHubContext = botsHubContext;
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
            "OnNewConnection", 
            new object?[] { newConnection }
        );
    }
    private async Task NotifyOnNewDisconnectDisconnection(BotConnectionDto newConnection)
    {
        await _botsHubContext.Clients.All.SendCoreAsync(
            "OnNewConnection",
            new object?[] { newConnection }
        );
    }

    public async Task DisconnectAsync(string id)
    {
        var connection = _connectionRepository.Remove(id);

        await connection.Connection.CloseAsync(
            closeStatus: WebSocketCloseStatus.NormalClosure,
            statusDescription: "Closed by the ConnectionManager",
            cancellationToken: CancellationToken.None
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