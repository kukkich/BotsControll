using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using BotsControll.Api.Services.Users;
using BotsControll.Api.Web.Connections;
using BotsControll.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BotsControll.Api.Hubs;

[Authorize]
public class UserHub : Hub
{
    private readonly UserConnectionService _userConnectionService;

    public UserHub(UserConnectionService userConnectionService)
    {
        _userConnectionService = userConnectionService;
    }

    public override Task OnConnectedAsync()
    {
        GetUserAndConnectionId(out var userIdentity, out var connectionId);

        _userConnectionService.Connect(userIdentity, connectionId);

        return base.OnConnectedAsync();
    }

    

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        GetUserAndConnectionId(out var userIdentity, out var connectionId);

        _userConnectionService.Disconnect(userIdentity, connectionId, exception);

        return base.OnDisconnectedAsync(exception);
    }

    private void GetUserAndConnectionId(out UserIdentity userIdentity, out string connectionId)
    {
        userIdentity = (UserIdentity)Context.User!;
        connectionId = Context.ConnectionId;
    }

}