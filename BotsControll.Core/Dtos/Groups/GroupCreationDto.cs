using System.ComponentModel.DataAnnotations;

namespace BotsControll.Core.Dtos.Groups;

public class GroupCreationDto
{
    [Required]
    public string Name { get; set; } = null!;
}