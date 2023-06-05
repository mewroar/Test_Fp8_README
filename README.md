■ <sub>**Gitan.FixedPoint8とは**</sub>

Gitan.FixedPoint8は、固定小数点で-92233720368.54775808～92233720368.54775807までの数字を扱うことができます。
内部にInt64をもつstructで、10進数の小数点を誤差なく扱うことができます。
実行速度が速いことに重点を置いてUTF8との親和性が高いです。


■ **技術仕様**

・偶数、奇数判定は速度に重点を置くため％を使用せずに実装しています。

・速度最適化未実施（速度はでないので使用の際はご注意ください）
　　FixedPoint8同士の乗算、除算
　　stringへの変換

・Gitan.FixedPoint8はuncheckedで動きます、オーバーフローが発生する値でエラーは発生しませんのでご注意ください。


■ **使用方法**

NuGetパッケージ : Gitan.FixedPoint8
NuGetを使用してFixedPoint8パッケージをインストールします。

    using Gitan.FixedPoint8;

    public void fp8Test()       
    {
        var v1 = FixedPoint8.FromDouble(12.34);        
        var v2 = FixedPoint8.FromDecimal(12.34m);
        //内部的には、Int64の1234000000となる。

        var add = v1 + v2;
        var sub = v1 - v2;
        var mul = v1 * v2;
        var div = v1 / v2;
    }


NuGetパッケージ : Gitan.Utf8Json_FixedPoint8
NuGetを使用してUtf8Json_FixedPoint8をインストールします。

Gitan.Utf8Json_FixedPoint8はJsonとFixedPoint8をRead,Writeします。
ReadFixedPoint8,WriteFixedPoint8の処理はUtf8JsonのNumberConverterを部分引用しています。

    using Gitan.FixedPoint8;

    static readonly byte[] _json = """{"Value":-12.34}"""u8.ToArray();

    public FixedPoint8 DeserializeFixedPoint8()
    {
        var obj = JsonSerializer.Deserialize<FixedPoint8Class>(_json);
        return obj.Value;
    }

    public byte[] SerializeFixedPoint8()
    {
        var test = FixedPoint8Class.GetSample();
        var result = JsonSerializer.Serialize<FixedPoint8Class>(test);
        return result;
    }

    public class FixedPoint8Class
    {
        [JsonFormatter(typeof(FixedPoint8Formatter))]
        public FixedPoint8 Value { get; set; }
        public static FixedPoint8Class GetSample()
        {
            var item = new FixedPoint8Class()
            {
                Value = new FixedPoint8(-1_234_000_000),
            };
            return item;
        }
    }

    public sealed class FixedPoint8Formatter : IJsonFormatter<FixedPoint8>
    {
        public static readonly FixedPoint8Formatter Default = new();

        public void Serialize(ref JsonWriter writer, FixedPoint8 value, IJsonFormatterResolver formatterResolver)
        {
            writer.WriteFixedPoint8(value);
        }

        public FixedPoint8 Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            return reader.ReadFixedPoint8();
        }
    }


下記はUtf8Jsonを使用したケースです。
実行するには別途NuGetを使用してUtf8Jsonをインストールしてください。

    using Gitan.FixedPoint8;

    readonly byte[] sharedBuffer = new byte[65535];

    public void CheckFixedPoint8()
    {
        var testRos = "12345678.12345678E-12"u8;

        var jsonArray = testRos.ToArray();
        var reader = new Utf8Json.JsonReader(jsonArray);
        var result = reader.ReadFixedPoint8();

        var writer = new Utf8Json.JsonWriter(sharedBuffer);
        writer.WriteFixedPoint8(result);
    }


