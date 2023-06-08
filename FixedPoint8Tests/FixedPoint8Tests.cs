using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gitan.FixedPoint8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using Newtonsoft.Json.Linq;

namespace Gitan.FixedPoint8.Tests
{
    public readonly struct FixedPoint8Test
    {
        public static FixedPoint8 V001 { get; } = new FixedPoint8(1_000_000); //0.01
        public static FixedPoint8 V002 { get; } = new FixedPoint8(2_000_000); //0.02
        public static FixedPoint8 V004 { get; } = new FixedPoint8(4_000_000); //0.04
        public static FixedPoint8 V005 { get; } = new FixedPoint8(5_000_000); //0.05
        public static FixedPoint8 V1 { get; } = new FixedPoint8(100_000_000); //1
        public static FixedPoint8 V2 { get; } = new FixedPoint8(200_000_000); //2

        public static FixedPoint8 V0p0 { get; } = new FixedPoint8(00_000_000); //0.0
        public static FixedPoint8 MaxPrice { get; } = new FixedPoint8(9_999_999_999_999_999);
        public static FixedPoint8 MinPrice { get; } = new FixedPoint8(100_000_000_000);

    }



    [TestClass()]
    public class FixedPoint8Tests
    {

        [TestMethod()]
        public void ToUtf8Test()
        {

            var v1 = FixedPoint8.FromInnerValue(100_000_000); // 1
            var max = FixedPoint8.FromInnerValue(long.MaxValue); // 92233720368.54775807
            var min = FixedPoint8.FromInnerValue(long.MinValue); // -92233720368.54775808
            var zero = FixedPoint8.FromInnerValue(0); // 0
            var plus_p01 = FixedPoint8.FromDouble(0.01); // 0.01
            var minus_p01 = FixedPoint8.FromDouble(-0.01); // -0.01
            var v12p34 = FixedPoint8.FromInnerValue(1_234_000_000); // 12.34
            var other = FixedPoint8.FromDouble(1.00909); // 1.00909

            var array1 = ToUtf8_Core(other);
            Assert.IsTrue(array1.Length.Equals(7));

            var array2 = ToUtf8_Core(v1);
            Assert.IsTrue(array2.Length.Equals(1));

            var array3 = ToUtf8_Core(v12p34);
            Assert.IsTrue(array3.Length.Equals(5));

            var array4 = ToUtf8_Core(max);
            Assert.IsTrue(array4.Length.Equals(20));

            var array5 = ToUtf8_Core(min);
            Assert.IsTrue(array5.Length.Equals(21));

            var array6 = ToUtf8_Core(zero);
            Assert.IsTrue(array6.Length.Equals(1));

            var array7 = ToUtf8_Core(plus_p01);
            Assert.IsTrue(array7.Length.Equals(4));

            var array8 = ToUtf8_Core(minus_p01);
            Assert.IsTrue(array8.Length.Equals(5));

        }

        public static byte[] ToUtf8_Core(FixedPoint8 value)
        {
            var byteArray = value.ToUtf8();
            return byteArray;
        }


        [TestMethod()]
        public void CalcTest()
        {
            var list = GetDecimals();
            foreach(var decimal_a in list)
            {
                var fp8_a = FixedPoint8.FromDecimal(decimal_a);

                foreach (var decimal_b in list)
                {
                    var fp8_b = FixedPoint8.FromDecimal(decimal_b);

                    var fp8_resultAdd = fp8_a + fp8_b;
                    Assert.IsTrue(fp8_resultAdd.Equals(FixedPoint8.FromDecimal(decimal_a + decimal_b)));
                    
                    var fp8_resultSub = fp8_a - fp8_b;
                    Assert.IsTrue(fp8_resultSub.Equals(FixedPoint8.FromDecimal(decimal_a - decimal_b)));

                    if ((Math.Abs(decimal_a) < 303700) && (Math.Abs(decimal_b) < 303700))
                    {
                        var fp8_resultMul = fp8_a * fp8_b;
                        Assert.IsTrue(fp8_resultMul.Equals(FixedPoint8.FromDecimal(decimal_a * decimal_b)));
                    }

                    if ((Math.Abs(decimal_a) < 303700) && (Math.Abs(decimal_b) > 1.0m / 303700))
                    {
                        var fp8_resultDiv = fp8_a / fp8_b;
                        Assert.IsTrue(fp8_resultDiv.Equals(FixedPoint8.FromDecimal(decimal_a / decimal_b)));
                    }
                }
            }
        }

