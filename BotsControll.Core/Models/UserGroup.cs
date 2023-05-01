namespace BotsControll.Core.Models;

public class UserGroup
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string GroupId { get; set; } = null!;
    public Group Group { get; set; } = null!;
}