using BotsControll.Api.Extensions;
using BotsControll.Api.Services.ModelsServices;
using BotsControll.Core.Dtos.Bots;
using BotsControll.Core.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BotsControll.Api.Controllers.ModelsControllers;

//[Authorize]
[Route("api/[controller]/[action]")]
public class BotsController : ControllerBase
{
    private readonly BotService _botService;

    public BotsController(BotService botService)
    {
        _botService = botService;
    }

    [HttpGet]
    [ActionName("own")]
    public async Task<IActionResult> GetOwnBots()
    {
        IUserIdentity user = HttpContext.GetUserIdentity();
        var bots = await _botService.GetOwned(user);

        return Ok(bots.Select(x => (BotDto)x));
    }

    [HttpGet]
    [ActionName("available")]
    public async Task<IActionResult> GetAvailableBots()
    {
        IUserIdentity user = HttpContext.GetUserIdentity();
        var bots = await _botService.GetAvailable(user);

        return Ok(bots.Select(x => (BotDto)x));
    }

    [HttpPost]
    public async Task<IActionResult> Create(BotCreationDto bot)
    {
        IUserIdentity user = HttpContext.GetUserIdentity();
        var createdBot = await _botService.Create(bot, user);

        return Ok(createdBot);
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(string id)
    {
        IUserIdentity user = HttpContext.GetUserIdentity();
        var bot = await _botService.Remove(id, user);

        return Ok(bot);
    }
}