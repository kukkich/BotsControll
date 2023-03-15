using BotsControll.Api.Web.Connections;
using Microsoft.AspNetCore.Mvc;

namespace BotsControll.Api.Controllers;

[Route("api/[controller]/[action]")]
public class ConnectionsController : ControllerBase
{
    private readonly UserConnectionRepository _userConnections;

    public ConnectionsController(UserConnectionRepository userConnections)
    {
        _userConnections = userConnections;
    }

    [HttpGet]
    [ActionName("users")]
    public IActionResult GetUsers()
    {
        return Ok(_userConnections.GetAll());
    }

}