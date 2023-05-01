using BotsControll.Core.Models.Base;
using BotsControll.Core.Models.Identity;
using System.Collections.Generic;

namespace BotsControll.Core.Models;

public class User : IUserIdentity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int AccessFailedCount { get; set; } = 0;

    public List<IdentityClaim> Claims { get; set; } = new();
    public List<UserRole> UserRoles { get; set; } = new();

    public List<Bot> Bots { get; set; } = new();
    public List<Group> OwnedGroups { get; set; } = new();
    public List<UserGroup> UserGroups { get; set; } = new();
    public List<ManagementRecord> ManagementRecords { get; set; } = new();
}