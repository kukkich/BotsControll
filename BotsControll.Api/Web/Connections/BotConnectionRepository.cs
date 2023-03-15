using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BotsControll.Api.Web.Connections;

public class BotConnectionRepository : IBotConnectionRepository
{
    private readonly ConcurrentDictionary<string, BotConnection> _sockets = new();
    
    public IEnumerable<KeyValuePair<string, BotConnection>> All => _sockets.AsEnumerable();
    public IEnumerable<BotConnection> AllConnection => _sockets.AsEnumerable().Select(x => x.Value);

    public BotConnection GetSocketById(string id)
    {
        return _sockets.FirstOrDefault(p => p.Key == id).Value;
    }

    public string GetId(BotConnection socket)
    {
        return _sockets.FirstOrDefault(p => p.Value == socket).Key;
    }

    public string Add(BotConnection socket)
    {
        var id = CreateConnectionId();
        _sockets.TryAdd(CreateConnectionId(), socket);

        return id;
    }

    public IEnumerable<string> GetAllIds()
    {
        return _sockets.Keys;
    }

    public BotConnection Remove(string id)
    {
        _sockets.TryRemove(id, out var botConnection);

        return botConnection;
    }
    
    private string CreateConnectionId()
    {
        return Guid.NewGuid().ToString();
    }
}