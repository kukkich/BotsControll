using BotsControll.Api.Web.Connections;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BotsControll.Api.Controllers;

[Route("api/[controller]/[action]")]
public class ConnectionsController : ControllerBase
{
    private readonly UserConnectionRepository _userConnections;
    private readonly IBotConnectionRepository _botConnections;

    public ConnectionsController(UserConnectionRepository userConnections, IBotConnectionRepository botConnections)
    {
        _userConnections = userConnections;
        _botConnections = botConnections;
    }

    [HttpGet]
    [ActionName("users")]
    public IActionResult GetUsers()
    {
        return Ok(_userConnections.GetAll());
    }

    [HttpGet]
    [ActionName("bots")]
    public IActionResult GetBots()
    {
        return Ok(_botConnections.All.Select(kv => new
        {
            kv.Key,
            kv.Value.Bot
        }));
    }

}