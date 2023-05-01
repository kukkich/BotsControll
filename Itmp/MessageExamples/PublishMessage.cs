namespace Itmp.MessageExamples;

public sealed class PublishMessage : ItmpMessage<string, object>
{
    public string Topic
    {
        get => Payload1;
        set => Payload1 = value;
    }
    public object Arguments
    {
        get => Payload2;
        set => Payload2 = value;
    }

    public PublishMessage()
        : base(ItmpConstant.Publish)
    { }
}