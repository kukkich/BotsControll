using System.Linq;
using System.Threading.Tasks;
using BotsControll.Api.Extensions;
using BotsControll.Api.Services.ModelsServices;
using BotsControll.Core.Dtos.Groups;
using BotsControll.Core.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BotsControll.Api.Controllers.ModelsControllers;

//[Authorize]
[Route("api/[controller]/[action]")]
public class GroupController : ControllerBase
{
    private readonly GroupService _groupService;

    public GroupController(GroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet]
    [ActionName("own")]
    public async Task<IActionResult> Get()
    {
        IUserIdentity user = HttpContext.GetUserIdentity();
        var groups = await _groupService.GetOwned(user);

        return Ok(groups.Select(x => (GroupDto)x));
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupCreationDto group)
    {
        var user = HttpContext.GetUserIdentity();
        var createdGroup = await _groupService.Create(group, user);

        return Ok(createdGroup);
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(string id)
    {
        IUserIdentity user = HttpContext.GetUserIdentity();
        var bot = await _groupService.Remove(id, user);

        return Ok(bot);
    }
}