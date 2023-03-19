using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;

namespace BotsControll.Api.Web.Receiving;

public interface IWebSocketReceiverFactory
{
    public IWebSocketReceiver Crete(WebSocket webSocket, HttpContext context);
}