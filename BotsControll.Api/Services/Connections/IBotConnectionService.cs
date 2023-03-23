using BotsControll.Core.Web;
using System.Threading.Tasks;

namespace BotsControll.Api.Services.Connections;

public interface IBotConnectionService
{
    public Task<string> AcceptConnectionAsync(BotConnection connection);
    public Task DisconnectAsync(string id, string? reason);
}