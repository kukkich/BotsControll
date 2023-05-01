using BotsControll.Core.Models;

namespace BotsControll.Core.Dtos;

public class UserPublicDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    public static explicit operator UserPublicDto(User user)
    {
        return new UserPublicDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
        };
    }
}