using System;
using BotsControll.Api.Middlewares;
using BotsControll.Api.Services.Connections;
using BotsControll.Core.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using BotsControll.Api.Services.Communication;

namespace BotsControll.Api.Web.Receiving;

public class BotWebSocketReceiverFactory : IWebSocketReceiverFactory
{
    private readonly IBotMessageService _botMessageService;
    private readonly IBotConnectionService _botConnectionService;
    private readonly ILogger<BotWebSocketReceiver> _botLogger;

    public BotWebSocketReceiverFactory(
        IBotMessageService botMessageService,
        IBotConnectionService botConnectionService,
        ILogger<BotWebSocketReceiver> botLogger
        )
    {
        _botMessageService = botMessageService;
        _botConnectionService = botConnectionService;
        _botLogger = botLogger;
    }

    public IWebSocketReceiver Crete(WebSocket webSocket, HttpContext context)
    {
        var botName = context.User.Identity!.Name;
        if (botName is null)
        {
            throw new ArgumentException("Bot has no name");
        }

        var botConnection = new BotConnection(webSocket, new TestBotIdentity(botName));

        return new BotWebSocketReceiver(
            botConnection, 
            _botConnectionService, 
            _botMessageService, 
            _botLogger
        );
    }
}