using Itmp.Convert;
using Itmp.Encoding;
using System.Dynamic;
using static System.Console;

var message = new object[] { 0L, 137L, "Bob", new Ttk { token = "qwertyuiop[]asdfghjkl;'zxcvbnm,./", Koken = "`1234567890-=\\~!@#$%^&*()_" } };

var encoded = Cbor.Encode(message);

WriteLine(new BytePool(encoded).AsString);

var expected =
    "10000100 00000011 00011000 10001001 01100011 01000010 01101111 01100010 10100010 01100101 01110100 01101111 01101011 01100101 01101110 01111000 00100001 01110001 01110111 01100101 01110010 01110100 01111001 01110101 01101001 01101111 01110000 01011011 01011101 01100001 01110011 01100100 01100110 01100111 01101000 01101010 01101011 01101100 00111011 00100111 01111010 01111000 01100011 01110110 01100010 01101110 01101101 00101100 00101110 00101111 01100101 01001011 01101111 01101011 01100101 01101110 01111000 00011010 01100000 00110001 00110010 00110011 00110100 00110101 00110110 00110111 00111000 00111001 00110000 00101101 00111101 01011100 01111110 00100001 01000000 00100011 00100100 00100101 01011110 00100110 00101010 00101000 00101001 01011111";
var resulted = new BytePool(encoded).AsString;
WriteLine(expected.Equals(resulted));
WriteLine();

ItmpConverter converter = new();
var deserializedMessage = converter.Deserialize(encoded);




Decoder decoder = new Decoder();
var decodedObject = decoder.Decode(encoded);

foreach (var x in decodedObject as object[])
{
    if (x is ExpandoObject obj)
    {
        dynamic any = obj;
        Write("connectMessageArgument : ");
        WriteLine(any.token);
        Write("Koken : ");
        WriteLine(any.Koken);
        continue;
    }
    WriteLine(x);
}

WriteLine(decodedObject);

class Ttk
{
    public string token { get; set; }
    public string Koken { get; set; }
}