using System.Net;
using BotsControll.Api.Services.Exceptions;

namespace BotsControll.Api.Services.Login.Exceptions;

public class InvalidPasswordException : ApiException
{
    public override string Message => $"Неверный павроль";
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}