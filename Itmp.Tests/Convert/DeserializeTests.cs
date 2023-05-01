using Itmp.Convert;
using Itmp.Messages;
using Itmp.Tests.Convert.Messages;

namespace Itmp.Tests.Convert;
using Encoding;

public class DeserializeTests
{
    public ItmpConverter Converter { get; set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Converter = new ItmpConverter(new TestMessageTypesSource(new []
            {
                typeof(TestMessageWithoutArgument), 
                typeof(TestMessageWithLongArgument), 
                typeof(TestMessageWithObjectArgument)
            }
        )); 
    }

    [Test]
    public void GIVEN_MessageWithoutArgument_THEN_CorrectDeserializationExpected()
    {
        var expectedType = ItmpMessageType.TestMessageWithoutArgument;
        var expectedTypeLong = (long) expectedType;
        var expectedMessageId = 27154L;
        var expectedTopic = "head/main";

        var message = new object[] { expectedTypeLong, expectedMessageId, expectedTopic };
        var encoded = Cbor.Encode(message);

        var deserializedMessage = Converter.Deserialize(encoded);

        Assert.Multiple(() =>
        {
            Assert.That(deserializedMessage.Type, Is.EqualTo(expectedType));
            Assert.That(deserializedMessage.TypeLong, Is.EqualTo(expectedTypeLong));
            Assert.That(deserializedMessage.MessageId, Is.EqualTo(expectedMessageId));
            Assert.That(deserializedMessage.Topic, Is.EqualTo(expectedTopic));
            Assert.That(deserializedMessage.Argument, Is.Null);
        });
    }

    [Test]
    public void GIVEN_MessageWithLongArgument_THEN_CorrectDeserializationExpected()
    {
        var expectedType = ItmpMessageType.TestMessageWithLongArgument;
        var expectedTypeLong = (long)expectedType;
        var expectedMessageId = 27154L;
        var expectedTopic = "something/something_else";
        var expectedArgument = 910312022L;


        var message = new object[] { expectedTypeLong, expectedMessageId, expectedTopic, expectedArgument};
        var encoded = Cbor.Encode(message);

        var deserializedMessage = Converter.Deserialize(encoded);

        Assert.Multiple(() =>
        {
            Assert.That(deserializedMessage.Type, Is.EqualTo(expectedType));
            Assert.That(deserializedMessage.TypeLong, Is.EqualTo(expectedTypeLong));
            Assert.That(deserializedMessage.MessageId, Is.EqualTo(expectedMessageId));
            Assert.That(deserializedMessage.Topic, Is.EqualTo(expectedTopic));

            Assert.That(deserializedMessage, Is.TypeOf(typeof(TestMessageWithLongArgument)));
            Assert.That(deserializedMessage.Argument, Is.TypeOf(typeof(long)));
            Assert.That(deserializedMessage.Argument, Is.EqualTo(expectedArgument));
        });
    }

    [Test]
    public void GIVEN_MessageWithObjectLikeArgument_THEN_CorrectDeserializationExpected()
    {
        var expectedType = ItmpMessageType.TestMessageWithObjectLikeArgument;
        var expectedTypeLong = (long)expectedType;
        var expectedMessageId = 27154L;
        var expectedTopic = "something/something_else";
        var expectedArgument = new ObjectArgument()
        {
            Code = 312,
            Nested = new NestedArgument()
            {
                SomeInformation = "This is secret information"
            },
            Token = "qwertyuiop[]123"
        };


        var message = new object[] { expectedTypeLong, expectedMessageId, expectedTopic, expectedArgument };
        var encoded = Cbor.Encode(message);

        var deserializedMessage = Converter.Deserialize(encoded);

        Assert.Multiple(() =>
        {
            Assert.That(deserializedMessage.Type, Is.EqualTo(expectedType));
            Assert.That(deserializedMessage.TypeLong, Is.EqualTo(expectedTypeLong));
            Assert.That(deserializedMessage.MessageId, Is.EqualTo(expectedMessageId));
            Assert.That(deserializedMessage.Topic, Is.EqualTo(expectedTopic));

            Assert.That(deserializedMessage, Is.TypeOf(typeof(TestMessageWithObjectArgument)));
            Assert.That(deserializedMessage.Argument, Is.TypeOf(typeof(ObjectArgument)));
            Assert.That(deserializedMessage.Argument, Is.EqualTo(expectedArgument));
        });
    }

}