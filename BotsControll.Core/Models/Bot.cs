using BotsControll.Core.Models.Base;
using System.Collections.Generic;

namespace BotsControll.Core.Models;

public class Bot : IBotIdentity
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = false;

    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public List<Journal> Journals { get; set; } = new();
    public List<BotGroup> BotGroups { get; set; } = new();
}