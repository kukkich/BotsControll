using BotsControll.Core.Models.Base;

namespace BotsControll.Core.Models.Identity;

public class Token : IHaveId<string>
{
    public string Id { get; set; } = null!;
    public required string RefreshToken { get; set; }

    public required int UserId { get; set; }
    public User User { get; set; } = null!;
}