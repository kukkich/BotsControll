using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BotsControll.Api.Web.Connections;

public class UserConnectionRepository
{
    private readonly ConcurrentDictionary<int, ConnectedUser> _users = new();

    public ConnectedUser GetOrAddIfNotExist(int userId, Func<int, ConnectedUser> userFactory)
    {
        return _users.GetOrAdd(userId, userFactory);
    }

    public bool TryGetByUserId(int id, out ConnectedUser? user)
    {
        return _users.TryGetValue(id, out user!);
    }

    public bool TryRemoveById(int id, out ConnectedUser? user)
    {
        return _users.TryRemove(id, out user);
    }

    public IEnumerable<KeyValuePair<int, ConnectedUser>> GetAll()
    {
        return _users.AsEnumerable();
    }
}