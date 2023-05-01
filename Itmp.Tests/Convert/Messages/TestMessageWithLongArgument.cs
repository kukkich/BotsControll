using Itmp.Convert;
using Itmp.Convert.Attributes;
using Itmp.Messages;

namespace Itmp.Tests.Convert.Messages;

[ItmpType(ItmpMessageType.TestMessageWithLongArgument)]
public class TestMessageWithLongArgument : ItmpMessage
{
    public override ItmpMessageType Type => ItmpMessageType.TestMessageWithLongArgument;
    public override object Topic { get; }
    public override object Argument => LongArgument;
    public long LongArgument { get; }

    [ConverterConstructor]
    public TestMessageWithLongArgument(long messageId, string topic, long argument)
        : base(messageId)
    {
        Topic = topic;
        LongArgument = argument;
    }
}