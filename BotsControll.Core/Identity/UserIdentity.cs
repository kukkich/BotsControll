using System;
using System.Linq;
using System.Security.Claims;

namespace BotsControll.Core.Identity;

public class UserIdentity
{
    public int Id { get; }
    public string Name { get; }
    
    public UserIdentity(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static explicit operator UserIdentity(ClaimsPrincipal claims)
    {
        var idClaim = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        var nameClaim = claims.FindFirst(c => c.Type == ClaimTypes.Name);
        
        EnsureNotNull(idClaim, nameClaim);

        var id = int.Parse(idClaim!.Value);
        var name = nameClaim!.Value;

        return new UserIdentity(id, name);
    }

    private static void EnsureNotNull(params object?[] objects)
    {
        if (objects.Any(@object => @object is null))
            throw new InvalidCastException();
    }
}