■ **パフォーマンス**

    static readonly FixedPoint8 fixedPoint8Value = FixedPoint8.FromInnerValue(-1_234_000_000);　//　12.34
    static readonly FixedPoint8 v2 = FixedPoint8.FromInnerValue(200_000_000);　//　2
    static readonly FixedPoint8 v10 = FixedPoint8.FromInnerValue(1_000_000_000);　//　1

    [Benchmark]
    public FixedPoint8 MulInt2FixedPoint8()
    {
        var result = FixedPoint8.Zero;
        for (var i = 0; i < 1000; i++)
        {
            result = fixedPoint8Value * 2;
        }
        return result;
    }

    [Benchmark]
    public FixedPoint8 Mul2FixedPoint8()
    {
        var result = FixedPoint8.Zero;
        for (var i = 0; i < 1000; i++)
        {
            result = fixedPoint8Value * v2;
        }
        return result;
    }

    [Benchmark]
    public FixedPoint8 MulInt10FixedPoint8()
    {
        var result = FixedPoint8.Zero;
        for (var i = 0; i < 1000; i++)
        {
            result = fixedPoint8Value * 10;
        }
        return result;
    }

    [Benchmark]
    public FixedPoint8 Mul10FixedPoint8()
    {
        var result = FixedPoint8.Zero;
        for (var i = 0; i < 1000; i++)
        {
            result = fixedPoint8Value * v10;
        }
        return result;
    }

    [Benchmark]
    public FixedPoint8 Add10FixedPoint8()
    {
        var result = FixedPoint8.Zero;
        for (var i = 0; i < 1000; i++)
        {
            result = fixedPoint8Value + v10;
        }
        return result;
    }

    [Benchmark]
    public FixedPoint8 Sub10FixedPoint8()
    {
        var result = FixedPoint8.Zero;
        for (var i = 0; i < 1000; i++)
        {
            result = fixedPoint8Value - v10;
        }
        return result;
    }


加算、減算はdecimalと比べて90%～95%速いがDoubleの約1/2の速度です。
FixedPoint8との乗算は遅いので使用を推奨しない


|                 Method |           Mean |       Error |      StdDev |         Median |
|----------------------- |---------------:|------------:|------------:|---------------:|
|                Mul2Int |    252.0915 ns |   5.0733 ns |  10.1320 ns |    249.4333 ns |
|             Mul2Double |    247.6842 ns |   4.8895 ns |   6.5273 ns |    250.0768 ns |
|            Mul2Decimal |  3,780.4062 ns |  25.2739 ns |  22.4047 ns |  3,776.3885 ns |
|     MulInt2FixedPoint8 |    361.1021 ns |  30.7209 ns |  90.5812 ns |    380.6810 ns |
|        Mul2FixedPoint8 | 48,425.0743 ns | 534.9942 ns | 500.4339 ns | 48,350.0397 ns |
|               Mul10Int |    338.9850 ns |  21.2681 ns |  62.7094 ns |    384.0795 ns |
|            Mul10Double |    248.7886 ns |   4.8682 ns |   5.6063 ns |    250.9856 ns |
|           Mul10Decimal |  3,817.4721 ns |  60.0774 ns |  56.1965 ns |  3,800.2945 ns |
|    MulInt10FixedPoint8 |    407.5580 ns |  19.4389 ns |  57.3160 ns |    417.3599 ns |
|       Mul10FixedPoint8 | 48,259.3571 ns | 348.5981 ns | 326.0789 ns | 48,316.4429 ns |
|                Add2Int |    239.0550 ns |   2.7776 ns |   2.5982 ns |    237.8625 ns |
|             Add2Double |    246.6463 ns |   4.8329 ns |   4.5207 ns |    248.1304 ns |
|            Add2Decimal |  8,185.7489 ns |  94.7229 ns |  83.9694 ns |  8,211.4624 ns |
|        Add2FixedPoint8 |    481.6143 ns |   6.7220 ns |   5.9589 ns |    480.0810 ns |
|               Add10Int |    240.1301 ns |   2.1247 ns |   1.8835 ns |    240.1348 ns |
|            Add10Double |    244.1696 ns |   4.6792 ns |   5.3886 ns |    242.8494 ns |
|           Add10Decimal |  8,056.7542 ns | 152.1530 ns | 175.2196 ns |  8,011.9179 ns |
|       Add10FixedPoint8 |    480.6928 ns |   9.4924 ns |   9.7480 ns |    476.9773 ns |
|                Sub2Int |    241.1786 ns |   3.8953 ns |   4.0002 ns |    240.3090 ns |
|             Sub2Double |    245.0493 ns |   4.3060 ns |   3.8172 ns |    244.8148 ns |
|            Sub2Decimal |  7,754.1480 ns | 110.1171 ns |  97.6159 ns |  7,789.7743 ns |
|        Sub2FixedPoint8 |    489.0548 ns |   9.4958 ns |  13.9189 ns |    483.1250 ns |
|               Sub10Int |    239.8962 ns |   2.6542 ns |   2.4827 ns |    239.3870 ns |
|            Sub10Double |    241.0682 ns |   3.4770 ns |   3.2524 ns |    241.1084 ns |
|           Sub10Decimal |  7,753.7237 ns |  83.3745 ns |  69.6215 ns |  7,780.5588 ns |
|       Sub10FixedPoint8 |    479.8721 ns |   4.5137 ns |   3.7691 ns |    478.9358 ns |
|            LessThanInt |      0.0903 ns |   0.0072 ns |   0.0063 ns |      0.0909 ns |
|         LessThanDouble |      0.0492 ns |   0.0244 ns |   0.0217 ns |      0.0456 ns |
|        LessThanDecimal |      1.2222 ns |   0.0267 ns |   0.0223 ns |      1.2134 ns |
|    LessThanFixedPoint8 |      0.1953 ns |   0.0167 ns |   0.0139 ns |      0.1911 ns |


