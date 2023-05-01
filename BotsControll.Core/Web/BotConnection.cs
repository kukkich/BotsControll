using BotsControll.Core.Models.Base;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotsControll.Core.Web;

public class BotConnection
{
    public string Id => Bot.Id;
    public WebSocket Connection { get; }
    public IBotIdentity Bot { get; }
    public string JournalId { get; }

    public BotConnection(WebSocket connection, IBotIdentity bot, string journalId)
    {
        Connection = connection;
        Bot = bot;
        JournalId = journalId;
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