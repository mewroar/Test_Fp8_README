
using System;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Gitan.FixedPoint8;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<保留中>")]
public class BenchMark_Calc
{

    /////////////////////////////////////// ROS Parse

    static readonly FixedPoint8 fixedPoint8Value = FixedPoint8.FromInnerValue(-1_234_000_000);
    static readonly FixedPoint8 v2 = FixedPoint8.FromInnerValue(200_000_000);
    static readonly FixedPoint8 v10 = FixedPoint8.FromInnerValue(1_000_000_000);
    static readonly int intValue = -1234;
    static readonly double doubleValue = -12.34;
    static readonly decimal decimalValue = -12.34m;


    /// ////////////////////////////////////// Kakeru 

    [Benchmark]
    public int Mul2Int()
    {
        var result = 0;
        for (var i = 0; i < 1000; i++)
        {
            result = intValue * 2;
        }
        return result;
    }

    [Benchmark]
    public double Mul2Double()
    {
        var result = 0.0;
        for (var i = 0; i < 1000; i++)
        {
            result = doubleValue * 2;
        }
        return result;
    }

    [Benchmark]
    public decimal Mul2Decimal()
    {
        var result = 0.0m;
        for (var i = 0; i < 1000; i++)
        {
            result = decimalValue * 2m;
        }
        return result;
    }

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
    public int Mul10Int()
    {
        var result = 0;
        for (var i = 0; i < 1000; i++)
        {
            result = intValue * 10;
        }
        return result;
    }

    [Benchmark]
    public double Mul10Double()
    {
        var result = 0.0;
        for (var i = 0; i < 1000; i++)
        {
            result = doubleValue * 10;
        }
        return result;
    }

