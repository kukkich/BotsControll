using BotsControll.Api.Web.Receiving;
using Microsoft.AspNetCore.Http;
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
        
        
    }
}