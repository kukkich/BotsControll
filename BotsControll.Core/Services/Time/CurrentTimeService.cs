using System;

namespace BotsControll.Core.Services.Time;

public class CurrentTimeService : ITimeService
{
    public DateTime GetTime() => DateTime.Now;
}