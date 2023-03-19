using BotsControll.Api.Services.Bots;
using BotsControll.Api.Services.Users;
using BotsControll.Core.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace BotsControll.Api.Hubs;

//[Authorize]
public class ClientHub : Hub
{
    private readonly UserConnectionService _userConnectionService;
    private readonly IBotConnectionService _botConnectionService;
    public ClientHub(UserConnectionService userConnectionService, IBotConnectionService botConnectionService)
    {
        _userConnectionService = userConnectionService;
        _botConnectionService = botConnectionService;
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
        var id = Random.Shared.Next(255);
        userIdentity = new UserIdentity(id, $"user with id {id} name");
        //(UserIdentity)Context.User!;
        connectionId = Context.ConnectionId;
    }

    public async Task SendToBot(string connectionId, string message)
    {
        await _botConnectionService.SendTo(connectionId, message);
    }

    public static class Actions
    {
        public const string ReceiveMessage = "ReceiveMessage";
        public const string OnNewBotConnection = "OnNewConnection";
        public const string OnBotDisconnection = "OnNewConnection";
    }
}