using Itmp.Encoding;

namespace Itmp.Tests.Cbor;

public class Float64Tests
{
    #region Cases
    [TestCase(0, 86)]
    [TestCase(1, 215)]
    [TestCase(2, 79)]
    [TestCase(3, 183)]
    [TestCase(4, 76)]
    [TestCase(5, 214)]
    [TestCase(6, 35)]
    [TestCase(7, 174)]
    [TestCase(8, 114)]
    [TestCase(9, 171)]
    [TestCase(10, 215)]
    [TestCase(11, 218)]
    [TestCase(12, 60)]
    [TestCase(13, 82)]
    #endregion
    public void GetUint8WithMultipleSet(int index, byte expected)
    {
        BytePool pool = new(14);
        pool.SetFloat64(1, 412941179e-14);
        pool.SetFloat64(2, 418441141e-41);
        pool.SetFloat64(0, 218991141e102);
        pool.SetFloat64(4, 141990e57);
        pool.SetFloat64(6, 81818181e-144);


        var oldView = pool.AsString;
        var actual = pool.GetUint8(index);
        var viewAfterGet = pool.AsString;

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(oldView, Is.EqualTo(viewAfterGet));
        });
    }
}