using BotsControll.Core.Web;

namespace BotsControll.Core.Dtos.Bots;

public record BotConnectionDto(string Name, string Id)
{
    public static explicit operator BotConnectionDto(BotConnection botConnection)
    {
        return new BotConnectionDto(botConnection.Bot.Name, botConnection.Bot.Id);
    }
}
