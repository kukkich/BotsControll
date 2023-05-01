namespace Itmp.MessageExamples;

public sealed class PublishResponse : ItmpMessage<object, object>
{
    public PublishResponse()
        : base(ItmpConstant.Published)
    { }
}