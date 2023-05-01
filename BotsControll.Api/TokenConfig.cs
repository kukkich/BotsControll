using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace BotsControll.Api;

public static class TokenConfig
{
    public const string Issuer = "Queuing.Api";
    public const string Audience = "Queuing.Client";
    public static TimeSpan LifeTime => TimeSpan.FromMinutes(15);
    public static TimeSpan RefreshLifeTime => TimeSpan.FromDays(30);

    private const string AccessKey = "24e6b622-579e-44dd-a5de-3836da322ad5";
    private const string RefreshKey = "24e6b622-579e-44dd-a5de-5436da322ad5";

    public static SymmetricSecurityKey GetSymmetricSecurityAccessKey() => new(Encoding.UTF8.GetBytes(AccessKey));
    public static SymmetricSecurityKey GetSymmetricSecurityRefreshKey() => new(Encoding.UTF8.GetBytes(RefreshKey));
}