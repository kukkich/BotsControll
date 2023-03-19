using BotsControll.Core.Identity;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotsControll.Core.Web;

public class BotConnection
{
    public WebSocket Connection { get; }

    public IBotIdentity Bot { get; }

    public BotConnection(WebSocket connection, IBotIdentity bot)
    {
        Connection = connection;
        Bot = bot;
    }

    public async Task SendAsync(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);

        await Connection.SendAsync(
            buffer: new ArraySegment<byte>(
                array: bytes,
                offset: 0,
                count: bytes.Length
            ),
            messageType: WebSocketMessageType.Text,
            endOfMessage: true,
            cancellationToken: CancellationToken.None
        );
    }
}