using System;
using System.Collections.Generic;
using BotsControll.Core.Models.Base;

namespace BotsControll.Core.Models;

public class Journal : IHaveId<string>
{
    public string Id { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    public bool IsClosed => ClosedAt is not null;

    public string BotId { get; set; } = null!;
    public Bot Bot { get; set; } = null!;

    public List<StateRecord> StateRecords { get; set; } = new();
    public List<ManagementRecord> ManagementRecords { get; set; } = new();
}