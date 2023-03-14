using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;

namespace BotsControll.Api.Web.Receiving;

public interface IWebSocketReceiverFactory
{
    public IWebSocketReceiver Crete(WebSocket webSocket, HttpContext context);
}