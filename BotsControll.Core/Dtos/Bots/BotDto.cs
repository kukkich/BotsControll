using BotsControll.Core.Models;

namespace BotsControll.Core.Dtos.Bots;

public class BotDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = false;

    public UserPublicDto Owner { get; set; } = null!;

    public static explicit operator BotDto(Bot bot)
    {
        return new BotDto
        {
            Id = bot.Id,
            Name = bot.Name,
            IsActive = bot.IsActive,
            Owner = (UserPublicDto)bot.Owner,
        };
    }
}