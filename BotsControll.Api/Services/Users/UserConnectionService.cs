using BotsControll.Api.Hubs;
using BotsControll.Api.Web.Connections;
using BotsControll.Core.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BotsControll.Api.Services.Users;

public class UserConnectionService
{
    private readonly UserConnectionRepository _userConnections;
    private readonly IHubContext<ClientHub> _userHub;

    public UserConnectionService(UserConnectionRepository userConnections, IHubContext<ClientHub> userHub)
    {
        _userConnections = userConnections;
        _userHub = userHub;
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

    public void Disconnect(UserIdentity user, string connectionId, Exception? exception = null)
    {
        _userConnections.TryGetByUserId(user.Id, out var connectedUser);

        if (connectedUser is null) return;

        lock (connectedUser.ConnectionIds)
        {
            connectedUser.ConnectionIds.RemoveWhere(id => id == connectionId);

            if (connectedUser.ConnectionIds.Any())
                return;

            _userConnections.TryRemoveById(user.Id, out _);
        }
    }

    public async Task Send(int userId, string message)
    {
        _userConnections.TryGetByUserId(userId, out var connectedUser);
        if (connectedUser is null) return;

        foreach (var connectionId in connectedUser.ConnectionIds)
        {
            await _userHub.Clients.Client(connectionId).SendCoreAsync(
                ClientHub.Actions.ReceiveMessage,
                new object?[] { message }
                );
        }
    }
}