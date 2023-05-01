namespace BotsControll.Core.Models;

public class BotGroup
{
    public string BotId { get; set; } = null!;
    public Bot Bot { get; set; } = null!;
    public string GroupId { get; set; } = null!;
    public Group Group { get; set; } = null!;
}