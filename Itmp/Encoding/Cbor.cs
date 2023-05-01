using System.ComponentModel;
using System.Numerics;
using Itmp.MessageExamples;

namespace Itmp.Encoding;

public class Cbor
{
    private static readonly ulong Pow2In32 = (ulong)BigInteger.Pow(2, 32);
    private static readonly long Pow2In53 = (long)BigInteger.Pow(2, 53);

    //Todo вынести с отдельный класс и сделать BytePool readonly ref struct
    public static byte[] Encode(object? value)
    {
        var data = new byte[256];
        var dataView = new BytePool(data);
        int lastLength;
        var offset = 0;

        #region EncodeHelpers
        BytePool PrepareWrite(int length)
        {
            var newLength = data!.Length;
            var requiredLength = offset + length;
            while (newLength < requiredLength)
                newLength <<= 1;
            if (newLength != data.Length)
            {
                var oldBytePool = dataView;
                data = new byte[newLength];
                dataView = new BytePool(data);
                var uint32Count = (offset + 3) >> 2;
                for (var i = 0; i < uint32Count; ++i)
                    dataView.SetUint32(i << 2, oldBytePool.GetUint32(i << 2));
            }

            lastLength = length;
            return dataView;
        }
        void CommitWrite()
        {
            offset += lastLength;
        }
        void WriteFloat64(double value)
        {
            PrepareWrite(8).SetFloat64(offset, value);
            CommitWrite();
        }
        void WriteUint8(byte value)
        {
            PrepareWrite(1).SetUint8(offset, value);
            CommitWrite();
        }
        void WriteUint8Array(byte[] value)
        {
            var dataView = PrepareWrite(value.Length);
            for (var i = 0; i < value.Length; ++i)
                dataView.SetUint8(offset + i, value[i]);
            CommitWrite();
        }
        void WriteUint16(UInt16 value)
        {
            PrepareWrite(2).SetUint16(offset, value);
            CommitWrite();
        }
        void WriteUint32(UInt32 value)
        {
            PrepareWrite(4).SetUint32(offset, value);
            CommitWrite();
        }
        void WriteUint64(UInt64 value)
        {
            uint low, high;

            checked
            {
                low = (uint)(value % Pow2In32);
                high = (uint)((value - low) / Pow2In32);
            }
            var dataView = PrepareWrite(8);
            dataView.SetUint32(offset, high);
            dataView.SetUint32(offset + 4, low);
            CommitWrite();
        }
        void WriteTypeAndLength(int type, long length)
        {
            switch (length)
            {
                case < 24:
                    {
                        int reducedLength = checked((int)length);
                        int shiftedType = type << 5;
                        int result = shiftedType | reducedLength;
                        byte resultAsUint8 = checked((byte)result);
                        WriteUint8(resultAsUint8);
                        return;
                    }
                case < 0x100:
                    {
                        int shiftedType = type << 5;
                        int result = shiftedType | 24;
                        byte resultAsUint8 = checked((byte)result);
                        WriteUint8(resultAsUint8);

                        byte lengthAsUint8 = checked((byte)length);
                        WriteUint8(lengthAsUint8);
                        return;
                    }
                case < 0x10000:
                    {
                        int shiftedType = type << 5;
                        int result = shiftedType | 25;
                        byte resultAsUint8 = checked((byte)result);
                        WriteUint8(resultAsUint8);

                        ushort lengthAsUint16 = checked((ushort)length);
                        WriteUint16(lengthAsUint16);
                        return;
                    }
                case < 0x100000000:
                    {
                        int shiftedType = type << 5;
                        int result = shiftedType | 26;
                        byte resultAsUint8 = checked((byte)result);
                        WriteUint8(resultAsUint8);

                        uint lengthAsUint32 = checked((uint)length);
                        WriteUint32(lengthAsUint32);
                        break;
                    }
                default:
                    {
                        int shiftedType = type << 5;
                        int result = shiftedType | 27;
                        byte resultAsUint8 = checked((byte)result);
                        WriteUint8(resultAsUint8);
                        ulong lengthAsUint32 = checked((ulong)length);
                        WriteUint64(lengthAsUint32);
                        break;
                    }
            }
        }

        void EncodeItem(object? value)
        {
            int i;

            if (value is false)
            {
                WriteUint8(0xf4);
                return;
            }
            if (value is true)
            {
                WriteUint8(0xf5);
                return;
            }
            if (value is null)
            {
                WriteUint8(0xf6);
                return;
            }

            switch (value)
            {
                case ItmpConstant itmpConstant:
                    long valueAsLong = (long)itmpConstant;
                    if (0 <= valueAsLong && valueAsLong <= Pow2In53)
                    {
                        WriteTypeAndLength(0, valueAsLong);
                        return;
                    }
                    if (-1 * Pow2In53 <= valueAsLong && valueAsLong < 0)
                    {
                        WriteTypeAndLength(1, -1 * (valueAsLong + 1));
                        return;
                    }
                    throw new ArgumentException();
                case long number:
                    if (0 <= number && number <= Pow2In53)
                    {
                        WriteTypeAndLength(0, number);
                        return;
                    }
                    if (-1 * Pow2In53 <= number && number < 0)
                    {
                        WriteTypeAndLength(1, -1 * (number + 1));
                        return;
                    }
                    throw new ArgumentException();
                case double floatPointValue:
                    WriteUint8(0xfb);
                    WriteFloat64(floatPointValue);
                    break;
                case string str:
                    var utf8data = new List<int>();
                    for (i = 0; i < str.Length; ++i)
                    {
                        var charCode = (int)str[i];
                        if (charCode < 0x80)
                        {
                            utf8data.Add(charCode);
                        }
                        else if (charCode < 0x800)
                        {
                            utf8data.Add(0xc0 | charCode >> 6);
                            utf8data.Add(0x80 | charCode & 0x3f);
                        }
                        else if (charCode < 0xd800)
                        {
                            utf8data.Add(0xe0 | charCode >> 12);
                            utf8data.Add(0x80 | (charCode >> 6) & 0x3f);
                            utf8data.Add(0x80 | charCode & 0x3f);
                        }
                        else
                        {
                            charCode = (charCode & 0x3ff) << 10;
                            charCode |= str[++i] & 0x3ff;
                            charCode += 0x10000;

                            utf8data.Add(0xf0 | charCode >> 18);
                            utf8data.Add(0x80 | (charCode >> 12) & 0x3f);
                            utf8data.Add(0x80 | (charCode >> 6) & 0x3f);
                            utf8data.Add(0x80 | charCode & 0x3f);
                        }
                    }

                    WriteTypeAndLength(3, utf8data.Count);
                    WriteUint8Array(utf8data.Select(x => checked((byte)x)).ToArray());
                    return;
                default:
                    if (value is byte[] bytes)
                    {
                        WriteTypeAndLength(2, bytes.Length);
                        WriteUint8Array(bytes);
                    }
                    else if (value is object[] array)
                    {
                        var length = array.Length;
                        WriteTypeAndLength(4, length);
                        for (i = 0; i < length; ++i)
                            EncodeItem(array[i]);
                    }
                    else
                    {
                        dynamic anyValue = value;
                        PropertyDescriptorCollection keys = TypeDescriptor.GetProperties(anyValue);

                        WriteTypeAndLength(5, keys.Count);
                        foreach (PropertyDescriptor pd in keys)
                        {
                            var key = pd.Name;
                            object valueOfKey = pd.GetValue(anyValue);
                            EncodeItem(key);
                            EncodeItem(valueOfKey);
                        }
                    }
                    break;

            }
        }
        #endregion

        EncodeItem(value);
        var str = System.Text.Encoding.Default.GetString(data);
        if (str.Contains("slice"))
            return data[0..offset];

        var ret = new byte[offset];
        var retView = new BytePool(ret);
        for (var i = 0; i < offset; ++i)
            retView.SetUint8(i, dataView.GetUint8(i));
        return ret;
    }

    public static object? Decode(byte[] bytes)
    {
        return new Decoder().Decode(bytes);
    }
}
