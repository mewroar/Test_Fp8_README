
using System;
using System.Text;
using System.Threading.Tasks;
using Utf8Json;
using BenchmarkDotNet.Attributes;

namespace Gitan.FixedPoint8;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<保留中>")]
public class BenchMark_Serializer
{
    /////////////////////////////////////// Reader

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


    //|                 Method |       Mean |     Error |    StdDev |
    //|----------------------- |-----------:|----------:|----------:|
    //|                ReadInt |   6.779 ns | 0.1109 ns | 0.1554 ns |
    //|               ReadLong |   5.235 ns | 0.0418 ns | 0.0391 ns |
    //|             ReadDouble |  75.224 ns | 0.7750 ns | 0.7249 ns |
    //|        ReadFixedPoint8 |   8.188 ns | 0.0412 ns | 0.0344 ns |

    //|         DeserializeInt |  44.483 ns | 0.2374 ns | 0.1853 ns |
    //|        DeserializeLong |  43.057 ns | 0.1556 ns | 0.1379 ns |
    //|      DeserializeDouble | 115.052 ns | 0.9007 ns | 0.7521 ns |
    //|     DeserializeDecimal | 117.528 ns | 0.9802 ns | 0.9168 ns |
    //| DeserializeFixedPoint8 |  50.152 ns | 0.3271 ns | 0.3060 ns |

    //|               WriteInt |   7.390 ns | 0.0747 ns | 0.0662 ns |
    //|              WriteLong |   7.096 ns | 0.0663 ns | 0.0620 ns |
    //|            WriteDouble |  78.730 ns | 0.3509 ns | 0.2740 ns |
    //|       WriteFixedPoint8 |   7.428 ns | 0.0822 ns | 0.0686 ns |

    //|           SerializeInt |  41.309 ns | 0.4833 ns | 0.4284 ns |
    //|          SerializeLong |  38.435 ns | 0.5191 ns | 0.4856 ns |
    //|        SerializeDouble | 118.182 ns | 0.5690 ns | 0.5322 ns |
    //|       SerializeDecimal | 108.904 ns | 1.2402 ns | 1.0356 ns |
    //|   SerializeFixedPoint8 |  45.996 ns | 0.4746 ns | 0.4439 ns |


    public class IntClass
    {
        public int Value { get; set; }
        public static IntClass GetSample()
        {
            var item = new IntClass()
            {
                Value = -1234,
            };
            return item;
        }
    }  
    
    public class LongClass
    {
        public long Value { get; set; }
        public static LongClass GetSample()
        {
            var item = new LongClass()
            {
                Value = -1234,
            };
            return item;
        }
    }

    public class DoubleClass
    {
        public double Value { get; set; }
        public static DoubleClass GetSample()
        {
            var item = new DoubleClass()
            {
                Value = -12.34,
            };
            return item;
        }
    }

    public class DecimalClass
    {
        public decimal Value { get; set; }
        public static DecimalClass GetSample()
        {
            var item = new DecimalClass()
            {
                Value = -12.34m,
            };
            return item;
        }
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
}

