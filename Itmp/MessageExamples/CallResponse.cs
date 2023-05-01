namespace Itmp.MessageExamples;

// In original <object, void>
public sealed class CallResponse : ItmpMessage<object, object>
{
    public object Result
    {
        get => Payload1;
        set => Payload1 = value;
    }

    public CallResponse()
        : base(ItmpConstant.Result)
    {
        Result = null;
    }
}