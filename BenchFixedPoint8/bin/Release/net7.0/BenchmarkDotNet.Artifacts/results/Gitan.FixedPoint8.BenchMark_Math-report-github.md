``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1702/22H2/2022Update/SunValley2)
AMD Ryzen 9 5900HS with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.202
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|               Method |       Mean |     Error |    StdDev |     Median |
|--------------------- |-----------:|----------:|----------:|-----------:|
|     FixedPoint8Round |  1.2365 ns | 0.0467 ns | 0.0480 ns |  1.2397 ns |
|     MathRoundDecimal |  7.1911 ns | 0.0607 ns | 0.0568 ns |  7.1869 ns |
|      MathRoundDouble |  0.0109 ns | 0.0142 ns | 0.0180 ns |  0.0000 ns |
|    FixedPoint8Round2 |  2.6201 ns | 0.0693 ns | 0.0925 ns |  2.6187 ns |
|    MathRound2Decimal |  7.3166 ns | 0.1016 ns | 0.0950 ns |  7.3226 ns |
|     MathRound2Double |  3.9170 ns | 0.1031 ns | 0.1723 ns |  3.9058 ns |
|     FixedPoint8Floor |  1.0050 ns | 0.0441 ns | 0.0686 ns |  0.9865 ns |
|     MathFloorDecimal |  6.1693 ns | 0.0455 ns | 0.0404 ns |  6.1639 ns |
|      MathFloorDouble |  0.0187 ns | 0.0166 ns | 0.0155 ns |  0.0157 ns |
|    FixedPoint8Floor2 |  1.6501 ns | 0.0429 ns | 0.0358 ns |  1.6661 ns |
|    MathFloor2Decimal | 29.0062 ns | 0.5975 ns | 0.6641 ns | 29.1964 ns |
|     MathFloor2Double |  0.0000 ns | 0.0000 ns | 0.0000 ns |  0.0000 ns |
|  FixedPoint8Truncate |  0.7222 ns | 0.0217 ns | 0.0192 ns |  0.7226 ns |
|  MathTruncateDecimal |  5.5630 ns | 0.0910 ns | 0.0851 ns |  5.5365 ns |
|   MathTruncateDouble |  0.0130 ns | 0.0098 ns | 0.0091 ns |  0.0119 ns |
| FixedPoint8Truncate2 |  1.1542 ns | 0.0479 ns | 0.1110 ns |  1.1711 ns |
| MathTruncate2Decimal | 30.2132 ns | 0.5734 ns | 0.5363 ns | 30.1327 ns |
|  MathTruncate2Double |  0.0001 ns | 0.0004 ns | 0.0007 ns |  0.0000 ns |
|   FixedPoint8Ceiling |  0.7259 ns | 0.0388 ns | 0.0531 ns |  0.7163 ns |
|   MathCeilingDecimal |  5.7350 ns | 0.1380 ns | 0.1417 ns |  5.6634 ns |
|    MathCeilingDouble |  0.0029 ns | 0.0057 ns | 0.0048 ns |  0.0000 ns |
|  FixedPoint8Ceiling2 |  1.3415 ns | 0.0518 ns | 0.1010 ns |  1.3741 ns |
|  MathCeiling2Decimal | 29.5010 ns | 0.6073 ns | 0.8902 ns | 29.3690 ns |
|   MathCeiling2Double |  0.0059 ns | 0.0096 ns | 0.0085 ns |  0.0000 ns |
