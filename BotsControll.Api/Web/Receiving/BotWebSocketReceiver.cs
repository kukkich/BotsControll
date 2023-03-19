using BotsControll.Api.Services.Bots;
using BotsControll.Core.Web;
using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotsControll.Api.Web.Receiving;

public class BotWebSocketReceiver : IWebSocketReceiver
{
    private readonly BotConnection _botConnection;
    private string _connectionId = null!;
    private readonly IBotConnectionService _connectionService;
    private readonly ILogger<BotWebSocketReceiver> _logger;
    private byte[] _buffer = new byte[1024 * 4];
    private CancellationTokenSource? _receivingCancellationSource;
    private bool _isDisconnected = false;
    private readonly object _disconnectionLock = new object();

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

        using CancellationTokenSource cts = new();
        _receivingCancellationSource = cts;
        var cancellationToken = cts.Token;

        Task connectionStatusСheckingTask = null;

        try
        {
            connectionStatusСheckingTask = Task.Run(async () =>
            {
                while (!_isDisconnected)
                    await CheckConnectionIsAlive();
            });

            while (_botConnection.Connection.State == WebSocketState.Open)
            {
                var result = await _botConnection.Connection.ReceiveAsync(
                    buffer: new ArraySegment<byte>(_buffer),
                    cancellationToken: cancellationToken
                );
                await HandleMessage(result);
            }
        }
        catch (WebSocketException e)
        {
            await Disconnect(e.Message);
        }
        finally
        {
            await connectionStatusСheckingTask!;
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
                await Disconnect("closed");
                return;

            case WebSocketMessageType.Binary:
                throw new NotSupportedException();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task CheckConnectionIsAlive()
    {
        await Task.Delay(10_000);

        var wsState = _botConnection.Connection.State;

        if (wsState is WebSocketState.Aborted or WebSocketState.Closed)
        {
            var disconnectionReason = wsState switch
            {
                WebSocketState.Aborted => "aborted",
                WebSocketState.Closed => "closed",
            };

            await Disconnect(disconnectionReason);
        }
    }

    private async Task Disconnect(string? reason)
    {
        lock (_disconnectionLock)
        {
            if (_isDisconnected) return;
            if (!_receivingCancellationSource!.IsCancellationRequested)
                _receivingCancellationSource!.Cancel();
            _isDisconnected = true;
        }

        await _connectionService.DisconnectAsync(_connectionId, reason);
    }

    private void CreateBuffer()
    {
        _buffer = new byte[1024 * 4];
    }
}