■ **Utf8JsonFixedPoint8**


public class BenchMark_Serializer
{
    /////////////////////////////////////// Reader
    ///
    static readonly byte[] _sourceInt = "-1234,"u8.ToArray();
    static readonly byte[] _source = "-12.34,"u8.ToArray();

    [Benchmark]
    public int ReadInt()
    {
        var reader = new JsonReader(_sourceInt);
        return reader.ReadInt32();
    }

    [Benchmark]
    public long ReadLong()
    {
        var reader = new JsonReader(_sourceInt);
        return reader.ReadInt64();
    }

    [Benchmark]
    public double ReadDouble()
    {
        var reader = new JsonReader(_source);
        return reader.ReadDouble();
    }

    [Benchmark]
    public FixedPoint8 ReadFixedPoint8()
    {
        var reader = new JsonReader(_source);
        return reader.ReadFixedPoint8();
    }


    /////////////////////////////////////// Deserialize
    ///
    static readonly byte[] _jsonInt = """{"Value":-1234}"""u8.ToArray();
    static readonly byte[] _json = """{"Value":-12.34}"""u8.ToArray();

    [Benchmark]
    public int DeserializeInt()
    {
        var obj = JsonSerializer.Deserialize<IntClass>(_jsonInt);
        return obj.Value;
    }

    [Benchmark]
    public long DeserializeLong()
    {
        var obj = JsonSerializer.Deserialize<LongClass>(_jsonInt);
        return obj.Value;
    }

    [Benchmark]
    public double DeserializeDouble()
    {
        var obj = JsonSerializer.Deserialize<DoubleClass>(_json);
        return obj.Value;
    }

    [Benchmark]
    public decimal DeserializeDecimal()
    {
        var obj = JsonSerializer.Deserialize<DecimalClass>(_json);
        return obj.Value;
    }

    [Benchmark]
    public FixedPoint8 DeserializeFixedPoint8()
    {
        var obj = JsonSerializer.Deserialize<FixedPoint8Class>(_json);
        return obj.Value;
    }

    /////////////////////////////////////// Writer

    readonly byte[] sharedBuffer = new byte[65535];

    [Benchmark]
    public void WriteInt()
    {
        var writer = new JsonWriter(sharedBuffer);
        writer.WriteInt32(-1234);
    }

    [Benchmark]
    public void WriteLong()
    {
        var writer = new JsonWriter(sharedBuffer);
        writer.WriteInt64(-1234);
    }

    [Benchmark]
    public void WriteDouble()
    {
        var writer = new JsonWriter(sharedBuffer);
        writer.WriteDouble(-12.34);
    }

    [Benchmark]
    public void WriteFixedPoint8()
    {
        var writer = new JsonWriter(sharedBuffer);
        writer.WriteFixedPoint8(new FixedPoint8(-1_234_000_000));
    }


    /////////////////////////////////////// Serialize

    [Benchmark]
    public byte[] SerializeInt()
    {
        var test = IntClass.GetSample();
        var result = JsonSerializer.Serialize<IntClass>(test);
        return result;
    }

    [Benchmark]
    public byte[] SerializeLong()
    {
        var test = LongClass.GetSample();
        var result = JsonSerializer.Serialize<LongClass>(test);
        return result;
    }

    [Benchmark]
    public byte[] SerializeDouble()
    {
        var test = DoubleClass.GetSample();
        var result = JsonSerializer.Serialize<DoubleClass>(test);
        return result;
    }

    [Benchmark]
    public byte[] SerializeDecimal()
    {
        var test = DecimalClass.GetSample();
        var result = JsonSerializer.Serialize<DecimalClass>(test);
        return result;
    }

    [Benchmark]
    public byte[] SerializeFixedPoint8()
    {
        var test = FixedPoint8Class.GetSample();
        var result = JsonSerializer.Serialize<FixedPoint8Class>(test);
        return result;
    }
}


