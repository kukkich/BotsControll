namespace Itmp.Encoding;

internal static class Extensions
{
    public static void Set(this byte[] array, IEnumerable<byte> additional, int index)
    {
        foreach (byte b in additional)
        {
            array[index++] = b;
        }
    }
}