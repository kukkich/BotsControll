using System.Collections.Generic;
using BotsControll.Core.Models.Base;

namespace BotsControll.Core.Models;

public class Group : IHaveId<string>
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;

    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    
    public List<UserGroup> UserGroups { get; set; } = new();
    public List<BotGroup> BotGroups { get; set; } = new();
}