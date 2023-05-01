using BotsControll.Api.Web.Connections;
using BotsControll.Core.Dtos.Identity;
using BotsControll.Core.Models.Base;
using System;
using System.Linq;

namespace BotsControll.Api.Services.Connections;

public class UserConnectionService
{
    private readonly UserConnectionRepository _userConnections;

    public UserConnectionService(UserConnectionRepository userConnections)
    {
        _userConnections = userConnections;
    }

    public void Connect(UserIdentity user, string connectionId)
    {
        var existedUser = _userConnections.GetOrAddIfNotExist(user.Id, _ => new ConnectedUser
        {
            User = user,
        });

        lock (existedUser.ConnectionIds)
        {
            existedUser.ConnectionIds.Add(connectionId);
        }
    }

    public void Disconnect(IUserIdentity user, string connectionId, Exception? exception = null)
    {
        _userConnections.TryGetByUserId(user.Id, out var connectedUser);

        if (connectedUser is null) return;

        lock (connectedUser.ConnectionIds)
        {
            connectedUser.ConnectionIds.RemoveWhere(id => id == connectionId);

            if (connectedUser.ConnectionIds.Any())
                return;

            _userConnections.TryRemoveByUserId(user.Id, out _);
        }
    }
}