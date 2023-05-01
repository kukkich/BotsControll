using BotsControll.Api.Services.Communication;
using BotsControll.Api.Services.Connections;
using BotsControll.Core.Dtos;
using BotsControll.Core.Dtos.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BotsControll.Api.Hubs;

//Todo
//[Authorize]
public class ClientHub : Hub
{
    private readonly UserConnectionService _userConnectionService;
    private readonly IBotMessageService _botMessageService;

    public ClientHub(
        UserConnectionService userConnectionService,
        IBotMessageService botMessageService)
    {
        _userConnectionService = userConnectionService;
        _botMessageService = botMessageService;
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

    public async Task SendToBot(string connectionId, string message)
    {
        await _botMessageService.SendTo(connectionId, message);
    }

    private void GetUserAndConnectionId(out UserIdentity userIdentity, out string connectionId)
    {
        var id = 1;
        userIdentity = new UserIdentity(id, "admin", "kukkich@gmail.com", Array.Empty<RoleDto>().ToList());
        //(GetUserIdentity)BotsControlContext.User!;
        connectionId = Context.ConnectionId;
    }

    public static class Actions
    {
        public const string ReceiveMessage = "ReceiveMessage";
        public const string OnNewBotConnection = "OnNewConnection";
        public const string OnBotDisconnection = "OnBotDisconnection";
    }
}