        public static List<decimal> GetDecimals()
        {
            var result = new List<decimal>()
            {
                0m,
                1m,
                13m,
                100m,
                303699.99m,
                0.03m,
                0.00001m,
                0.0000001m,
                123.456m,
                123.789m,
                123456.789m,
                12233720368.54775807m,
                -1m,
                -13m,
                -100m,
                -303699.99m,
                -0.03m,
                -0.00001m,
                -0.0000001m,
                -123.456m,
                -123.789m,
                -123456.789m,
                -12233720368.54775808m,
            };

            return result;
        }

        [TestMethod()]
        public void PercentTest()
        {
            var list = GetDecimals();
            foreach (var decimal_a in list)
            {
                var fp8_a = FixedPoint8.FromDecimal(decimal_a);

                foreach (var decimal_b in list)
                {
                    var fp8_b = FixedPoint8.FromDecimal(decimal_b);

                    decimal decimal_result = 0m;
                    bool decimal_exception = false;
                    try
                    {
                        decimal_result = decimal_a % decimal_b;
                    }
                    catch
                    {
                        decimal_exception = true;
                    }

                    FixedPoint8 fp8_result = FixedPoint8.Zero;
                    bool fp8_exception = false;
                    try
                    {
                        fp8_result = fp8_a % fp8_b;
                    }
                    catch
                    {
                        fp8_exception = true;
                    }

                    Assert.IsTrue(decimal_exception == fp8_exception);

                    if (decimal_exception == false)
                    {
                        Assert.IsTrue(FixedPoint8.FromDecimal(decimal_result) == fp8_result);
                    }
                }

            }
        }

        [TestMethod()]
        public void CompTest()
        {
            var list = GetDecimals();

            // ==
            foreach (var decimal_a in list)
            {
                var fp8_a = FixedPoint8.FromDecimal(decimal_a);

                foreach(var decimal_b in list)
                {
                    var fp8_b = FixedPoint8.FromDecimal(decimal_b);

                    if(decimal_a == decimal_b)
                    {
                        Assert.IsTrue(fp8_a == fp8_b);
                    }
                    if (decimal_a != decimal_b)
                    {
                        Assert.IsFalse(fp8_a == fp8_b);
                    }
                }
            }

            // !=
            foreach (var decimal_a in list)
            {
                var fp8_a = FixedPoint8.FromDecimal(decimal_a);

                foreach (var decimal_b in list)
                {
                    var fp8_b = FixedPoint8.FromDecimal(decimal_b);

                    if (decimal_a == decimal_b)
                    {
                        Assert.IsFalse(fp8_a != fp8_b);
                    }
                    if (decimal_a != decimal_b)
                    {
                        Assert.IsTrue(fp8_a != fp8_b);
                    }
                }
            }

            // <
            foreach (var decimal_a in list)
            {
                var fp8_a = FixedPoint8.FromDecimal(decimal_a);

                foreach (var decimal_b in list)
                {
                    var fp8_b = FixedPoint8.FromDecimal(decimal_b);

                    if (decimal_a < decimal_b)
                    {
                        Assert.IsTrue(fp8_a < fp8_b);
                    }
                    if (decimal_a > decimal_b)
                    {
                        Assert.IsFalse(fp8_a < fp8_b);
                    }
                }
            }

            // <=
            foreach (var decimal_a in list)
            {
                var fp8_a = FixedPoint8.FromDecimal(decimal_a);

                foreach (var decimal_b in list)
                {
                    var fp8_b = FixedPoint8.FromDecimal(decimal_b);

                    if (decimal_a != decimal_b)
                    {
                        if (decimal_a <= decimal_b)
                        {
                            Assert.IsTrue(fp8_a <= fp8_b);
                        }
                        if (decimal_a >= decimal_b)
                        {
                            Assert.IsFalse(fp8_a <= fp8_b);
                        }
                    }
                    else
                    {
                        Assert.IsTrue(fp8_a <= fp8_b);
                    }
                }
            }

            // >
            foreach (var decimal_a in list)
            {
                var fp8_a = FixedPoint8.FromDecimal(decimal_a);

                foreach (var decimal_b in list)
                {
                    var fp8_b = FixedPoint8.FromDecimal(decimal_b);

                    if (decimal_a > decimal_b)
                    {
                        Assert.IsTrue(fp8_a > fp8_b);
                    }
                    if (decimal_a < decimal_b)
                    {
                        Assert.IsFalse(fp8_a > fp8_b);
                    }
                }
            }

            // >=
            foreach (var decimal_a in list)
            {
                var fp8_a = FixedPoint8.FromDecimal(decimal_a);

                foreach (var decimal_b in list)
                {
                    var fp8_b = FixedPoint8.FromDecimal(decimal_b);

                    if (decimal_a != decimal_b)
                    {
                        if (decimal_a >= decimal_b)
                        {
                            Assert.IsTrue(fp8_a >= fp8_b);
                        }
                        if (decimal_a <= decimal_b)
                        {
                            Assert.IsFalse(fp8_a >= fp8_b);
                        }
                    }
                    else
                    {
                        Assert.IsTrue(fp8_a >= fp8_b);
                    }
                }
            }
        }


