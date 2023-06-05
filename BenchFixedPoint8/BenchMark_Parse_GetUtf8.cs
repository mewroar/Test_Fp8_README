
using System;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Gitan.FixedPoint8;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<保留中>")]
public class BenchMark_Parse_GetUtf8
{
    static readonly FixedPoint8 fixedPoint8Value = FixedPoint8.FromInnerValue(-1_234_000_000);
    static readonly int intValue = -1234;
    static readonly double doubleValue = -12.34;
    static readonly decimal decimalValue = -12.34m;

    static readonly string str = "-12.34";
    static readonly string strInt = "-1234";

    static readonly byte[] byteArray = "-12.34"u8.ToArray();

    /////////////////////////////////////// ROS Parse

    [Benchmark]
    public int StringToInt()
    {
        var result = int.Parse(strInt);
        return result;
    }

    [Benchmark]
    public double StringToDouble()
    {
        var result = double.Parse(str);
        return result;
    }

    [Benchmark]
    public decimal StringToDecimal()
    {
        var result = decimal.Parse(str);
        return result;
    }

    [Benchmark]
    public FixedPoint8 StringToFixedPoint8()
    {
        var result = FixedPoint8.Parse(str);
        return result;
    }

    [Benchmark]
    public FixedPoint8 Utf8ToFixedPoint8()
    {
        var result = FixedPoint8.Parse(byteArray);
        return result;
    }


    /////////////////////////////////////// GetUtf8

    [Benchmark]
    public string IntToString()
    {
        var result = intValue.ToString();
        return result;
    }

    [Benchmark]
    public string DoubleToString()
    {
        var result = doubleValue.ToString();
        return result;
    }

    [Benchmark]
    public string DecimalToString()
    {
        var result = decimalValue.ToString();
        return result;
    }

    [Benchmark]
    public string FixedPoint8ToString()
    {
        var result = fixedPoint8Value.ToString();
        return result;
    }

    [Benchmark]
    public byte[] FixedPoint8ToUtf8()
    {
        var result = fixedPoint8Value.ToUtf8();
        return result;
    }


    //|              Method |      Mean |     Error |    StdDev |
    //|-------------------- |----------:|----------:|----------:|
    //|         StringToInt |  9.986 ns | 0.2077 ns | 0.1841 ns |
    //|      StringToDouble | 37.782 ns | 0.4586 ns | 0.3829 ns |
    //|     StringToDecimal | 35.427 ns | 0.4764 ns | 0.4224 ns |
    //| StringToFixedPoint8 | 45.343 ns | 0.9202 ns | 0.9037 ns |
    //|   Utf8ToFixedPoint8 |  6.730 ns | 0.1567 ns | 0.2393 ns |

    //|         IntToString | 15.734 ns | 0.3500 ns | 0.3274 ns |
    //|      DoubleToString | 80.292 ns | 1.2928 ns | 1.1460 ns |
    //|     DecimalToString | 42.489 ns | 0.7550 ns | 0.7062 ns |
    //| FixedPoint8ToString | 65.276 ns | 1.1657 ns | 1.0904 ns |
    //|   FixedPoint8ToUtf8 | 12.879 ns | 0.3065 ns | 0.5121 ns |

}

