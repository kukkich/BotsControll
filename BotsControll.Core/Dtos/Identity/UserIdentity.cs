using BotsControll.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BotsControll.Core.Dtos.Identity;

public class UserIdentity : IUserIdentity
{
    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public List<RoleDto> Roles { get; }

    public UserIdentity(int id, string name, string email, List<RoleDto> roles)
    {
        Id = id;
        Name = name;
        Email = email;
        Roles = roles;
    }

    public static explicit operator UserIdentity(ClaimsPrincipal claims)
    {
        var idClaim = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        var nameClaim = claims.FindFirst(c => c.Type == ClaimTypes.Name);
        var emailClaim = claims.FindFirst(c => c.Type == ClaimTypes.Email);
        List<RoleDto> roles = new(
            claims.FindAll(x => x.Type == ClaimTypes.Role)
                .Select(x => (RoleDto)x)
       );

        EnsureNotNull(idClaim, nameClaim, emailClaim);

        var id = int.Parse(idClaim!.Value);
        var name = nameClaim!.Value;
        var email = emailClaim!.Value;

        return new UserIdentity(id, name, email, roles);
    }

    private static void EnsureNotNull(params object?[] objects)
    {
        if (objects.Any(@object => @object is null))
            throw new InvalidCastException();
    }
}