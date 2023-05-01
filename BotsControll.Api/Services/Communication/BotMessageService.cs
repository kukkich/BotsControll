using BotsControll.Api.Web.Connections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        await Task.WhenAll(
            _connectionRepository.All
                .Select(botConnection => botConnection.SendAsync(message))
                .ToArray()
        );
    }

    public async Task SendAllExcept(string message, IEnumerable<string> exceptedIds)
    {
        await Task.WhenAll(
            _connectionRepository.All
                .Where(x => exceptedIds.All(e => e != x.Id))
                .Select(connection => connection.SendAsync(message))
        );
    }

    public async Task SendTo(string botId, string message)
    {
        var botConnection = _connectionRepository.GetByBotId(botId);

        await botConnection.SendAsync(message);
    }
}