using BotsControll.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

    public async Task Invoke(HttpContext context, BotsControlContext dbContext)
    {
        var botName = context.Request.Query["Name"].FirstOrDefault();
        var bot = await dbContext.Bots.FirstOrDefaultAsync(x => x.Name == botName);

        if (botName is null || bot is null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        context.User = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, bot.Id),
                    new Claim(ClaimTypes.Name, bot.Name)
                }
            )
        );

        await _next(context);
    }
}