        [TestMethod()]
        public void IncrementDecrementTest()
        {
            var list = GetDecimals();

            foreach(var decimal_a in list)
            {
                var fp8 = FixedPoint8.FromDecimal(decimal_a);
                var decimalIncrement = decimal_a + 1;
                fp8++;

                Assert.IsTrue((FixedPoint8)decimalIncrement == fp8);
            }

            foreach(var decimal_a in list)
            {
                var fp8 = FixedPoint8.FromDecimal(decimal_a);
                var decimalIncrement = decimal_a - 1;
                fp8--;

                Assert.IsTrue((FixedPoint8)decimalIncrement == fp8);
            }
        }

        [TestMethod()]
        public void PlusMinusTest()
        {
            var v1 = FixedPoint8Test.V001;

            var v2 = -FixedPoint8Test.V001;
            Assert.IsFalse(v1.Equals(v2));

            var v3 = -v2;
            Assert.IsTrue(v1.Equals(v3));
        }

        [TestMethod()]
        public void EqualsTest()
        {
            var v1 = FixedPoint8Test.V001;
            var v2 = FixedPoint8Test.V001;
            Assert.IsTrue(v1.Equals(v2));

            object v3 = FixedPoint8Test.V001;
            Assert.IsTrue(v1.Equals(v3));

        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            var dic = new Dictionary<FixedPoint8, FixedPoint8>();
            long innerValue = 0;
            for (int i = 0; i <= 10000; i++)
            {
                innerValue += 1_000_000;
                var value = FixedPoint8.FromInnerValue(innerValue);
                dic.Add(value, value);
            }
        }

        [TestMethod()]
        public void CompareToTest()
        {
            var v1 = FixedPoint8.One;
            var v2 = FixedPoint8.Zero;
            Assert.IsTrue(v1.CompareTo(v2) > 0);

            var v3 = FixedPoint8Test.V2;
            Assert.IsTrue(v1.CompareTo(v3) < 0);

            object v4 = FixedPoint8.One;
            Assert.IsTrue(v1.CompareTo(v4) == 0);
        }

        [TestMethod()]
        public void AbsTest()
        {
            var v1 = FixedPoint8Test.V001;
            var v2 = FixedPoint8.Abs(v1);
            Assert.IsTrue(v1.Equals(v2));

            var v3 = -FixedPoint8Test.V001;
            var v4 = FixedPoint8.Abs(v3);
            Assert.IsTrue(v1.Equals(v4));

        }

