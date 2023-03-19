using BotsControll.Core.Identity;
using System.Collections.Generic;

namespace BotsControll.Core.Web.Dtos;

public record BotConnectionDto(string ConnectionId, IBotIdentity Bot)
{
    public static explicit operator BotConnectionDto(KeyValuePair<string, BotConnection> kv)
    {
        return new BotConnectionDto(kv.Key, kv.Value.Bot);
    }
}