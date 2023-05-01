using BotsControll.Core.Models.Base;
using BotsControll.Core.Web;
using System.Threading.Tasks;

namespace BotsControll.Api.Services.Connections;

public interface IBotConnectionService
{
    public Task AcceptConnectionAsync(BotConnection connection);
    public Task DisconnectAsync(IBotIdentity bot, string? reason);
}