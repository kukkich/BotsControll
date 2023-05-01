using System;

namespace BotsControll.Core.Models;

public class StateRecord
{
    public int Id { get; set; }
    public string Info { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public string JournalId { get; set; } = null!;
    public Journal Journal { get; set; } = null!;
}