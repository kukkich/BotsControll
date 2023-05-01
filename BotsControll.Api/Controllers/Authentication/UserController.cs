using System;
using System.Linq;
using System.Threading.Tasks;
using BotsControll.Api.Extensions;
using BotsControll.Core.Dtos.Login;
using BotsControll.Core.Repositories;
using BotsControll.Core.Services.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BotsControll.Api.Controllers.Authentication;

[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private const string RefreshTokenCookieKey = "refreshToken";
    private readonly IUserRepository _userRepository;
    private readonly IUserLoginService _userLoginService;

    public UserController(IUserRepository userRepository, IUserLoginService userLoginService)
    {
        _userRepository = userRepository;
        _userLoginService = userLoginService;
    }

    [HttpPost]
    public async Task<IActionResult> Registration(RegistrationModel registrationModel)
    {
        var loginResult = await _userLoginService.Register(registrationModel);
        HttpContext.Response.Cookies.Append(
            key: RefreshTokenCookieKey,
            value: loginResult.TokenPair.RefreshToken,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(30),
                HttpOnly = true
            }
        );
        return Ok(loginResult);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        var loginResult = await _userLoginService.Login(loginModel);

        HttpContext.Response.Cookies.Append(
            key: RefreshTokenCookieKey,
            value: loginResult.TokenPair.RefreshToken,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(30),
                HttpOnly = true
            }
        );
        return Ok(loginResult);

    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        // Todo Проверка кук на null
        string? refreshToken = HttpContext.Request.Cookies[RefreshTokenCookieKey];
        await _userLoginService.Logout(refreshToken);

        HttpContext.Response.Cookies.Delete(RefreshTokenCookieKey);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Refresh()
    {
        string? refreshToken = HttpContext.Request.Cookies[RefreshTokenCookieKey];

        var loginResult = await _userLoginService.Refresh(refreshToken);

        HttpContext.Response.Cookies.Append(
            key: RefreshTokenCookieKey,
            value: loginResult.TokenPair.RefreshToken,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(30),
                HttpOnly = true
            }
        );
        return Ok(loginResult);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Identity()
    {
        // TODO Extract in AuthService
        var user = HttpContext.GetUserIdentity();

        return Ok(user);
    }

    [HttpGet]
    [Route("/api/[controller]/[action]")]
    public async Task<IActionResult> GetAll()
    {
        var users = (await _userRepository.GetAll())
            .Select(u => (LoggedUserDto)u)
            .ToList();

        return Ok(users);
    }
}