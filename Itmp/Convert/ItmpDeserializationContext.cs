using Itmp.Messages;

namespace Itmp.Convert;

public class ItmpDeserializationContext
{
    public bool HasArgument { get; }
    public ItmpMessageType Type { get; } 
    public long MessageId { get; }
    public string Topic { get; }
    public object? Argument { get; set; }

    public ItmpDeserializationContext(object[] payloads)
    {
        Type = (ItmpMessageType)((long)payloads[0]);
        MessageId = (long)payloads[1];
        Topic = (string)payloads[2];
        HasArgument = payloads.Length == 4;
        Argument = HasArgument ? payloads[3] : null;
    }
}