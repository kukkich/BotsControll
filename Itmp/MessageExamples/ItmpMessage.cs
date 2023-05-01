namespace Itmp.MessageExamples;

public class ItmpMessage : ItmpMessage<object, object>
{
    public ItmpMessage(ItmpConstant operationCode)
        : base(operationCode)
    { }
}

public abstract class ItmpMessage<T1, T2>
{
    // Todo what is this?
    private static long _counter = default;

    public ItmpConstant MessageCode { get; }
    public long RequestId { get; set; }
    public T1? Payload1 { get; set; } // Topic
    public T2? Payload2 { get; set; } // Args
    public Dictionary<string, object> Options { get; } = new();

    public override string ToString()
    {
        var p1 = Payload1 != null ? Payload1.GetType().Name : "null";
        var p2 = Payload2 != null ? Payload2.GetType().Name : "null";
        return $"[ {MessageCode}, {RequestId}, {p1}, {p2} ]";
    }

    protected ItmpMessage(ItmpConstant operationCode)
    {
        MessageCode = operationCode;
        RequestId = Interlocked.Increment(ref _counter);
    }
}