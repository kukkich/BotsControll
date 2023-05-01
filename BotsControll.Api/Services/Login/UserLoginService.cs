using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BotsControll.Api.Services.Login.Exceptions;
using BotsControll.Api.Services.Tokens;
using BotsControll.Core.Dtos.Login;
using BotsControll.Core.Models;
using BotsControll.Core.Repositories;
using BotsControll.Core.Services.Login;
using BotsControll.Core.Services.Password;
using Microsoft.Extensions.Logging;

namespace BotsControll.Api.Services.Login;

public class UserLoginService : IUserLoginService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserLoginService> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly TokenService _tokenService;

    public UserLoginService(IUserRepository userRepository, ILogger<UserLoginService> logger, IPasswordHasher passwordHasher, TokenService tokenService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResultModel> Register(RegistrationModel registrationModel)
    {
        //TODO Валидация email
        var (login, password, email) = registrationModel;

        var isAlreadyExist = await _userRepository.IsAnyWithLogin(login);
        if (isAlreadyExist)
            throw new UserAlreadyExistException(login);

        var user = new User
        {
            Name = login,
            Login = login,
            Email = email,
            PasswordHash = _passwordHasher.Hash(password)
        };
        await _userRepository.Add(user);

        var userDto = (LoggedUserDto)user;
        var userIdentity = GetIdentity(userDto);
        var tokenPair = _tokenService.CreateTokenPair(userIdentity);
        await _tokenService.SaveToken(user.Id, tokenPair.RefreshToken);

        _logger.LogInformation("Register role [{role.Login}].[{role.Id}]", user.Login, user.Id);

        return new LoginResultModel(
            TokenPair: tokenPair,
            User: userDto
        );
    }

    public async Task<LoginResultModel> Login(LoginModel loginModel)
    {
        var (login, password) = loginModel;

        var user = await _userRepository.TryGetByLogin(login);
        if (user == null)
            throw new NoUserWithLoginException(login);

        if (_passwordHasher.Hash(password) != user.PasswordHash)
            throw new InvalidPasswordException();

        var userDto = (LoggedUserDto)user;
        var userIdentity = GetIdentity(userDto);
        var tokenPair = _tokenService.CreateTokenPair(userIdentity);
        await _tokenService.SaveToken(user.Id, tokenPair.RefreshToken);

        return new LoginResultModel(
            TokenPair: tokenPair,
            User: userDto
        );
    }

    public async Task<string> Logout(string refreshToken)
    {
        var token = await _tokenService.RemoveToken(refreshToken);

        return token.RefreshToken;
    }

    public async Task<LoginResultModel> Refresh(string? refreshToken)
    {
        if (refreshToken is null)
            throw new UnauthorizedException();

        var isTokenValid = _tokenService.ValidateRefreshToken(refreshToken, out ClaimsPrincipal? userClaimsPrincipal);
        var tokenFromService = await _tokenService.FindToken(refreshToken);
        if (!isTokenValid || tokenFromService is null)
            throw new UnauthorizedException();

        //Todo replace userClaimsPrincipal to UserIdentity
        var user = await _userRepository.GetByLogin(userClaimsPrincipal!.Identity!.Name!);

        var userDto = (LoggedUserDto)user;
        var userIdentity = GetIdentity(userDto);
        var tokenPair = _tokenService.CreateTokenPair(userIdentity);
        await _tokenService.SaveToken(user.Id, tokenPair.RefreshToken);

        return new LoginResultModel(
            TokenPair: tokenPair,
            User: userDto
        );
    }

    private static List<Claim> GetIdentity(LoggedUserDto loggedUser)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, loggedUser.Login),
            new (ClaimTypes.Email, loggedUser.Email),
            new (ClaimTypes.NameIdentifier, loggedUser.Id.ToString())
        };

        //claims.AddRange(role.Claims.Select(claim => claim.ToClaim()));
        claims.AddRange(loggedUser.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

        return claims;
    }
}