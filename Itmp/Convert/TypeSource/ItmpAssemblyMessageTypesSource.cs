using System.Reflection;
using Itmp.Messages;

namespace Itmp.Convert.TypeSource;

public class ItmpAssemblyMessageTypesSource : IMessageTypesSource
{
    public IReadOnlyCollection<Type> Types { get; }

    public ItmpAssemblyMessageTypesSource()
    {
        Types = Assembly
            .GetAssembly(typeof(ItmpMessage))!
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ItmpMessage)))
            .ToList();
    }

    public IEnumerable<Type> GetTypes()
    {
        return Types;
    }
}