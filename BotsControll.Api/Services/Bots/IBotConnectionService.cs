using System.Collections.Generic;
using System.Threading.Tasks;
using BotsControll.Api.Web.Connections;

namespace BotsControll.Api.Services.Bots;

public interface IBotConnectionService
{
    public string AcceptConnection(BotConnection connection); 
    public Task DisconnectAsync(string id);
    public Task SendToAll(string message);
    public Task SendAllExcept(string message, IEnumerable<string> exceptedIds);
}