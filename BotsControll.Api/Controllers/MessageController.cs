using BotsControll.Api.Services.Communication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BotsControll.Api.Controllers;

[Route("api/[controller]/[action]")]
public class MessageController : ControllerBase
{
    private readonly IUserMessageService _userMessageService;
    private readonly IBotMessageService _borBotMessageService;

    public MessageController(IUserMessageService userMessageService, IBotMessageService borBotMessageService)
    {
        _userMessageService = userMessageService;
        _borBotMessageService = borBotMessageService;
    }

    [HttpPost]
    [ActionName("user")]
    public async Task<IActionResult> SendToUser(int userId, string message)
    {
        await _userMessageService.Send(userId, message);
        return Ok();
    }

    [HttpPost]
    [ActionName("bot")]
    public async Task<IActionResult> SendToBot(string connectionId, string message)
    {
        await _borBotMessageService.SendTo(connectionId, message);
        return Ok();
    }

}