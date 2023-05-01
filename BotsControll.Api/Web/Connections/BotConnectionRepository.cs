using BotsControll.Core.Web;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BotsControll.Api.Web.Connections;

public class BotConnectionRepository : IBotConnectionRepository
{
    private readonly ConcurrentDictionary<string, BotConnection> _sockets = new();

    public IEnumerable<BotConnection> All => _sockets.Values;

    public BotConnection GetByBotId(string id)
    {
        return _sockets.FirstOrDefault(p => p.Key == id).Value;
    }

    public void Add(string botId, BotConnection connection)
    {
        _sockets.TryAdd(botId, connection);
    }

    public BotConnection Remove(string id)
    {
        if (!_sockets.TryRemove(id, out var botConnection))
            throw new InvalidOperationException();

        return botConnection;
    }
}