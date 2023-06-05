
using System;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Gitan.FixedPoint8;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<保留中>")]
public class BenchMark_Math
{
    static FixedPoint8 fp8Value = FixedPoint8.FromDecimal(-1.5678m);
    static decimal decimalValue = -1.5678m;
    static double doubleValue = -1.5678;

    // Round
    [Benchmark]
    public FixedPoint8 FixedPoint8Round()
    {
        var result = fp8Value.Round();
        return result;
    }

    [Benchmark]
    public decimal MathRoundDecimal()
    {
        var result = Math.Round(decimalValue);
        return result;
    }

    [Benchmark]
    public double MathRoundDouble()
    {
        var result = Math.Round(doubleValue);
        return result;
    }

    [Benchmark]
    public FixedPoint8 FixedPoint8Round2()
    {
        var result = fp8Value.Round(2);
        return result;
    }

    [Benchmark]
    public decimal MathRound2Decimal()
    {
        var result = Math.Round(decimalValue, 2);
        return result;
    }
    
    [Benchmark]
    public double MathRound2Double()
    {
        var result = Math.Round(doubleValue, 2);
        return result;
    }


    // Floor
    [Benchmark]
    public FixedPoint8 FixedPoint8Floor()
    {
        var result = fp8Value.Floor();
        return result;
    }

    [Benchmark]
    public decimal MathFloorDecimal()
    {
        var result = Math.Floor(decimalValue);
        return result;
    }

    [Benchmark]
    public double MathFloorDouble()
    {
        var result = Math.Floor(doubleValue);
        return result;
    }

    [Benchmark]
    public FixedPoint8 FixedPoint8Floor2()
    {
        var result = fp8Value.Floor(2);
        return result;
    }

    [Benchmark]
    public decimal MathFloor2Decimal()
    {
        var result = Math.Floor(decimalValue * 100) / 100;
        return result;
    }

    [Benchmark]
    public double MathFloor2Double()
    {
        var result = Math.Floor(doubleValue * 100) / 100;
        return result;
    }


    // Truncate
    [Benchmark]
    public FixedPoint8 FixedPoint8Truncate()
    {
        var result = fp8Value.Truncate();
        return result;
    }

    [Benchmark]
    public decimal MathTruncateDecimal()
    {
        var result = Math.Truncate(decimalValue);
        return result;
    }

    [Benchmark]
    public double MathTruncateDouble()
    {
        var result = Math.Truncate(doubleValue);
        return result;
    }

    [Benchmark]
    public FixedPoint8 FixedPoint8Truncate2()
    {
        var result = fp8Value.Truncate(2);
        return result;
    }

    [Benchmark]
    public decimal MathTruncate2Decimal()
    {
        var result = Math.Truncate(decimalValue * 100) / 100;
        return result;
    }

    [Benchmark]
    public double MathTruncate2Double()
    {
        var result = Math.Truncate(doubleValue * 100) / 100;
        return result;
    }



    // Ceiling
    [Benchmark]
    public FixedPoint8 FixedPoint8Ceiling()
    {
        var result = fp8Value.Ceiling();
        return result;
    }

    [Benchmark]
    public decimal MathCeilingDecimal()
    {
        var result = Math.Ceiling(decimalValue);
        return result;
    }

    [Benchmark]
    public double MathCeilingDouble()
    {
        var result = Math.Ceiling(doubleValue);
        return result;
    }

    [Benchmark]
    public FixedPoint8 FixedPoint8Ceiling2()
    {
        var result = fp8Value.Ceiling(2);
        return result;
    }

    [Benchmark]
    public decimal MathCeiling2Decimal()
    {
        var result = Math.Ceiling(decimalValue * 100) / 100;
        return result;
    }

    [Benchmark]
    public double MathCeiling2Double()
    {
        var result = Math.Ceiling(doubleValue * 100) / 100;
        return result;
    }

    // Round
    //|               Method |       Mean |     Error |    StdDev |     Median |
    //|--------------------- |-----------:|----------:|----------:|-----------:|
    //|     FixedPoint8Round |  1.4010 ns | 0.0451 ns | 0.0400 ns |  1.3898 ns |
    //|     MathRoundDecimal |  7.1975 ns | 0.0836 ns | 0.0698 ns |  7.2278 ns |
    //|      MathRoundDouble |  0.0242 ns | 0.0177 ns | 0.0138 ns |  0.0210 ns |
    //|    FixedPoint8Round2 |  2.5243 ns | 0.1138 ns | 0.3355 ns |  2.5198 ns |
    //|    MathRound2Decimal |  7.2582 ns | 0.1511 ns | 0.1413 ns |  7.1807 ns |
    //|     MathRound2Double |  3.9704 ns | 0.1009 ns | 0.1311 ns |  3.9911 ns |

