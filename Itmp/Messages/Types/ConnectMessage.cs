using Itmp.Convert;
using Itmp.Convert.Attributes;

namespace Itmp.Messages.Types;


[ItmpType(ItmpMessageType.Connect)]
public class ConnectMessage : ItmpMessage
{
    public override ItmpMessageType Type => ItmpMessageType.Connect;
    public override string Topic { get; }
    public override object Argument => ConnectMessageArgument;
    public ConnectMessageArgument ConnectMessageArgument { get; }

    [ConverterConstructor]
    public ConnectMessage(long messageId, string topic, ConnectMessageArgument connectMessageArgument)
        : base(messageId)
    {
        Topic = topic;
        ConnectMessageArgument = connectMessageArgument;
    }
}

public class ConnectMessageArgument
{
    public required string Token { get; init; }
}