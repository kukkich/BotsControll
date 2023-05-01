namespace Itmp.MessageExamples;

public sealed class ConnectResponse : ItmpMessage<string, object>
{
    public string AuthString
    {
        get => Payload1;
        set => Payload1 = value;
    }

    public ConnectResponse()
        : base(ItmpConstant.Connected)
    { }
}