    [Benchmark]
    public decimal Mul10Decimal()
    {
        var result = 0.0m;
        for (var i = 0; i < 1000; i++)
        {
            result = decimalValue * 10m;
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


    /// ////////////////////////////////////// Plus

    [Benchmark]
    public int Add2Int()
    {
        var result = 0;
        for (var i = 0; i < 1000; i++)
        {
            result = intValue + 2;
        }
        return result;
    }

    [Benchmark]
    public double Add2Double()
    {
        var result = 0.0;
        for (var i = 0; i < 1000; i++)
        {
            result = doubleValue + 2;
        }
        return result;
    }

    [Benchmark]
    public decimal Add2Decimal()
    {
        var result = 0.0m;
        for (var i = 0; i < 1000; i++)
        {
            result = decimalValue + 2m;
        }
        return result;
    }

    [Benchmark]
    public FixedPoint8 Add2FixedPoint8()
    {
        var result = FixedPoint8.Zero;
        for (var i = 0; i < 1000; i++)
        {
            result = fixedPoint8Value + v2;
        }
        return result;
    }

    [Benchmark]
    public int Add10Int()
    {
        var result = 0;
        for (var i = 0; i < 1000; i++)
        {
            result = intValue + 10;
        }
        return result;
    }

    [Benchmark]
    public double Add10Double()
    {
        var result = 0.0;
        for (var i = 0; i < 1000; i++)
        {
            result = doubleValue + 10;
        }
        return result;
    }

    [Benchmark]
    public decimal Add10Decimal()
    {
        var result = 0.0m;
        for (var i = 0; i < 1000; i++)
        {
            result = decimalValue + 10m;
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


    ///// ////////////////////////////////////// Minus

    [Benchmark]
    public int Sub2Int()
    {
        var result = 0;
        for (var i = 0; i < 1000; i++)
        {
            result = intValue - 2;
        }
        return result;
    }

    [Benchmark]
    public double Sub2Double()
    {
        var result = 0.0;
        for (var i = 0; i < 1000; i++)
        {
            result = doubleValue - 2;
        }
        return result;
    }

    [Benchmark]
    public decimal Sub2Decimal()
    {
        var result = 0.0m;
        for (var i = 0; i < 1000; i++)
        {
            result = decimalValue - 2m;
        }
        return result;
    }

    [Benchmark]
    public FixedPoint8 Sub2FixedPoint8()
    {
        var result = FixedPoint8.Zero;
        for (var i = 0; i < 1000; i++)
        {
            result = fixedPoint8Value - v2;
        }
        return result;
    }

    [Benchmark]
    public int Sub10Int()
    {
        var result = 0;
        for (var i = 0; i < 1000; i++)
        {
            result = intValue - 10;
        }
        return result;
    }

    [Benchmark]
    public double Sub10Double()
    {
        var result = 0.0;
        for (var i = 0; i < 1000; i++)
        {
            result = doubleValue - 10;
        }
        return result;
    }

    [Benchmark]
    public decimal Sub10Decimal()
    {
        var result = 0.0m;
        for (var i = 0; i < 1000; i++)
        {
            result = decimalValue - 10m;
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


    /// ////////////////////////////////////// 大なり小なり

    [Benchmark]
    public bool LessThanInt()
    {
        var result = intValue < 10;
        return result;
    }

    [Benchmark]
    public bool LessThanDouble()
    {
        var result = doubleValue < 10;
        return result;
    }

    [Benchmark]
    public bool LessThanDecimal()
    {
        var result = decimalValue < 10m;
        return result;
    }

    [Benchmark]
    public bool LessThanFixedPoint8()
    {
        var result = fixedPoint8Value < v10;
        return result;
    }


    //|                 Method |           Mean |       Error |      StdDev |         Median |
    //|----------------------- |---------------:|------------:|------------:|---------------:|
    //|                Mul2Int |    252.0915 ns |   5.0733 ns |  10.1320 ns |    249.4333 ns |
    //|             Mul2Double |    247.6842 ns |   4.8895 ns |   6.5273 ns |    250.0768 ns |
    //|            Mul2Decimal |  3,780.4062 ns |  25.2739 ns |  22.4047 ns |  3,776.3885 ns |
    //|     MulInt2FixedPoint8 |    361.1021 ns |  30.7209 ns |  90.5812 ns |    380.6810 ns |
    //|        Mul2FixedPoint8 | 48,425.0743 ns | 534.9942 ns | 500.4339 ns | 48,350.0397 ns |

    //|               Mul10Int |    338.9850 ns |  21.2681 ns |  62.7094 ns |    384.0795 ns |
    //|            Mul10Double |    248.7886 ns |   4.8682 ns |   5.6063 ns |    250.9856 ns |
    //|           Mul10Decimal |  3,817.4721 ns |  60.0774 ns |  56.1965 ns |  3,800.2945 ns |
    //|    MulInt10FixedPoint8 |    407.5580 ns |  19.4389 ns |  57.3160 ns |    417.3599 ns |
    //|       Mul10FixedPoint8 | 48,259.3571 ns | 348.5981 ns | 326.0789 ns | 48,316.4429 ns |

    //|                Add2Int |    239.0550 ns |   2.7776 ns |   2.5982 ns |    237.8625 ns |
    //|             Add2Double |    246.6463 ns |   4.8329 ns |   4.5207 ns |    248.1304 ns |
    //|            Add2Decimal |  8,185.7489 ns |  94.7229 ns |  83.9694 ns |  8,211.4624 ns |
    //|        Add2FixedPoint8 |    481.6143 ns |   6.7220 ns |   5.9589 ns |    480.0810 ns |

    //|               Add10Int |    240.1301 ns |   2.1247 ns |   1.8835 ns |    240.1348 ns |
    //|            Add10Double |    244.1696 ns |   4.6792 ns |   5.3886 ns |    242.8494 ns |
    //|           Add10Decimal |  8,056.7542 ns | 152.1530 ns | 175.2196 ns |  8,011.9179 ns |
    //|       Add10FixedPoint8 |    480.6928 ns |   9.4924 ns |   9.7480 ns |    476.9773 ns |

    //|                Sub2Int |    241.1786 ns |   3.8953 ns |   4.0002 ns |    240.3090 ns |
    //|             Sub2Double |    245.0493 ns |   4.3060 ns |   3.8172 ns |    244.8148 ns |
    //|            Sub2Decimal |  7,754.1480 ns | 110.1171 ns |  97.6159 ns |  7,789.7743 ns |
    //|        Sub2FixedPoint8 |    489.0548 ns |   9.4958 ns |  13.9189 ns |    483.1250 ns |

    //|               Sub10Int |    239.8962 ns |   2.6542 ns |   2.4827 ns |    239.3870 ns |
    //|            Sub10Double |    241.0682 ns |   3.4770 ns |   3.2524 ns |    241.1084 ns |
    //|           Sub10Decimal |  7,753.7237 ns |  83.3745 ns |  69.6215 ns |  7,780.5588 ns |
    //|       Sub10FixedPoint8 |    479.8721 ns |   4.5137 ns |   3.7691 ns |    478.9358 ns |

    //|            LessThanInt |      0.0903 ns |   0.0072 ns |   0.0063 ns |      0.0909 ns |
    //|         LessThanDouble |      0.0492 ns |   0.0244 ns |   0.0217 ns |      0.0456 ns |
    //|        LessThanDecimal |      1.2222 ns |   0.0267 ns |   0.0223 ns |      1.2134 ns |
    //|    LessThanFixedPoint8 |      0.1953 ns |   0.0167 ns |   0.0139 ns |      0.1911 ns |
}

