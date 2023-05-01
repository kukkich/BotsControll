using Itmp.Messages;

namespace Itmp.Convert.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ItmpTypeAttribute : Attribute
{
    public ItmpMessageType Type { get; }

    public ItmpTypeAttribute(ItmpMessageType type)
    {
        Type = type;
    }
}