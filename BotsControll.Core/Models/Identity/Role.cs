using System.Collections.Generic;

namespace BotsControll.Core.Models.Identity;

public class Role
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public List<UserRole> UserRoles { get; set; } = new();
}