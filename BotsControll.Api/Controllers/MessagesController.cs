using BotsControll.Api.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BotsControll.Api.Controllers;

[Route("api/[controller]/[action]")]
public class MessagesController : ControllerBase
{
    private readonly UserConnectionService _userService;

    public MessagesController(UserConnectionService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> SendToUser(int userId, string message)
    {
        await _userService.Send(userId, message);
        return Ok();
    }
}