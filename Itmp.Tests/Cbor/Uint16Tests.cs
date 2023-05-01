using Itmp.Encoding;

namespace Itmp.Tests.Cbor;

public class Uint16Tests
{
    #region Cases
    [TestCase(0, 93)]
    [TestCase(1, 6)]
    [TestCase(2, 0)]
    [TestCase(3, 0)]
    #endregion
    public void GetUint8(int index, byte expected)
    {
        BytePool pool = new(4);

        pool.SetUint16(0, 23814);
        var actual = pool.GetUint8(index);

        Assert.That(actual, Is.EqualTo(expected));
    }

    #region Cases
    [TestCase(0, 0)]
    [TestCase(1, 0)]
    [TestCase(2, 0)]
    [TestCase(3, 93)]
    [TestCase(4, 36)]
    [TestCase(5, 156)]
    [TestCase(6, 0)]
    [TestCase(7, 0)]
    [TestCase(8, 0)]
    [TestCase(9, 5)]
    #endregion
    public void GetUint8WithMultipleSet(int index, byte expected)
    {
        BytePool pool = new(10);
        pool.SetUint16(3, 23814);
        pool.SetUint16(4, 9372);
        pool.SetUint16(8, 5);
        var actual = pool.GetUint8(index);

        Assert.That(actual, Is.EqualTo(expected));
    }

    #region Cases
    [TestCase(0, (UInt16)29854)]
    [TestCase(1, (UInt16)40496)]
    [TestCase(2, (UInt16)12345)]
    [TestCase(3, (UInt16)14631)]
    [TestCase(4, (UInt16)10013)]
    [TestCase(5, (UInt16)7424)]
    [TestCase(6, (UInt16)0)]
    [TestCase(7, (UInt16)38)]
    [TestCase(8, (UInt16)9814)]
    #endregion
    public void GetUint16WithMultipleSet(int index, UInt16 expected)
    {
        BytePool pool = new(10);
        pool.SetUint16(1, 23456);
        pool.SetUint16(2, 12345);
        pool.SetUint16(0, 29854);
        pool.SetUint16(4, 10013);
        pool.SetUint16(8, 9814);

        var oldView = pool.AsString;
        var actual = pool.GetUint16(index);
        var viewAfterGet = pool.AsString;

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(oldView, Is.EqualTo(viewAfterGet));
        });
    }

    #region Cases
    [TestCase(0, (UInt32)1956524089)]
    [TestCase(1, (UInt32)2653960487)]
    [TestCase(2, (UInt32)809051933)]
    [TestCase(3, (UInt32)958864640)]
    [TestCase(4, (UInt32)656211968)]
    [TestCase(5, (UInt32)486539302)]
    [TestCase(6, (UInt32)9814)]
    #endregion
    public void GetUint32WithMultipleSet(int index, UInt32 expected)
    {
        BytePool pool = new(10);
        pool.SetUint16(1, 23456);
        pool.SetUint16(2, 12345);
        pool.SetUint16(0, 29854);
        pool.SetUint16(4, 10013);
        pool.SetUint16(8, 9814);

        var oldView = pool.AsString;
        var actual = pool.GetUint32(index);
        var viewAfterGet = pool.AsString;

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(oldView, Is.EqualTo(viewAfterGet));
        });
    }

    #region Cases
    [TestCase(0, (float)1.0026379093197526e+32)]
    [TestCase(1, (float)-9.329181247475295e-21)]
    [TestCase(2, (float)6.735819124692455e-10)]
    [TestCase(3, (float)0.00015937164425849915)]
    [TestCase(4, (float)2.1788126858268697e-15)]
    [TestCase(5, (float)1.6940735685474867e-21)]
    [TestCase(6, (float)1.3752343128883755e-41)]
    #endregion
    public void GetFloat32WithMultipleSet(int index, float expected)
    {
        BytePool pool = new(10);
        pool.SetUint16(1, 23456);
        pool.SetUint16(2, 12345);
        pool.SetUint16(0, 29854);
        pool.SetUint16(4, 10013);
        pool.SetUint16(8, 9814);

        var oldView = pool.AsString;
        var actual = pool.GetFloat32(index);
        var viewAfterGet = pool.AsString;

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(oldView, Is.EqualTo(viewAfterGet));
        });
    }

    #region Cases
    [TestCase(0, 5.53319608013083e+253)]
    [TestCase(1, -2.817217008543837e-163)]
    [TestCase(2, 2.1722370172773182e-76)]
    #endregion
    public void GetFloat64WithMultipleSet(int index, double expected)
    {
        BytePool pool = new(10);
        pool.SetUint16(1, 23456);
        pool.SetUint16(2, 12345);
        pool.SetUint16(0, 29854);
        pool.SetUint16(4, 10013);
        pool.SetUint16(8, 9814);

        var oldView = pool.AsString;
        var actual = pool.GetFloat64(index);
        var viewAfterGet = pool.AsString;

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(oldView, Is.EqualTo(viewAfterGet));
        });
    }
}