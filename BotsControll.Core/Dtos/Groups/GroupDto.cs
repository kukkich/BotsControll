using System.ComponentModel.DataAnnotations;
using BotsControll.Core.Models;

namespace BotsControll.Core.Dtos.Groups;

public class GroupDto
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }

    public static explicit operator GroupDto(Group group)
    {
        return new GroupDto(group);
    }

    public GroupDto(Group group)
    {
        Id = group.Id;
        Name = group.Name;
    }
}