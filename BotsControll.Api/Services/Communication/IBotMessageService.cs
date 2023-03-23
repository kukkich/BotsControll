using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotsControll.Api.Services.Communication;

public interface IBotMessageService
{
    public Task SendToAll(string message);
    public Task SendAllExcept(string message, IEnumerable<string> exceptedIds);
    public Task SendTo(string connectionId, string message);
}