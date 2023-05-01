namespace Itmp.Encoding;

public readonly struct BytePool
{
    private readonly byte[] _bytes;
    public ReadOnlySpan<byte> Bytes => _bytes;

    public BytePool(byte[] bytes)
    {
        _bytes = bytes;
    }
    public BytePool(int size)
        : this(new byte[size])
    { }

    public void SetUint8(int offset, byte value)
    {
        Span<byte> x = new Span<byte>(_bytes, offset, sizeof(byte));
        x[0] = value;
        x.Reverse();
    }
    public void SetUint16(int offset, UInt16 value)
    {
        Span<byte> x = new Span<byte>(_bytes, offset, sizeof(UInt16));
        BitConverter.TryWriteBytes(x, value);
        x.Reverse();
    }
    public void SetUint32(int offset, UInt32 value)
    {
        Span<byte> x = new Span<byte>(_bytes, offset, sizeof(UInt32));
        BitConverter.TryWriteBytes(x, value);
        x.Reverse();
    }
    public void SetFloat64(int offset, double value)
    {
        Span<byte> x = new Span<byte>(_bytes, offset, sizeof(double));
        BitConverter.TryWriteBytes(x, value);
        x.Reverse();
    }

    public byte GetUint8(int index) => _bytes[index];
    public UInt16 GetUint16(int index)
    {
        const int byteLength = sizeof(UInt16);

        var span = _bytes.AsSpan(index, byteLength);
        Span<byte> tmpSpan = stackalloc byte[byteLength];

        span.CopyTo(tmpSpan);

        tmpSpan.Reverse();
        return BitConverter.ToUInt16(tmpSpan);
    }
    public UInt32 GetUint32(int index)
    {
        const int byteLength = sizeof(UInt32);

        var span = _bytes.AsSpan(index, byteLength);
        Span<byte> tmpSpan = stackalloc byte[byteLength];

        span.CopyTo(tmpSpan);

        tmpSpan.Reverse();
        return BitConverter.ToUInt32(tmpSpan);
    }
    public float GetFloat32(int index)
    {
        const int byteLength = sizeof(Single);

        var span = _bytes.AsSpan(index, byteLength);
        Span<byte> tmpSpan = stackalloc byte[byteLength];

        span.CopyTo(tmpSpan);

        tmpSpan.Reverse();
        return BitConverter.ToSingle(tmpSpan);
    }
    public double GetFloat64(int index)
    {
        const int byteLength = sizeof(double);

        var span = _bytes.AsSpan(index, byteLength);
        Span<byte> tmpSpan = stackalloc byte[byteLength];

        span.CopyTo(tmpSpan);

        tmpSpan.Reverse();
        return BitConverter.ToDouble(tmpSpan);
    }

    public string AsString => string.Join(" ", _bytes.ToArray().Select(x => System.Convert.ToString(x, 2).PadLeft(8, '0')));
}