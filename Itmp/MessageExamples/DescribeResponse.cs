namespace Itmp.MessageExamples;

public sealed class DescribeResponse : ItmpMessage<object, object>
{
    public object Response => Payload1;

    public DescribeResponse()
        : base(ItmpConstant.Description)
    { }

    public void SetResponse(string response)
    {
        SetResponse(new[] { response });
    }

    public void SetResponse(IEnumerable<string> response)
    {
        Payload1 = response;
    }
}