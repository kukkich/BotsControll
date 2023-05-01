namespace Itmp.MessageExamples;

public sealed class CancelMessage : ItmpMessage<object, object>
{
    public CancelMessage()
        : base(ItmpConstant.Cancel)
    { }
}