    // Floor
    //|     FixedPoint8Floor |  0.7209 ns | 0.0217 ns | 0.0182 ns |  0.7177 ns |
    //|     MathFloorDecimal |  5.5960 ns | 0.1306 ns | 0.1158 ns |  5.5834 ns |
    //|      MathFloorDouble |  0.2077 ns | 0.0287 ns | 0.0509 ns |  0.2240 ns |
    //|    FixedPoint8Floor2 |  1.5462 ns | 0.0546 ns | 0.1152 ns |  1.5376 ns |
    //|    MathFloor2Decimal | 29.0802 ns | 0.5947 ns | 0.7304 ns | 29.1511 ns |
    //|     MathFloor2Double |  0.0010 ns | 0.0025 ns | 0.0024 ns |  0.0000 ns |

    // Truncate
    //|  FixedPoint8Truncate |  0.5102 ns | 0.0332 ns | 0.0311 ns |  0.5089 ns |
    //|  MathTruncateDecimal |  5.4864 ns | 0.1018 ns | 0.0952 ns |  5.4812 ns |
    //|   MathTruncateDouble |  0.0358 ns | 0.0244 ns | 0.0228 ns |  0.0292 ns |
    //| FixedPoint8Truncate2 |  1.0878 ns | 0.0451 ns | 0.0790 ns |  1.1026 ns |
    //| MathTruncate2Decimal | 29.4732 ns | 0.5756 ns | 0.5102 ns | 29.6164 ns |
    //|  MathTruncate2Double |  0.0002 ns | 0.0007 ns | 0.0006 ns |  0.0000 ns |

    // Ceiling
    //|   FixedPoint8Ceiling |  0.9382 ns | 0.0214 ns | 0.0190 ns |  0.9298 ns |
    //|   MathCeilingDecimal |  6.1030 ns | 0.0692 ns | 0.0614 ns |  6.0949 ns |
    //|    MathCeilingDouble |  0.0097 ns | 0.0114 ns | 0.0089 ns |  0.0075 ns |
    //|  FixedPoint8Ceiling2 |  1.4759 ns | 0.0687 ns | 0.2026 ns |  1.4904 ns |
    //|  MathCeiling2Decimal | 28.6468 ns | 0.4819 ns | 0.4272 ns | 28.6874 ns |
    //|   MathCeiling2Double |  0.0015 ns | 0.0055 ns | 0.0051 ns |  0.0000 ns |


//    |--------------------- |-----------:|----------:|----------:|-----------:|
//|     FixedPoint8Round |  1.2597 ns | 0.0495 ns | 0.0589 ns |  1.2419 ns |
//|     MathRoundDecimal |  7.3546 ns | 0.1598 ns | 0.1495 ns |  7.2857 ns |
//|      MathRoundDouble |  0.0090 ns | 0.0150 ns | 0.0167 ns |  0.0000 ns |
//|    FixedPoint8Round2 |  2.4360 ns | 0.0724 ns | 0.0967 ns |  2.4378 ns |
//|    MathRound2Decimal |  7.3145 ns | 0.0843 ns | 0.0747 ns |  7.3288 ns |
//|     MathRound2Double |  3.8750 ns | 0.1016 ns | 0.1457 ns |  3.8876 ns |

//|     FixedPoint8Floor |  0.5431 ns | 0.0338 ns | 0.0282 ns |  0.5429 ns |
//|     MathFloorDecimal |  5.8083 ns | 0.1372 ns | 0.1784 ns |  5.7828 ns |
//|      MathFloorDouble |  0.0261 ns | 0.0230 ns | 0.0351 ns |  0.0071 ns |
//|    FixedPoint8Floor2 |  1.5508 ns | 0.0547 ns | 0.0986 ns |  1.5537 ns |
//|    MathFloor2Decimal | 29.5772 ns | 0.6028 ns | 0.8251 ns | 29.7859 ns |
//|     MathFloor2Double |  0.0150 ns | 0.0173 ns | 0.0206 ns |  0.0022 ns |

//|  FixedPoint8Truncate |  0.5126 ns | 0.0256 ns | 0.0227 ns |  0.5103 ns |
//|  MathTruncateDecimal |  5.6190 ns | 0.1214 ns | 0.1014 ns |  5.6030 ns |
//|   MathTruncateDouble |  0.0053 ns | 0.0115 ns | 0.0102 ns |  0.0000 ns |
//| FixedPoint8Truncate2 |  1.0413 ns | 0.0445 ns | 0.0731 ns |  1.0377 ns |
//| MathTruncate2Decimal | 29.6899 ns | 0.5406 ns | 0.6837 ns | 29.6902 ns |
//|  MathTruncate2Double |  0.0021 ns | 0.0054 ns | 0.0048 ns |  0.0000 ns |

//|   FixedPoint8Ceiling |  0.9555 ns | 0.0272 ns | 0.0241 ns |  0.9513 ns |
//|   MathCeilingDecimal |  6.1574 ns | 0.0862 ns | 0.0673 ns |  6.1405 ns |
//|    MathCeilingDouble |  0.0121 ns | 0.0154 ns | 0.0172 ns |  0.0000 ns |
//|  FixedPoint8Ceiling2 |  1.3698 ns | 0.0512 ns | 0.0949 ns |  1.3787 ns |
//|  MathCeiling2Decimal | 28.9548 ns | 0.5401 ns | 0.5052 ns | 28.9100 ns |
//|   MathCeiling2Double |  0.0009 ns | 0.0027 ns | 0.0024 ns |  0.0000 ns |

}

