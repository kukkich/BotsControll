using BotsControll.Api.Middlewares;
using BotsControll.Api.Services.Bots;
using BotsControll.Core.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;

namespace BotsControll.Api.Web.Receiving;

public class BotWebSocketReceiverFactory : IWebSocketReceiverFactory
{
    private readonly IBotConnectionService _botConnectionService;
    private readonly ILogger<BotWebSocketReceiver> _botLogger;

    public BotWebSocketReceiverFactory(
        IBotConnectionService botConnectionService,
        ILogger<BotWebSocketReceiver> botLogger
        )
    {
        _botConnectionService = botConnectionService;
        _botLogger = botLogger;
    }

    public IWebSocketReceiver Crete(WebSocket webSocket, HttpContext context)
    {
        var botName = context.User.Identity!.Name;
        var botConnection = new BotConnection(webSocket, new TestBotIdentity(botName));

        return new BotWebSocketReceiver(botConnection, _botConnectionService, _botLogger);
    }
}