namespace Itmp.MessageExamples;

public sealed class SubscribeResponse : ItmpMessage<object, object>
{
    public SubscribeResponse()
        : base(ItmpConstant.Subscribed)
    { }
}