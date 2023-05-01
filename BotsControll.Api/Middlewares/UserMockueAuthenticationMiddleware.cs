using BotsControll.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BotsControll.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BotsControll.Core.Dtos.Login;

namespace BotsControll.Api.Middlewares;

public class UserMockueAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public UserMockueAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserRepository userRepository)
    {
        var user = (LoggedUserDto)await userRepository.GetByLogin("kukkich");
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Login),
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

        context.User = new ClaimsPrincipal(
            new ClaimsIdentity(
                claims
            )
        );

        await _next(context);
    }
}