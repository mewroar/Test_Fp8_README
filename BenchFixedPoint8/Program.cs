namespace Gitan.FixedPoint8;

public class Program
{
    public static void Main()
    {
        //BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchMark_Calc>();
        //BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchMark_Parse_GetUtf8>();
        //BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchMark_Serializer>();
        BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchMark_Math>();
    }
}