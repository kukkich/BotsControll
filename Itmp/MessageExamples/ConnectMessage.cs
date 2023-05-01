namespace Itmp.MessageExamples;

public sealed class ConnectMessage : ItmpMessage<string, object>
{
    public string AuthString
    {
        get => Payload1;
        set => Payload1 = value;
    }

    public ConnectMessage()
        : base(ItmpConstant.Connect)
    { }
}