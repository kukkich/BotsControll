using BotsControll.Core.Models.Base;

namespace BotsControll.Core.Dtos.Identity;

public class BotIdentity : IBotIdentity
{
    public string Id { get; }
    public string Name { get; }

    public BotIdentity(string name, string id)
    {
        Name = name;
        Id = id;
    }
}