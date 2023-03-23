using BotsControll.Api.Services.Connections;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BotsControll.Api.Services.Communication;

namespace BotsControll.Api.Controllers;

[Route("api/[controller]/[action]")]
public class MessagesController : ControllerBase
{
    private readonly IUserMessageService _userMessageService;
    
    public MessagesController(IUserMessageService userMessageService)
    {
        _userMessageService = userMessageService;
    }

    [HttpPost]
    public async Task<IActionResult> SendToUser(int userId, string message)
    {
        await _userMessageService.Send(userId, message);
        return Ok();
    }
}