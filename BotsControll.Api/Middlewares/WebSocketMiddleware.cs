using BotsControll.Api.Web.Receiving;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BotsControll.Api.Middlewares;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebSocketReceiverFactory _receiverFactory;

    public WebSocketMiddleware(RequestDelegate next, IWebSocketReceiverFactory receiverFactory)
    {
        _next = next;
        _receiverFactory = receiverFactory;
    }

    public async Task Invoke(HttpContext context)
    {
        // todo: should be invoke next
        if (!context.WebSockets.IsWebSocketRequest)
            return;

        var socket = await context.WebSockets.AcceptWebSocketAsync();

        var receiver = _receiverFactory.Crete(socket, context);

        await receiver.StartReceiveAsync();
    }

}