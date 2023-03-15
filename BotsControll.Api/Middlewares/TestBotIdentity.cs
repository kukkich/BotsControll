using BotsControll.Core.Identity;

namespace BotsControll.Api.Middlewares;

public class TestBotIdentity : IBotIdentity
{
    public string Name { get; }

    public TestBotIdentity(string name)
    {
        Name = name;
    }
}