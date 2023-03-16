using System.Net.Sockets;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using BotsControll.Api.Services.Bots;
using BotsControll.Api.Web.Connections;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using System.Text;
using BotsControll.Core.Web;

namespace BotsControll.Api.Web.Receiving;

public class BotWebSocketReceiver : IWebSocketReceiver
{
    private readonly BotConnection _botConnection;
    private string _connectionId = null!;
    private readonly IBotConnectionService _connectionService;
    private readonly ILogger<BotWebSocketReceiver> _logger;
    private byte[] _buffer = new byte[1024 * 4];

    public BotWebSocketReceiver(
        BotConnection botConnection, 
        IBotConnectionService connectionService,
        ILogger<BotWebSocketReceiver> logger
    )
    {
        _botConnection = botConnection;
        _connectionService = connectionService;
        _logger = logger;
    }

    public async Task StartReceiveAsync()
    {
        CreateBuffer();
        _connectionId = await _connectionService.AcceptConnectionAsync(_botConnection);

        while (_botConnection.Connection.State == WebSocketState.Open)
        {
            var result = await _botConnection.Connection.ReceiveAsync(
                buffer: new ArraySegment<byte>(_buffer),
                cancellationToken: CancellationToken.None
            );

            await HandleMessage(result);
        }
    }

    private async Task HandleMessage(WebSocketReceiveResult result) 
    {
        switch (result.MessageType)
        {
            case WebSocketMessageType.Text:
                var stringMessage = Encoding.UTF8.GetString(_buffer, 0, result.Count);

                await _connectionService.SendToAll(stringMessage);
                return;
            case WebSocketMessageType.Close:
                await _connectionService.DisconnectAsync(_connectionId);
                return;
            case WebSocketMessageType.Binary:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CreateBuffer()
    {
        _buffer = new byte[1024 * 4];
    }
}