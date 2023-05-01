using System.Dynamic;
using System.Numerics;


namespace Itmp.Encoding;

public ref struct Decoder
{
    private static readonly double Pow2InMinus24 = Math.Pow(2, -24);
    private static readonly ulong Pow2In32 = (ulong)BigInteger.Pow(2, 32);

    private BytePool _dataView;
    private byte[] _data;
    private int _offset;
    private Func<object?, int, object?> _tagger;
    private Func<object?, object?> _simpleValue;

    public object? Decode(byte[] data, Func<object?, int, object?>? tagger = null, Func<object?, object?>? simpleValue = null)
    {
        _dataView = new BytePool(data);
        _offset = 0;
        _data = data;

        _tagger = tagger ?? ((x, _) => x);
        _simpleValue = simpleValue ?? (_ => null);

        var ret = DecodeItem();
        if (_offset != data.Length)
            throw new InvalidOperationException("Remaining bytes");

        return ret;
    }

    private void CommitRead(int length)
    {
        _offset += length;
    }

    private byte[] ReadArrayBuffer(int length)
    {
        CommitRead(length);
        return new Span<byte>(_data, _offset, length).ToArray();
    }

    private double ReadFloat16()
    {
        var tempArrayBuffer = new byte[4];
        var tempDataView = new BytePool(tempArrayBuffer);
        ushort value = ReadUint16();

        uint sign = checked((uint)(value & 0x8000));

        uint exponent = checked((uint)(value & 0x7c00));
        uint fraction = checked((uint)(value & 0x03ff));

        if (exponent == 0x7c00)
            exponent = 0xff << 10;
        else if (exponent != 0)
            exponent += (127 - 15) << 10;
        else if (fraction != 0)
            return (sign == 1 ? -1 : 1) * fraction * Pow2InMinus24;

        tempDataView.SetUint32(0, sign << 16 | exponent << 13 | fraction << 13);
        return tempDataView.GetFloat32(0);
    }

    private float ReadFloat32()
    {
        var retValue = _dataView.GetFloat32(_offset);
        CommitRead(4);
        return retValue;
    }

    private double ReadFloat64()
    {
        var retValue = _dataView.GetFloat64(_offset);
        CommitRead(8);
        return retValue;
    }

    private byte ReadUint8()
    {
        var retValue = _dataView.GetUint8(_offset);
        CommitRead(sizeof(byte));
        return retValue;
    }

    private UInt16 ReadUint16()
    {
        var retValue = _dataView.GetUint16(_offset);
        CommitRead(sizeof(UInt16));
        return retValue;
    }
    private UInt32 ReadUint32()
    {
        var retValue = _dataView.GetUint32(_offset);
        CommitRead(sizeof(UInt32));
        return retValue;
    }
    private UInt64 ReadUint64()
    {
        return ReadUint32() * Pow2In32 + ReadUint32();
    }
    private bool ReadBreak()
    {
        if (_dataView.GetUint8(_offset) != 0xff)
            return false;
        _offset += 1;
        return true;
    }
    private long ReadLength(int additionalInformation)
    {
        if (additionalInformation < 24)
            return additionalInformation;
        if (additionalInformation == 24)
            return ReadUint8();
        if (additionalInformation == 25)
            return ReadUint16();
        if (additionalInformation == 26)
            return ReadUint32();
        if (additionalInformation == 27)
            return checked((long)ReadUint64());
        if (additionalInformation == 31)
            return -1;
        throw new ArgumentOutOfRangeException(nameof(additionalInformation), "Invalid length encoding");
    }

    private Int128 ReadIndefiniteStringLength(int majorType)
    {
        var initialByte = ReadUint8();
        if (initialByte == 0xff)
            return -1;
        var length = ReadLength(initialByte & 0x1f);
        if (length < 0 || (initialByte >> 5) != majorType)
            throw new ArgumentOutOfRangeException(nameof(majorType), "Invalid indefinite length element");
        return length;
    }

    private void AppendUtf16Data(List<UInt16> utf16data, int length)
    {
        for (var i = 0; i < length; ++i)
        {
            int value = ReadUint8();
            if ((value & 0x80) != 0)
            {
                if (value < 0xe0)
                {
                    value = (value & 0x1f) << 6
                            | (ReadUint8() & 0x3f);
                    length -= 1;
                }
                else if (value < 0xf0)
                {
                    value = (value & 0x0f) << 12
                            | (ReadUint8() & 0x3f) << 6
                            | (ReadUint8() & 0x3f);
                    length -= 2;
                }
                else
                {
                    value = (value & 0x0f) << 18
                            | (ReadUint8() & 0x3f) << 12
                            | (ReadUint8() & 0x3f) << 6
                            | (ReadUint8() & 0x3f);
                    length -= 3;
                }
            }

            if (value < 0x10000)
            {
                utf16data.Add(checked((ushort)value));
            }
            else
            {
                value -= 0x10000;
                utf16data.Add(checked((ushort)(0xd800 | (value >> 10))));
                utf16data.Add(checked((ushort)(0xdc00 | (value & 0x3ff))));
            }
        }
    }

    private object? DecodeItem()
    {
        var initialByte = ReadUint8();
        var majorType = initialByte >> 5;
        var additionalInformation = initialByte & 0x1f;

        if (majorType == 7)
        {
            switch (additionalInformation)
            {
                case 25:
                    return ReadFloat16();
                case 26:
                    return ReadFloat32();
                case 27:
                    return ReadFloat64();
            }
        }

        var length = ReadLength(additionalInformation);
        if (length < 0 && (majorType < 2 || 6 < majorType))
            throw new InvalidOperationException("Invalid length");

        var lengthAsInt = checked((int)length);
        switch (majorType)
        {
            case 0:
                return length;
            case 1:
                return -1 - length;
            case 2:
                if (length < 0)
                {
                    var elements = new List<byte[]>();
                    var fullArrayLength = 0;
                    while ((ReadIndefiniteStringLength(majorType)) >= 0)
                    {
                        fullArrayLength += lengthAsInt;
                        elements.Add(ReadArrayBuffer(lengthAsInt));
                    }
                    var fullArray = new byte[fullArrayLength];
                    var fullArrayOffset = 0;
                    for (var i = 0; i < elements.Count; ++i)
                    {
                        fullArray.Set(elements[i], fullArrayOffset);
                        fullArrayOffset += elements[i].Length;
                    }
                    return fullArray;
                }
                return ReadArrayBuffer(lengthAsInt);
            case 3:
                var utf16data = new List<ushort>();
                if (length < 0)
                {
                    while ((ReadIndefiniteStringLength(majorType)) >= 0)
                        AppendUtf16Data(utf16data, lengthAsInt);
                }
                else
                    AppendUtf16Data(utf16data, lengthAsInt);

                var utf16Array = utf16data.ToArray();
                var bytes = new byte[utf16Array.Length * 2];
                Buffer.BlockCopy(utf16Array.ToArray(), 0, bytes, 0, utf16Array.Length * 2);

                return System.Text.Encoding.Unicode.GetString(bytes);
            case 4:
                if (length < 0)
                {
                    var retArray = new List<object>();
                    while (!ReadBreak())
                        retArray.Add(DecodeItem()!);
                    return retArray;
                }
                else
                {
                    var retArray = new object[lengthAsInt];
                    for (var i = 0; i < length; ++i)
                        retArray[i] = DecodeItem()!;
                    return retArray;
                }
            case 5:
                var retObject = new ExpandoObject() as IDictionary<string, Object>;
                for (var i = 0; i < length || length < 0 && !ReadBreak(); ++i)
                {
                    var key = (string)DecodeItem()!;
                    retObject.Add(key, DecodeItem());
                }
                return retObject;
            case 6:
                return _tagger(DecodeItem(), lengthAsInt);
            case 7:
                return lengthAsInt switch
                {
                    20 => false,
                    21 => true,
                    22 => null,
                    23 => null, //undefined
                    _ => _simpleValue(length)
                };
        }

        throw new InvalidOperationException();
    }
}