        [TestMethod()]
        public void IsCanonicalTest()
        {
            var result = FixedPoint8.IsCanonical(FixedPoint8Test.V001);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsComplexNumberTest()
        {
            var result = FixedPoint8.IsComplexNumber(FixedPoint8Test.V001);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsEvenIntegerTest()
        {
            var v1 = FixedPoint8Test.V1;
            var v2 = FixedPoint8.IsEvenInteger(v1);
            Assert.IsFalse(v2);

            var v3 = FixedPoint8Test.V2;
            var v4 = FixedPoint8.IsEvenInteger(v3);
            Assert.IsTrue(v4);

            var v5 = FixedPoint8Test.V001;
            var v6 = FixedPoint8.IsEvenInteger(v5);
            Assert.IsFalse(v6);

            var v7 = FixedPoint8.FromDouble(-3.0);
            var v8 = FixedPoint8.IsEvenInteger(v7);
            Assert.IsFalse(v8);
        }

        [TestMethod()]
        public void IsFiniteTest()
        {
            var result = FixedPoint8.IsFinite(FixedPoint8Test.V001);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsImaginaryNumberTest()
        {
            var result = FixedPoint8.IsImaginaryNumber(FixedPoint8Test.V001);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsInfinityTest()
        {
            var result = FixedPoint8.IsInfinity(FixedPoint8Test.V001);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsIntegerTest()
        {
            var v1 = FixedPoint8.IsInteger(FixedPoint8Test.V001);
            Assert.IsFalse(v1);

            var v2 = FixedPoint8.IsInteger(FixedPoint8.One);
            Assert.IsTrue(v2);
        }

        [TestMethod()]
        public void IsNaNTest()
        {
            var result = FixedPoint8.IsNaN(FixedPoint8Test.V001);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsNegativeTest()
        {
            var v1 = FixedPoint8.IsNegative(FixedPoint8.Zero);
            Assert.IsFalse(v1);

            var v2 = FixedPoint8.IsNegative(-FixedPoint8.One);
            Assert.IsTrue(v2);
        }

        [TestMethod()]
        public void IsNegativeInfinityTest()
        {
            var result = FixedPoint8.IsNegativeInfinity(FixedPoint8Test.V001);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsNormalTest()
        {
            var result = FixedPoint8.IsNormal(FixedPoint8Test.V001);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsOddIntegerTest()
        {
            var v1 = FixedPoint8Test.V1;
            var v2 = FixedPoint8.IsOddInteger(v1);
            Assert.IsTrue(v2);

            var v3 = FixedPoint8Test.V2;
            var v4 = FixedPoint8.IsOddInteger(v3);
            Assert.IsFalse(v4);

            var v5 = FixedPoint8Test.V001;
            var v6 = FixedPoint8.IsOddInteger(v5);
            Assert.IsFalse(v6);

            var v7 = FixedPoint8.FromDouble(2.0);
            var v8 = FixedPoint8.IsOddInteger(v7);
            Assert.IsFalse(v8);
        }

        [TestMethod()]
        public void IsPositiveTest()
        {
            var v1 = FixedPoint8.IsPositive(FixedPoint8.Zero);
            Assert.IsTrue(v1);

            var v2 = FixedPoint8.IsPositive(-FixedPoint8.One);
            Assert.IsFalse(v2);
        }

        [TestMethod()]
        public void IsPositiveInfinityTest()
        {
            var result = FixedPoint8.IsPositiveInfinity(FixedPoint8Test.V001);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsRealNumberTest()
        {
            var result = FixedPoint8.IsRealNumber(FixedPoint8Test.V001);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsSubnormalTest()
        {
            var result = FixedPoint8.IsSubnormal(FixedPoint8Test.V001);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsZeroTest()
        {
            var v1 = FixedPoint8.Zero;
            var v2 = FixedPoint8.IsZero(v1);
            Assert.IsTrue(v2);

            var v3 = FixedPoint8.One;
            var v4 = FixedPoint8.IsZero(v3);
            Assert.IsFalse(v4);
        }


        [TestMethod()]
        public void MaxMagnitudeTest()
        {
            var maxMag = FixedPoint8.MaxMagnitude(FixedPoint8Test.V005, FixedPoint8Test.V001);
            Assert.IsTrue(maxMag.Equals(FixedPoint8Test.V005));

            var maxMagSameNum = FixedPoint8.MaxMagnitude(FixedPoint8Test.V001, -FixedPoint8Test.V001);
            Assert.IsTrue(maxMagSameNum.Equals(FixedPoint8Test.V001));
        }

        [TestMethod()]
        public void MaxMagnitudeNumberTest()
        {
            var maxMag = FixedPoint8.MaxMagnitudeNumber(FixedPoint8Test.V005, FixedPoint8Test.V001);
            Assert.IsTrue(maxMag.Equals(FixedPoint8Test.V005));

            var maxMagSameNum = FixedPoint8.MaxMagnitudeNumber(FixedPoint8Test.V001, -FixedPoint8Test.V001);
            Assert.IsTrue(maxMagSameNum.Equals(FixedPoint8Test.V001));
        }

        [TestMethod()]
        public void MinMagnitudeTest()
        {
            var minMag = FixedPoint8.MinMagnitude(FixedPoint8Test.V001, FixedPoint8Test.V005);
            Assert.IsTrue(minMag.Equals(FixedPoint8Test.V001));

            var minMagSameNum = FixedPoint8.MinMagnitude(FixedPoint8Test.V001, -FixedPoint8Test.V001);
            Assert.IsTrue(minMagSameNum.Equals(-FixedPoint8Test.V001));
        }

        [TestMethod()]
        public void MinMagnitudeNumberTest()
        {
            var minMag = FixedPoint8.MinMagnitudeNumber(FixedPoint8Test.V001, FixedPoint8Test.V005);
            Assert.IsTrue(minMag.Equals(FixedPoint8Test.V001));

            var minMagSameNum = FixedPoint8.MinMagnitudeNumber(FixedPoint8Test.V001, -FixedPoint8Test.V001);
            Assert.IsTrue(minMagSameNum.Equals(-FixedPoint8Test.V001));
        }


        [TestMethod()]
        public void Prase_TryParseTest()
        {
            var trueDic = new Dictionary<string, decimal>()
            {
                { "0",0m },
                { "0.01",0.01m },
                { ".01",0.01m },
                {"1",1m },
                { "1.0",1.0m },
                { "-0",-0m },
                { "-0.01",-0.01m },
                { "-.01",-0.01m },
                {"-1",-1m },
                { "-1.0",-1.0m },
                {"123.456",123.456m},
                {"-123.456",-123.456m},
                {"0.01234",0.01234m},
                {"-0.01234",-0.01234m},
            };

            var trueDicBytes = new Dictionary<string, decimal>()
            {
                { "0",0m },
                { "0.01",0.01m },
                { ".01",0.01m },
                {"1",1m },
                { "1.0",1.0m },
                { "-0",-0m },
                { "-0.01",-0.01m },
                { "-.01",-0.01m },
                {"-1",-1m },
                { "-1.0",-1.0m },
                {"123.456",123.456m},
                {"-123.456",-123.456m},
                {"0.01234",0.01234m},
                {"-0.01234",-0.01234m},
                {"1.2e3",1200m},
                {"1.2e+3",1200m},
                {"1.2e-3",0.0012m},
                {"1.2E3",1200m}
            };


            var falseList = new List<string>()
            {
                "--",
                "++",
                "a",
                "9a",
                "1.2.3",
                "-1.2.3",
                "1.2x",
                "5.0ea",
                "5.0e3b",
            };

            bool result;
            FixedPoint8 value;

            foreach (KeyValuePair<string, decimal> dic in trueDic)
            {
                result = FixedPoint8.TryParse(dic.Key, out value);
                Assert.IsTrue(result);
                Assert.IsTrue(value == FixedPoint8.FromDecimal(dic.Value));

                result = FixedPoint8.TryParse(dic.Key.ToCharArray(), out value);
                Assert.IsTrue(result);
                Assert.IsTrue(value == FixedPoint8.FromDecimal(dic.Value));

                result = true;
                try
                {
                    value = FixedPoint8.Parse(dic.Key);
                }
                catch
                {
                    result = false;
                }
                Assert.IsTrue(result);
                Assert.IsTrue(value == FixedPoint8.FromDecimal(dic.Value));

                result = true;
                try
                {
                    value = FixedPoint8.Parse(dic.Key.ToCharArray());
                }
                catch
                {
                    result = false;
                }
                Assert.IsTrue(result);
                Assert.IsTrue(value == FixedPoint8.FromDecimal(dic.Value));

            }

            foreach (KeyValuePair<string, decimal> dic in trueDicBytes)
            {
                result = FixedPoint8.TryParse(Encoding.UTF8.GetBytes(dic.Key), out value);
                Assert.IsTrue(result);
                Assert.IsTrue(value == FixedPoint8.FromDecimal(dic.Value));

                result = true;
                try
                {
                    value = FixedPoint8.Parse(Encoding.UTF8.GetBytes(dic.Key));
                }
                catch
                {
                    result = false;
                }
                Assert.IsTrue(result);
                Assert.IsTrue(value == FixedPoint8.FromDecimal(dic.Value));

            }

            foreach (var item in falseList)
            {
                result = FixedPoint8.TryParse(item, out value);
                Assert.IsFalse(result);

                result = FixedPoint8.TryParse(Encoding.UTF8.GetBytes(item), out value);
                Assert.IsFalse(result);

                result = true;
                try
                {
                    value = FixedPoint8.Parse(item);
                }
                catch
                {
                    result = false;
                }
                Assert.IsFalse(result);

                result = true;
                try
                {
                    value = FixedPoint8.Parse(Encoding.UTF8.GetBytes(item));
                }
                catch
                {
                    result = false;
                }
                Assert.IsFalse(result);

            }

        }


        [TestMethod()]
        public void CastTest()
        {
            var list = GetDecimals();
            foreach (var decimal_a in list)
            {
                if (sbyte.MinValue <= decimal_a && decimal_a <= sbyte.MaxValue)
                {
                    var fp8_a = FixedPoint8.FromDecimal(decimal_a);
                    var decimalTo = (sbyte)decimal_a;
                    var fp8To = (sbyte)fp8_a;
                    Assert.IsTrue(decimalTo == fp8To);

                    var fromFp8_a = (FixedPoint8)decimalTo;
                    var fromFp8_b = (FixedPoint8)fp8To;
                    Assert.IsTrue(fromFp8_a == fromFp8_b);
                }

                if (byte.MinValue <= decimal_a && decimal_a <= byte.MaxValue)
                {
                    var fp8_a = FixedPoint8.FromDecimal(decimal_a);
                    var decimalTo = (byte)decimal_a;
                    var fp8To = (byte)fp8_a;
                    Assert.IsTrue(decimalTo == fp8To);

                    var fromFp8_a = (FixedPoint8)decimalTo;
                    var fromFp8_b = (FixedPoint8)fp8To;
                    Assert.IsTrue(fromFp8_a == fromFp8_b);
                }

                if (short.MinValue <= decimal_a && decimal_a <= short.MaxValue)
                {
                    var fp8_a = FixedPoint8.FromDecimal(decimal_a);
                    var decimalTo = (short)decimal_a;
                    var fp8To = (short)fp8_a;
                    Assert.IsTrue(decimalTo == fp8To);

                    var fromFp8_a = (FixedPoint8)decimalTo;
                    var fromFp8_b = (FixedPoint8)fp8To;
                    Assert.IsTrue(fromFp8_a == fromFp8_b);
                }

                if (ushort.MinValue <= decimal_a && decimal_a <= ushort.MaxValue)
                {
                    var fp8_a = FixedPoint8.FromDecimal(decimal_a);
                    var decimalTo = (ushort)decimal_a;
                    var fp8To = (ushort)fp8_a;
                    Assert.IsTrue(decimalTo == fp8To);

                    var fromFp8_a = (FixedPoint8)decimalTo;
                    var fromFp8_b = (FixedPoint8)fp8To;
                    Assert.IsTrue(fromFp8_a == fromFp8_b);
                }

                if (int.MinValue <= decimal_a && decimal_a <= int.MaxValue)
                {
                    var fp8_a = FixedPoint8.FromDecimal(decimal_a);
                    var decimalTo = (int)decimal_a;
                    var fp8To = (int)fp8_a;
                    Assert.IsTrue(decimalTo == fp8To);

                    var fromFp8_a = (FixedPoint8)decimalTo;
                    var fromFp8_b = (FixedPoint8)fp8To;
                    Assert.IsTrue(fromFp8_a == fromFp8_b);
                }

                if (uint.MinValue <= decimal_a && decimal_a <= uint.MaxValue)
                {
                    var fp8_a = FixedPoint8.FromDecimal(decimal_a);
                    var decimalTo = (uint)decimal_a;
                    var fp8To = (uint)fp8_a;
                    Assert.IsTrue(decimalTo == fp8To);

                    var fromFp8_a = (FixedPoint8)decimalTo;
                    var fromFp8_b = (FixedPoint8)fp8To;
                    Assert.IsTrue(fromFp8_a == fromFp8_b);
                }

                if (92233720368.54775808m <= decimal_a && decimal_a <= 92233720368.54775807m)
                {
                    var fp8_a = FixedPoint8.FromDecimal(decimal_a);
                    var decimalTo = (long)decimal_a;
                    var fp8To = (long)fp8_a;
                    Assert.IsTrue(decimalTo == fp8To);

                    var fromFp8_a = (FixedPoint8)decimalTo;
                    var fromFp8_b = (FixedPoint8)fp8To;
                    Assert.IsTrue(fromFp8_a == fromFp8_b);
                }

                if (92233720368.54775808m <= decimal_a && decimal_a <= 92233720368.54775807m)
                {
                    var fp8_a = FixedPoint8.FromDecimal(decimal_a);
                    var decimalTo = (ulong)decimal_a;
                    var fp8To = (ulong)fp8_a;
                    Assert.IsTrue(decimalTo == fp8To);

                    var fromFp8_a = (FixedPoint8)decimalTo;
                    var fromFp8_b = (FixedPoint8)fp8To;
                    Assert.IsTrue(fromFp8_a == fromFp8_b);
                }

                if (92233720368.54775808m <= decimal_a && decimal_a <= 92233720368.54775807m)
                {
                    var fp8_a = FixedPoint8.FromDecimal(decimal_a);
                    var decimalTo = (float)decimal_a;
                    var fp8To = (float)fp8_a;
                    Assert.IsTrue(decimalTo == fp8To);

                    var fromFp8_a = (FixedPoint8)decimalTo;
                    var fromFp8_b = (FixedPoint8)fp8To;
                    Assert.IsTrue(fromFp8_a == fromFp8_b);
                }

                if (92233720368.54775808m <= decimal_a && decimal_a <= 92233720368.54775807m)
                {
                    var fp8_a = FixedPoint8.FromDecimal(decimal_a);
                    var decimalTo = (double)decimal_a;
                    var fp8To = (double)fp8_a;
                    Assert.IsTrue(decimalTo == fp8To);

                    var fromFp8_a = (FixedPoint8)decimalTo;
                    var fromFp8_b = (FixedPoint8)fp8To;
                    Assert.IsTrue(fromFp8_a == fromFp8_b);
                }

                if (92233720368.54775808m <= decimal_a && decimal_a <= 92233720368.54775807m)
                {
                    var fp8_a = FixedPoint8.FromDecimal(decimal_a);
                    var decimalTo = decimal_a;
                    var fp8To = (decimal)fp8_a;
                    Assert.IsTrue(decimalTo == fp8To);

                    var fromFp8_a = (FixedPoint8)decimalTo;
                    var fromFp8_b = (FixedPoint8)fp8To;
                    Assert.IsTrue(fromFp8_a == fromFp8_b);
                }
            }
        }


        [TestMethod()]
        public void MathTest()
        {
            MathTest2(0m);
            MathTest2(5500.00m);
            MathTest2(-5500.00m);
            MathTest2(6500.00m);
            MathTest2(-6500.00m);
            MathTest2(1234.5678m);
            MathTest2(12345678912.12345678m);
            MathTest2(-12345678912.12345678m);
            MathTest2(1.2345m);
            MathTest2(1.2356m);
            MathTest2(1.0m);
            MathTest2(1.5m);
            MathTest2(2.5m);
            MathTest2(-1.0m);
            MathTest2(-1.5m);
            MathTest2(-2.5m);
            MathTest2(-1.215m);
            MathTest2(-1.225m);
            MathTest2(-1.2215m);
            MathTest2(-1.2225m);
            MathTest2(1.5645m);
            MathTest2(1.5656m);
            MathTest2(-1.2345m);
            MathTest2(-1.2356m);
            MathTest2(-1.5645m);
            MathTest2(-1.5656m);
            MathTest2(1234.56m);
            MathTest2(-1234.56m);
            MathTest2(1234567.89m);
            MathTest2(-1234567.89m);
        }

        [TestMethod()]
        public void MathTest3()
        {
            var math = Math.Floor(-1.510m * 100)/100;
            var fp8 = FixedPoint8.FromDecimal(-1.510m);
            var result = fp8.Floor(2);
        }

        public void MathTest2(decimal d)
        {
            var fp8 = FixedPoint8.FromDecimal(d);

            var mathRound = Math.Round(d);
            var fp8Round = fp8.Round();
            Assert.IsTrue((decimal)fp8Round == mathRound);

            for (int decimals = -10; decimals < 7; decimals++)
            {
                decimal d2 = d;
                decimal mathRound2;
                if (decimals < 0)
                {
                    for (int i = 0; i < -decimals; i++)
                    {
                        d2 /= 10;
                    }
                    mathRound2 = Math.Round(d2);

                    for (int i = 0; i < -decimals; i++)
                    {
                        mathRound2 *= 10;
                    }
                }
                else
                {
                    mathRound2 = Math.Round(d, decimals);
                }
                var fp8Round2 = fp8.Round(decimals);
                Assert.IsTrue((decimal)fp8Round2 == mathRound2);
            }


            var mathFloor = Math.Floor(d);
            var fp8Floor = fp8.Floor();
            Assert.IsTrue((decimal)fp8Floor == mathFloor);

            for (int decimals = -10; decimals < 7; decimals++)
            {
                decimal d2 = d;
                decimal mathFloor2;
                if (decimals < 0)
                {
                    for (int i = 0; i < -decimals; i++)
                    {
                        d2 /= 10;
                    }
                    mathFloor2 = Math.Floor(d2);

                    for (int i = 0; i < -decimals; i++)
                    {
                        mathFloor2 *= 10;
                    }
                }
                else
                {
                    for (int i = 0; i < decimals; i++)
                    {
                        d2 *= 10;
                    }
                    mathFloor2 = Math.Floor(d2);

                    for (int i = 0; i < decimals; i++)
                    {
                        mathFloor2 /= 10;
                    }
                }
                var fp8Floor2 = fp8.Floor(decimals);
                Assert.IsTrue((decimal)fp8Floor2 == mathFloor2);
            }


            var mathTruncate = Math.Truncate(d);
            var fp8Truncate = fp8.Truncate();
            Assert.IsTrue((decimal)fp8Floor == mathFloor);

            for (int decimals = -10; decimals < 7; decimals++)
            {
                decimal d2 = d;
                decimal mathTruncate2;
                if (decimals < 0)
                {
                    for (int i = 0; i < -decimals; i++)
                    {
                        d2 /= 10;
                    }
                    mathTruncate2 = Math.Truncate(d2);

                    for (int i = 0; i < -decimals; i++)
                    {
                        mathTruncate2 *= 10;
                    }
                }
                else
                {
                    for (int i = 0; i < decimals; i++)
                    {
                        d2 *= 10;
                    }
                    mathTruncate2 = Math.Truncate(d2);

                    for (int i = 0; i < decimals; i++)
                    {
                        mathTruncate2 /= 10;
                    }
                }
                var fp8Truncate2 = fp8.Truncate(decimals);
                Assert.IsTrue((decimal)fp8Truncate2 == mathTruncate2);
            }


            var mathCeiling = Math.Ceiling(d);
            var fp8Ceiling = fp8.Ceiling();
            Assert.IsTrue((decimal)fp8Ceiling == mathCeiling);

            for (int decimals = -10; decimals < 7; decimals++)
            {
                decimal d2 = d;
                decimal mathCeiling2;
                if (decimals < 0)
                {
                    for (int i = 0; i < -decimals; i++)
                    {
                        d2 /= 10;
                    }
                    mathCeiling2 = Math.Ceiling(d2);

                    for (int i = 0; i < -decimals; i++)
                    {
                        mathCeiling2 *= 10;
                    }
                }
                else
                {
                    for (int i = 0; i < decimals; i++)
                    {
                        d2 *= 10;
                    }
                    mathCeiling2 = Math.Ceiling(d2);

                    for (int i = 0; i < decimals; i++)
                    {
                        mathCeiling2 /= 10;
                    }
                }
                var fp8Ceiling2 = fp8.Ceiling(decimals);
                Assert.IsTrue((decimal)fp8Ceiling2 == mathCeiling2);
            }
        }
    }
}
