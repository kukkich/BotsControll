using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BotsControll.Core.Dtos.Groups;

namespace BotsControll.Core.Dtos.Bots;

public class BotCreationDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public List<GroupDto> Groups { get; set; } = new();
}