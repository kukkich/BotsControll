using System;

namespace BotsControll.Core.Models;

public class ManagementRecord
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Content { get; set; } = null!;

    public string JournalId { get; set; } = null!;
    public Journal Journal { get; set; } = null!;

    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
}