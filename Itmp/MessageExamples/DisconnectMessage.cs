namespace Itmp.MessageExamples;

public sealed class DisconnectMessage : ItmpMessage<int, string>
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

    public DisconnectMessage()
        : base(ItmpConstant.Disconnect)
    { }
}