using Itmp.Convert;
using Itmp.Messages;
using Itmp.Tests.Convert.Messages;

namespace Itmp.Tests.Convert;
using Encoding;

public class ThereAndBackConvert
{
    public ItmpConverter Converter { get; set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Converter = new ItmpConverter(new TestMessageTypesSource(new[]
            {
                typeof(TestMessageWithoutArgument),
                typeof(TestMessageWithLongArgument),
                typeof(TestMessageWithObjectArgument)
            }
        ));
    }

    [Test]
    public void GIVEN_Message_WHEN_SerializedAndThenDeserialized_THEN_TheSameMessageExpected()
    {
        ItmpMessage expected = new TestMessageWithObjectArgument(321L, "some/topic", new ObjectArgument()
        {
            Code = 459L,
            Token = "toooooken",
            Nested = new NestedArgument()
            {
                SomeInformation = "super information"
            }
        });

        var bytes = Converter.Serialize(expected);
        var actual = Converter.Deserialize(bytes);

        Assert.Multiple(() =>
        {
            Assert.AreEqual(actual, expected);
        });
    }
}