namespace Itmp.MessageExamples;

public sealed class SubscribeMessage : ItmpMessage<string, object>
{
    public SubscribeMessage()
        : base(ItmpConstant.Subscribe)
    { }
}