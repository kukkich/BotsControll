using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BotsControll.Api.Middlewares;

public class BotAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public BotAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var botName = context.Request.Query["Name"].FirstOrDefault();

        if (botName is null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        context.User = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Name, botName)
                }
            )
        );

        await _next(context);
    }
}