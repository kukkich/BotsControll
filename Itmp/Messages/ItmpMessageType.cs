namespace Itmp.Messages;

public enum ItmpMessageType
{
    Connect = 0,
    Connected = 1,
    Abort = 2,
    KeepAlive = 3,
    Disconnect = 4,
    Error = 5,
    Describe = 6,
    Description = 7,
    Call = 8,
    Result = 9,
    Arguments = 10,
    Progress = 11,
    Cancel = 12,
    Event = 13,
    Publish = 14,
    Published = 15,
    Subscribe = 16,
    Subscribed = 17,
    Unsubscribe = 18,
    Unsubscribed = 19,
    
    TestMessageWithoutArgument = -1,
    TestMessageWithLongArgument = -2,
    TestMessageWithObjectLikeArgument = -3,
}