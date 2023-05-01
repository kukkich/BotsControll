namespace Itmp.MessageExamples;

public sealed class DescribeMessage : ItmpMessage<string, object>
{
    public string Topic
    {
        get => Payload1;
        set => Payload1 = value;
    }

    public DescribeMessage()
        : base(ItmpConstant.Describe)
    { }
}