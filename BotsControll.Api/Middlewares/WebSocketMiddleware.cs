using BotsControll.Api.Web.Receiving;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BotsControll.Api.Middlewares;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;

    public WebSocketMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IWebSocketReceiverFactory receiverFactory)
    {
        // todo: should be invoke next
        if (!context.WebSockets.IsWebSocketRequest)
            return;

        var socket = await context.WebSockets.AcceptWebSocketAsync();

        var receiver = receiverFactory.Crete(socket, context);

        await receiver.StartReceiveAsync();
    }

}