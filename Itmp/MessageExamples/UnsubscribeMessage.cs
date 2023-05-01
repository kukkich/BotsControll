namespace Itmp.MessageExamples;

public sealed class UnsubscribeMessage : ItmpMessage<string, object>
{
    public UnsubscribeMessage()
        : base(ItmpConstant.Unsubscribe)
    { }
}