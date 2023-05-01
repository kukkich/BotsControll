namespace Itmp.MessageExamples;

public sealed class EventMessage : ItmpMessage<string, object>
{
    public string Topic
    {
        get => Payload1;
        set => Payload1 = value;
    }
    public object Arguments
    {
        get => Payload2;
        set => Payload2 = value;
    }

    public EventMessage()
        : base(ItmpConstant.Event)
    { }
}