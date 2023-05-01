namespace Itmp.MessageExamples;

public sealed class ErrorMessage : ItmpMessage<int, string>
{
    public int Code
    {
        get => Payload1;
        set => Payload1 = value;
    }
    public string Cause
    {
        get => Payload2;
        set => Payload2 = value;
    }

    public ErrorMessage()
        : base(ItmpConstant.Error)
    { }
}