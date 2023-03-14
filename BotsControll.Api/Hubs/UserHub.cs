using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BotsControll.Api.Web.Connections;
using BotsControll.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BotsControll.Api.Hubs;

[Authorize]
public class UserHub : Hub
{
    private static readonly ConcurrentDictionary<int, ConnectedUser> Users = new ();

    public async Task Join()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "My group");
    }

    public override Task OnConnectedAsync()
    {
        var userIdentity = (UserIdentity)Context.User!;
        var connectionId = Context.ConnectionId;

        var existedUser = Users.GetOrAdd(userIdentity.Id, _ => new ConnectedUser
        {
            User = userIdentity,
        });

        lock (existedUser.ConnectionIds)
        {
            existedUser.ConnectionIds.Add(connectionId);
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userIdentity = (UserIdentity)Context.User!;
        var connectionId = Context.ConnectionId;

        Users.TryGetValue(userIdentity.Id, out var user);

        if (user is null) return base.OnDisconnectedAsync(exception);
        
        lock (user.ConnectionIds)
        {
            user.ConnectionIds.RemoveWhere(conId => conId == connectionId);

            if (user.ConnectionIds.Any())
                return base.OnDisconnectedAsync(exception);

            Users.TryRemove(userIdentity.Id, out _);
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task Send(string message)
    {
        await Clients.Groups("My group").SendAsync("123");
    }
}