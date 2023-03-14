using System.Collections.Generic;

namespace BotsControll.Api.Web.Connections;

public interface IBotConnectionRepository
{
    public BotConnection GetSocketById(string id);

    public IEnumerable<KeyValuePair<string, BotConnection>> All { get; }
    public IEnumerable<BotConnection> AllConnection { get; }

    public string GetId(BotConnection socket);

    public string Add(BotConnection socket);

    public IEnumerable<string> GetAllIds();

    public BotConnection Remove(string id);
}