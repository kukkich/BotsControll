using BotsControll.Core.Web;
using System.Collections.Generic;

namespace BotsControll.Api.Web.Connections;

public interface IBotConnectionRepository
{
    public IEnumerable<BotConnection> All { get; }
    public BotConnection GetByBotId(string id);
    public void Add(string botId, BotConnection connection);
    public BotConnection Remove(string id);
}