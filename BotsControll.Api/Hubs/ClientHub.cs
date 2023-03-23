using BotsControll.Api.Services.Connections;
using BotsControll.Core.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using BotsControll.Api.Services.Communication;

namespace BotsControll.Api.Hubs;

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
        var id = Random.Shared.Next(255);
        userIdentity = new UserIdentity(id, $"user with id {id} name");
        //(UserIdentity)Context.User!;
        connectionId = Context.ConnectionId;
    }

    public static class Actions
    {
        public const string ReceiveMessage = "ReceiveMessage";
        public const string OnNewBotConnection = "OnNewConnection";
        public const string OnBotDisconnection = "OnNewConnection";
    }
}