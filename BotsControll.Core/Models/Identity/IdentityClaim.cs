using System.Security.Claims;

namespace BotsControll.Core.Models.Identity;

public class IdentityClaim
{
    public int Id { get; set; }
    public required string Type { get; set; }
    public required string Value { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public Claim ToClaim()
    {
        return new Claim(Type, Value);
    }

    public static implicit operator Claim(IdentityClaim identityClaim)
    {
        return identityClaim.ToClaim();
    }
}