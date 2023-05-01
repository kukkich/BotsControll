using System.Net;
using BotsControll.Api.Services.Exceptions;

namespace BotsControll.Api.Services.Login.Exceptions;

public class UnauthorizedException : ApiException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
    public override string Message => "Не авторизован";
}