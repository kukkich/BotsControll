using System;
using System.Net;

namespace BotsControll.Api.Services.Exceptions;

public abstract class ApiException : Exception
{
    public abstract HttpStatusCode StatusCode { get; }
}