Reader,WriterはDoubleと比較して90%速い
Deserialize,SerializeはDouble,Decimalと比較して55%～60%速い
Int32,Int64と比べても差はほとんどない。


byte[]でReader,Writer,Deserialize,Serializeの比較

|                 Method |       Mean |     Error |    StdDev |
|----------------------- |-----------:|----------:|----------:|
|                ReadInt |   6.779 ns | 0.1109 ns | 0.1554 ns |
|               ReadLong |   5.235 ns | 0.0418 ns | 0.0391 ns |
|             ReadDouble |  75.224 ns | 0.7750 ns | 0.7249 ns |
|        ReadFixedPoint8 |   8.188 ns | 0.0412 ns | 0.0344 ns |
|         DeserializeInt |  44.483 ns | 0.2374 ns | 0.1853 ns |
|        DeserializeLong |  43.057 ns | 0.1556 ns | 0.1379 ns |
|      DeserializeDouble | 115.052 ns | 0.9007 ns | 0.7521 ns |
|     DeserializeDecimal | 117.528 ns | 0.9802 ns | 0.9168 ns |
| DeserializeFixedPoint8 |  50.152 ns | 0.3271 ns | 0.3060 ns |
|               WriteInt |   7.390 ns | 0.0747 ns | 0.0662 ns |
|              WriteLong |   7.096 ns | 0.0663 ns | 0.0620 ns |
|            WriteDouble |  78.730 ns | 0.3509 ns | 0.2740 ns |
|       WriteFixedPoint8 |   7.428 ns | 0.0822 ns | 0.0686 ns |
|           SerializeInt |  41.309 ns | 0.4833 ns | 0.4284 ns |
|          SerializeLong |  38.435 ns | 0.5191 ns | 0.4856 ns |
|        SerializeDouble | 118.182 ns | 0.5690 ns | 0.5322 ns |
|       SerializeDecimal | 108.904 ns | 1.2402 ns | 1.0356 ns |
|   SerializeFixedPoint8 |  45.996 ns | 0.4746 ns | 0.4439 ns |

■ **Api定義**

|                 プロパティ|                                 説明|
| ---------------------- | ----------------------------------- |
|MaxValue                |longの可能な最大値をFixedPoint8で返します                         |
|MinValue                |longの可能な最小値をFixedPoint8で返します                         |
|Zero                    |FixedPoint8の0を返します                               |
|One                     |FixedPoint8の1を返します                               |


