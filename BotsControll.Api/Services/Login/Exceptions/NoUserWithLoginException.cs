using System.Net;
using BotsControll.Api.Services.Exceptions;

namespace BotsControll.Api.Services.Login.Exceptions;

public class NoUserWithLoginException : ApiException
{
    public override string Message => $"Пользователя {_nonExistedLogin} не существует";
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    private readonly string _nonExistedLogin;

    public NoUserWithLoginException(string login)
    {
        _nonExistedLogin = login;
    }
}