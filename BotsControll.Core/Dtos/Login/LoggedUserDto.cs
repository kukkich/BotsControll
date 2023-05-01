using System.Collections.Generic;
using System.Linq;
using BotsControll.Core.Models;

namespace BotsControll.Core.Dtos.Login;

public class LoggedUserDto
{
    public int Id { get; }
    public string Name { get; }
    public string Login { get; }
    public string Email { get; }
    public List<RoleDto> Roles { get; }

    public static explicit operator LoggedUserDto(User user)
    {
        return new LoggedUserDto(user);
    }

    public LoggedUserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Login = user.Login;
        Email = user.Email;
        Roles = user.UserRoles.DistinctBy(ur => ur.RoleId)
            .Select(ur => (RoleDto)ur.Role)
            .ToList();
    }
}