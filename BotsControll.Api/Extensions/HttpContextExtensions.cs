using BotsControll.Core.Dtos.Identity;
using Microsoft.AspNetCore.Http;
using System;

namespace BotsControll.Api.Extensions;

public static class HttpContextExtensions
{
    public static UserIdentity GetUserIdentity(this HttpContext context)
    {
        if (context.User is null)
            throw new InvalidOperationException();

        return (UserIdentity)context.User;
    }
}