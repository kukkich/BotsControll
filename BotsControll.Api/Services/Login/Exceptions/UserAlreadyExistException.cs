using System.Net;
using BotsControll.Api.Services.Exceptions;

namespace BotsControll.Api.Services.Login.Exceptions;

public class UserAlreadyExistException : ApiException
{
    public override string Message => $"Пользователь {_existedLogin} уже существует";
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    private readonly string _existedLogin;

    public UserAlreadyExistException(string login)
    {
        _existedLogin = login;
    }
}