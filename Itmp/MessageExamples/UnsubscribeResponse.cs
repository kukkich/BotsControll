namespace Itmp.MessageExamples;

public sealed class UnsubscribeResponse : ItmpMessage<object, object>
{
    public UnsubscribeResponse()
        : base(ItmpConstant.Unsubscribed)
    { }
}