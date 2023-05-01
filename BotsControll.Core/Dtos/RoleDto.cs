using BotsControll.Core.Models.Identity;
using System;
using System.Security.Claims;

namespace BotsControll.Core.Dtos;

public class RoleDto
{
    public string Name { get; set; }

    public static explicit operator RoleDto(Role role)
    {
        return new RoleDto(role);
    }
    public static explicit operator RoleDto(Claim claim)
    {
        if (claim.Type != ClaimTypes.Role) throw new InvalidCastException();
        return new RoleDto(claim);
    }
    public RoleDto(Role role)
    {
        Name = role.Name;
    }

    private RoleDto(Claim claim)
    {
        Name = claim.Value;
    }
}