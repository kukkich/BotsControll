using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotsControll.Api.Web.Connections;

namespace BotsControll.Api.Services.Communication;

public class BotMessageService : IBotMessageService
{
    private readonly IBotConnectionRepository _connectionRepository;

    public BotMessageService(IBotConnectionRepository connectionRepository)
    {
        _connectionRepository = connectionRepository;
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