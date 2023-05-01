using Itmp.Encoding;

namespace Itmp.Tests.Cbor;

public class Uint32Tests
{
    #region Cases
    [TestCase(0, 13)]
    [TestCase(1, 13)]
    [TestCase(2, 138)]
    [TestCase(3, 37)]
    [TestCase(4, 0)]
    [TestCase(5, 2)]
    [TestCase(6, 42)]
    [TestCase(7, 166)]
    [TestCase(8, 4)]
    [TestCase(9, 224)]
    [TestCase(10, 114)]
    [TestCase(11, 69)]
    [TestCase(12, 0)]
    [TestCase(13, 0)]
    #endregion
    public void GetUint8WithMultipleSet(int index, byte expected)
    {
        BytePool pool = GetBytePool();

        var oldView = pool.AsString;
        var actual = pool.GetUint8(index);
        var viewAfterGet = pool.AsString;

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(oldView, Is.EqualTo(viewAfterGet));
        });
    }

    #region Cases
    [TestCase(0, (UInt16)3341)]
    [TestCase(1, (UInt16)3466)]
    [TestCase(2, (UInt16)35365)]
    [TestCase(3, (UInt16)9472)]
    [TestCase(4, (UInt16)2)]
    [TestCase(5, (UInt16)554)]
    [TestCase(6, (UInt16)10918)]
    [TestCase(7, (UInt16)42500)]
    [TestCase(8, (UInt16)1248)]
    [TestCase(9, (UInt16)57458)]
    [TestCase(10, (UInt16)29253)]
    [TestCase(11, (UInt16)17664)]
    [TestCase(12, (UInt16)0)]
    #endregion
    public void GetUint16WithMultipleSet(int index, UInt16 expected)
    {
        BytePool pool = GetBytePool();

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
    [TestCase(0, (UInt32)218991141)]
    [TestCase(1, (UInt32)227157248)]
    [TestCase(2, (UInt32)2317680642)]
    [TestCase(3, (UInt32)620757546)]
    [TestCase(4, (UInt32)141990)]
    [TestCase(5, (UInt32)36349444)]
    [TestCase(6, (UInt32)715523296)]
    [TestCase(7, (UInt32)2785337458)]
    [TestCase(8, (UInt32)81818181)]
    [TestCase(9, (UInt32)3765585152)]
    [TestCase(10, (UInt32)1917124608)]
    #endregion
    public void GetUint32WithMultipleSet(int index, UInt32 expected)
    {
        BytePool pool = GetBytePool();

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
    [TestCase(0, (float)4.361526497624336e-31)]
    [TestCase(1, (float)8.513814060406824e-31)]
    [TestCase(2, (float)-7.944462489965286e-33)]
    [TestCase(3, (float)1.1102963459146532e-16)]
    [TestCase(4, (float)1.9897036894948078e-40)]
    [TestCase(5, (float)1.2537271625273849e-37)]
    [TestCase(6, (float)2.949090624482231e-13)]
    [TestCase(7, (float)-4.610087988505857e-16)]
    [TestCase(8, (float)5.276708690781933e-36)]
    [TestCase(9, -69829438122286380000f)]
    [TestCase(10, (float)3.9019870038275186e+30)]
    #endregion
    public void GetFloat32WithMultipleSet(int index, float expected)
    {
        BytePool pool = GetBytePool();
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
    [TestCase(0, 8.449646875208292e-246)]
    [TestCase(1, 1.9144956254926443e-243)]
    [TestCase(2, -8.536379792928165e-260)]
    [TestCase(3, 1.804270007642764e-130)]
    [TestCase(4, 3.013022227827115e-309)]
    [TestCase(5, 3.1833710919476678e-298)]
    [TestCase(6, 3.072204134065562e-103)]
    #endregion
    public void GetFloat64WithMultipleSet(int index, double expected)
    {
        BytePool pool = GetBytePool();

        var oldView = pool.AsString;
        var actual = pool.GetFloat64(index);
        var viewAfterGet = pool.AsString;

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(oldView, Is.EqualTo(viewAfterGet));
        });
    }

    public BytePool GetBytePool()
    {
        BytePool pool = new(14);
        pool.SetUint32(1, 412941179);
        pool.SetUint32(2, 418441141);
        pool.SetUint32(0, 218991141);
        pool.SetUint32(4, 141990);
        pool.SetUint32(8, 81818181);
        return pool;
    }
}