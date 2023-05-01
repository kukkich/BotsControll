using BotsControll.Api.Services.Communication;
using BotsControll.Api.Services.Connections;
using BotsControll.Core.Dtos.Identity;
using BotsControll.Core.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using System.Security.Claims;

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
        var botId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (botName is null || botId is null)
        {
            throw new ArgumentException("Bot has no name or id");
        }

        var botConnection = new BotConnection(
            webSocket, 
            new BotIdentity(botName, botId),
            Guid.NewGuid().ToString()
        );

        return new BotWebSocketReceiver(
            botConnection,
            _botConnectionService,
            _botMessageService,
            _botLogger
        );
    }
}