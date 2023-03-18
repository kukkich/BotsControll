using System.Net.WebSockets;
using System.Text;

var endpoint = "wss://localhost:7126/ws/bot";

Console.Write("Input a bot name: ");
var botName = Console.ReadLine();

using var webSocket = new ClientWebSocket();

var uri = new Uri($"{endpoint}?Name={botName}");

await webSocket.ConnectAsync(uri, CancellationToken.None);

while (webSocket.State != WebSocketState.Open)
{
    await webSocket.ConnectAsync(uri, CancellationToken.None);
}

Console.WriteLine("Connected to server");

var buf = new byte[1024];

while (webSocket.State == WebSocketState.Open)
{
    var result = await webSocket.ReceiveAsync(buf, CancellationToken.None);

    if (result.MessageType == WebSocketMessageType.Close)
    {
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
        Console.WriteLine(result.CloseStatusDescription);
    }
    else Console.WriteLine($"New message: {Encoding.UTF8.GetString(buf, 0, result.Count)}");
}