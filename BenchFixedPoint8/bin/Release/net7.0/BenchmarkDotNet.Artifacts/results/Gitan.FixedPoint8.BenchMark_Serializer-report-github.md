``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1702/22H2/2022Update/SunValley2)
AMD Ryzen 9 5900HS with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.202
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|                 Method |     Mean |    Error |   StdDev |
|----------------------- |---------:|---------:|---------:|
| DeserializeFixedPoint8 | 51.10 ns | 1.022 ns | 2.132 ns |
