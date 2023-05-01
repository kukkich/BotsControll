using System.Text;
using System.Text.Json;

namespace Itmp.Messages;

public abstract class ItmpMessage
{
    public long TypeLong => (long) Type;
    public abstract ItmpMessageType Type { get; }
    public long MessageId { get; }
    public abstract object Topic { get; }
    public abstract object? Argument { get; }

    protected ItmpMessage(long messageId)
    {
        MessageId = messageId;
    }

    public object[] ToTransferFormat()
    {
        return Argument != null 
            ? new [] {TypeLong, MessageId, Topic, Argument} 
            : new [] {TypeLong, MessageId, Topic};
    }

    public override bool Equals(object? obj)
    {
        if (obj is ItmpMessage other)
        {
            return other.Type == Type &&
                   other.TypeLong == TypeLong &&
                   other.MessageId == MessageId &&
                   other.Topic.Equals(Topic) &&
                   (other.Argument?.Equals(Argument) ?? false);
        }
        return false;
    }

    protected bool Equals(ItmpMessage other)
    {
        return Type == other.Type && 
               MessageId == other.MessageId && 
               Topic.Equals(other.Topic) && 
               Equals(Argument, other.Argument);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int) Type, MessageId, Topic, Argument);
    }
}