using Itmp.Convert.Attributes;
using Itmp.Messages;

namespace Itmp.Tests.Convert.Messages;

[ItmpType(ItmpMessageType.TestMessageWithObjectLikeArgument)]
public class TestMessageWithObjectArgument : ItmpMessage
{
    public override ItmpMessageType Type => ItmpMessageType.TestMessageWithObjectLikeArgument;
    public override object Topic { get; }
    public override object Argument => DifficultArgument;
    public ObjectArgument DifficultArgument { get; }

    [ConverterConstructor]
    public TestMessageWithObjectArgument(long messageId, string topic, ObjectArgument argument)
        : base(messageId)
    {
        Topic = topic;
        DifficultArgument = argument;
    }
}

public class ObjectArgument
{
    public NestedArgument Nested { get; set; }
    public long Code { get; set; }
    public string Token { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is ObjectArgument arg)
        {
            return arg.Token.Equals(Token) &&
                   arg.Code == Code &&
                   arg.Nested.SomeInformation.Equals(Nested.SomeInformation);
        }

        return false;
    }
}

public class NestedArgument
{
    public string SomeInformation { get; set; } = null!;
}