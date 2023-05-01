using Itmp.Convert.TypeSource;

namespace Itmp.Tests.Convert.Messages;

public class TestMessageTypesSource : IMessageTypesSource
{
    private readonly IEnumerable<Type> _types;

    public TestMessageTypesSource(IEnumerable<Type> types)
    {
        _types = types;
    }

    public IEnumerable<Type> GetTypes()
    {
        return _types;
    }
}