using System.Collections.Generic;

namespace BotsControll.Api.Web.Connections;

public interface IBotConnectionRepository
{
    public IEnumerable<KeyValuePair<string, BotConnection>> All { get; }
    public IEnumerable<BotConnection> AllConnection { get; }

    public string GetId(BotConnection socket);
    public BotConnection GetById(string id);
    public string Add(BotConnection socket);
    public BotConnection Remove(string id);

    public IEnumerable<string> GetAllIds();
}