using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using BotsControll.Api.Web.Connections;

namespace BotsControll.Api.Services.Bots;

public class BotConnectionService : IBotConnectionService
{
    private readonly IBotConnectionRepository _connectionRepository;

    public BotConnectionService(IBotConnectionRepository connectionRepository)
    {
        _connectionRepository = connectionRepository;
    }

    public string AcceptConnection(BotConnection connection)
    {
        return _connectionRepository.Add(connection);
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
}