using Itmp.Convert;
using Itmp.Convert.Attributes;
using Itmp.Messages;

namespace Itmp.Tests.Convert.Messages;

[ItmpType(ItmpMessageType.TestMessageWithoutArgument)]
public class TestMessageWithoutArgument : ItmpMessage
{
    public override ItmpMessageType Type => ItmpMessageType.TestMessageWithoutArgument;
    public override object Topic { get; }
    public override object? Argument { get; }

    [ConverterConstructor]
    public TestMessageWithoutArgument(long messageId, string topic)
        : base(messageId)
    {
        Topic = topic;
    }


}