|                                                    メソッド|                                                    説明|
| ------------------------------------------------------ | ------------------------------------------------------ |
|FromInnerValue(long)                                    |longをFixedPoint8の値で返します                                                    |
|FromDouble(double)                                      |doubleをFixedPoint8の値で返します                                                            |
|FromDecimal(decimal)                                    |decimalのFixedPoint8の値で返します                                                              |
|Parse(string)                                           |文字列をFixedPoint8に変換しようとします。                                                               |
|Parse(ReadOnlySpan<char>)                               |読み取り専用のcharを、FixedPoint8に変換しようとします。                                                               |
|Parse(ReadOnlySpan<byte>)                               |読み取り専用のbyteを、FixedPoint8に変換しようとします。                                                            |
|TryParse([NotNullWhen(true)] string?, out FixedPoint8)  |文字列をFixedPoint8に変換しようとします。戻り値は、変換が成功したか失敗したかを示します。                                                            |
|TryParse(ReadOnlySpan<char>, out FixedPoint8)           |読み取り専用のcharを、FixedPoint8に変換しようとします。戻り値は、変換が成功したか失敗したかを示します。                                                         |
|TryParse(ReadOnlySpan<byte>, out FixedPoint8)           |読み取り専用のbyteを、FixedPoint8に変換しようとします。 戻り値は、変換が成功したか失敗したかを示します。                                                       |
|ToString()                                              |このインスタンスの数値を同等の文字列表現に変換します。                                                               |
|ToUtf8()                                                |このインスタンスの数値をUTF8に変換します。                                                               |
|Equals(object?)                                         |このインスタンスが指定されたオブジェクトと等しいかどうかを示す値を返します。                                                              |
|Equals(FixedPoint8)                                     |このインスタンスが指定されたFixedPoint8と等しいかどうかを示す値を返します。                                                               |
|GetHashCode()                                           |このインスタンスのハッシュ コードを返します                                                              |
|CompareTo(object?)                                      |このインスタンスを指定されたオブジェクトと比較し、それらの相対値の指示を返します。                                                               |
|CompareTo(FixedPoint8)                                  |このインスタンスを指定されたFixedPoint8と比較し、それらの相対値の指示を返します。                                                               |
|Abs(FixedPoint8)                                        |FixedPoint8の絶対値を返します。                                                            |
|IsCanonical(FixedPoint8)                                |trueを返します                                                               |
|IsComplexNumber(FixedPoint8)                            |falseを返します                                                               |
|IsEvenInteger(FixedPoint8)                              |値が偶数の整数を表すかどうかを判断します。                                                               |
|IsFinite(FixedPoint8)                                   |trueを返します                                                                |
|IsImaginaryNumber(FixedPoint8)                          |falseを返します                                                              |
|IsInfinity(FixedPoint8)                                 |falseを返します                                                             |
|IsInteger(FixedPoint8)                                  |longの可能な最小値を返します                                                               |
|IsNaN(FixedPoint8)                                      |falseを返します                                                                |
|IsNegative(FixedPoint8)                                 |値が負かどうかを判断します。                                                              |
|IsNegativeInfinity(FixedPoint8)                         |falseを返します                                                               |
|IsNormal(FixedPoint8)                                   |trueを返します                                                              |
|IsOddInteger(FixedPoint8)                               |値が奇数の整数を表すかどうかを判断します。                                                               |
|IsPositive(FixedPoint8)                                 |値が正かどうかを判断します                                                        |
|IsPositiveInfinity(FixedPoint8)                         |falseを返します                                                                  |
|IsRealNumber(FixedPoint8)                               |trueを返します                                                               |
|IsSubnormal(FixedPoint8)                                |falseを返します                                                                 |
|IsZero(FixedPoint8)                                     |値が0かどうかを判断します                                                              |
|MaxMagnitude(FixedPoint8, FixedPoint8)                  |値を比較して大きい方の値を返します                                                               |
|MaxMagnitudeNumber(FixedPoint8, FixedPoint8)            |値を比較して大きい方の値を返します                                                                 |
|MinMagnitude(FixedPoint8, FixedPoint8)                  |値を比較して小さい方の値を返します                                                                |
|MinMagnitudeNumber(FixedPoint8, FixedPoint8)            |値を比較して小さい方の値を返します                                                             |
|Parse(ReadOnlySpan<char>, NumberStyles, IFormatProvider?)   |開発されていないのでスローされます                                                        |
|Parse(string, NumberStyles, IFormatProvider?)               |開発されていないのでスローされます                                                               |
|TryParse(ReadOnlySpan<char>, NumberStyles, IFormatProvider?, [MaybeNullWhen(false)] out FixedPoint8)   |開発されていないのでスローされます                                                             |
|TryParse([NotNullWhen(true)] string?, NumberStyles, IFormatProvider?, [MaybeNullWhen(false)] out FixedPoint8)    |開発されていないのでスローされます                                                               |
|TryFormat(Span<char> , out int, ReadOnlySpan<char>, IFormatProvider?)   |開発されていないのでスローされます                                                               |
|ToString(string?, IFormatProvider?)                     |開発されていないのでスローされます                                                               |
|Parse(ReadOnlySpan<char>, IFormatProvider?)             |開発されていないのでスローされます                                                            |
|TryParse(ReadOnlySpan<char>, IFormatProvider?, [MaybeNullWhen(false)] out FixedPoint8)   |開発されていないのでスローされます                                                               |
|Parse(string, IFormatProvider?)                         |開発されていないのでスローされます                                                               |
|TryParse([NotNullWhen(true)] string?, IFormatProvider?, [MaybeNullWhen(false)] out FixedPoint8)   |開発されていないのでスローされます                                                               |
|Round()                                                 |最も近い整数に値を丸めます                                                              |
|Round(int)                                              |指定した小数点以下の桁数に値を丸めます。                                                            |
|Floor()                                                 |指定した倍精度浮動小数点数以下の数のうち、最大の整数値を返します。                                                               |
|Floor(int)                                              |指定した小数点以下の桁数に最大の値を返します。                                                    |
|Truncate()                                              |指定した10進数の整数部を計算します。                                                              |
|Truncate(int)                                           |指定した小数点以下の桁数に値を計算します。                                                              |
|Ceiling()                                               |指定した 10進数以上の数のうち、最小の整数値を返します。                                                              |
|Ceiling(int)                                            |指定した倍精度浮動小数点数以上の数のうち、最小の値を返します。                                                               |
