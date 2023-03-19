using BotsControll.Core.Web;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotsControll.Api.Services.Bots;

public interface IBotConnectionService
{
    public Task<string> AcceptConnectionAsync(BotConnection connection);
    public Task DisconnectAsync(string id, string? reason);
    public Task SendToAll(string message);
    public Task SendAllExcept(string message, IEnumerable<string> exceptedIds);
    public Task SendTo(string connectionId, string message);
}