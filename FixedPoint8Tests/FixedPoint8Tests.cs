using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gitan.FixedPoint8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

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

        //[TestMethod()]
        //public void CalcTest()
        //{
        //    // +
        //    var add1 = FixedPoint8Test.V001 + FixedPoint8Test.V001;
        //    Assert.IsTrue(add1.Equals(FixedPoint8Test.V002));
        //    var add2 = FixedPoint8Test.V1 + FixedPoint8Test.V1;
        //    Assert.IsTrue(add2.Equals(FixedPoint8Test.V2));

        //    // -
        //    var sub1 = FixedPoint8Test.V001 - FixedPoint8Test.V001;
        //    Assert.IsTrue(sub1.Equals(FixedPoint8.Zero));
        //    var sub2 = FixedPoint8Test.V1 - FixedPoint8Test.V1;
        //    Assert.IsTrue(sub2.Equals(FixedPoint8.Zero));

        //    // *(FixedPoint8 * FixedPoint8)
        //    var mul1 = new FixedPoint8(15 * 10_000_000);
        //    var mul2 = new FixedPoint8(4 * 100_000_000);
        //    var mul3 = mul1 * mul2;
        //    Assert.IsTrue(mul3.Equals(FixedPoint8.FromDecimal(6)));

        //    var mul4 = new FixedPoint8(10_000_000_000L * 100_000_000);
        //    var mul5 = new FixedPoint8(1 * 100_000_000);
        //    var mul6 = mul4 * mul5;
        //    Assert.IsTrue(mul6.Equals(FixedPoint8.FromDecimal(10_000_000_000)));

        //    var mul7 = FixedPoint8.MinValue * new FixedPoint8(-1_000_000);
        //    Assert.IsTrue(mul7.Equals(new FixedPoint8(92233720368547758)));

        //    var mul8 = mul7 * FixedPoint8.Zero;
        //    Assert.IsTrue(mul8.Equals(FixedPoint8.FromDecimal(0)));

        //    // /(FixedPoint8 / FixedPoint8)
        //    var div1 = new FixedPoint8(15_000_000_000 / 10);
        //    var div2 = new FixedPoint8(1_500_000_000 / 10);
        //    var div3 = div1 / div2;
        //    Assert.IsTrue(div3.Equals(FixedPoint8.FromDecimal(10)));

        //    var div4 = new FixedPoint8(10_000_000_000_000_000 / 1);
        //    var div5 = new FixedPoint8(1_000_000_000_000_000_000 / 10_000_000_000);
        //    var div6 = div4 / div5;
        //    Assert.IsTrue(div6.Equals(FixedPoint8.FromDecimal(100_000_000)));

        //    var div7 = new FixedPoint8(-1_000_000_000) / new FixedPoint8(-1_000_000_000);
        //    Assert.IsTrue(div7.Equals(FixedPoint8.One));

        //    // *(FixedPoint8 * long)
        //    var fp8MulLong1 = FixedPoint8.One * 2L;
        //    var fp8MulLong2 = fp8MulLong1 * 10L;
        //    Assert.IsTrue(fp8MulLong2.Equals(FixedPoint8.FromDecimal(20M)));

        //    var fp8MulLong3 = FixedPoint8Test.V001 * 1L;
        //    var fp8MulLong4 = fp8MulLong3 * 10L;
        //    Assert.IsTrue(fp8MulLong4.Equals(FixedPoint8.FromDecimal(0.1M)));

        //    var fp8MulMinusValue = new FixedPoint8(-922337203685477580) * -10L;
        //    Assert.IsTrue(fp8MulMinusValue.Equals(new FixedPoint8(9223372036854775800)));

        //    var fp8MulZero = FixedPoint8Test.V001 * 0;
        //    Assert.IsTrue(fp8MulZero.Equals(FixedPoint8.FromDecimal(0)));

        //    // /(FixedPoint8 / long)
        //    var fp8DivLong1 = FixedPoint8.One / 2L;
        //    var fp8DivLong2 = fp8DivLong1 / 10L;
        //    Assert.IsTrue(fp8DivLong2.Equals(FixedPoint8.FromDecimal(0.05M)));

        //    var fp8DivLong3 = FixedPoint8.MaxValue / 1_000_000_000L;
        //    var fp8DivLong4 = fp8DivLong3 / 10L;
        //    Assert.IsTrue(fp8DivLong4.Equals(FixedPoint8.FromDecimal(9.22337203M)));

        //}

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

                    var overPoint_a = fp8_a.InnerValue / 100000000;
                    var overPoint_b = fp8_b.InnerValue / 100000000;

                    if ((overPoint_a < 1000000 && overPoint_a > -1000000) && (overPoint_b < 1000000 && overPoint_b > -1000000))
                    {
                        var fp8_resultMul = fp8_a * fp8_b;
                        Assert.IsTrue(fp8_resultMul.Equals(FixedPoint8.FromDecimal(decimal_a * decimal_b)));

                        if (decimal_a != 0 && decimal_b != 0)
                        {
                            var fp8_resultDiv = fp8_a / fp8_b;
                            Assert.IsTrue(fp8_resultDiv.Equals(FixedPoint8.FromDecimal(decimal_a / decimal_b)));
                        }
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
                0.03m,
                0.00001m,
                123.456m,
                123456.789m,
                12233720368.54775807m,
                -1m,
                -13m,
                -100m,
                -0.03m,
                -0.00001m,
                -123.456m,
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
            // ==
            var equality1 = FixedPoint8Test.V1 == FixedPoint8Test.V1;
            Assert.IsTrue(equality1);
            var equality2 = FixedPoint8Test.V1 == FixedPoint8Test.V2;
            Assert.IsFalse(equality2);

            // !=
            var inequality1 = FixedPoint8Test.V1 != FixedPoint8Test.V1;
            Assert.IsFalse(inequality1);
            var inequality2 = FixedPoint8Test.V1 != FixedPoint8Test.V2;
            Assert.IsTrue(inequality2);

            // <
            var lessThan = new FixedPoint8(1_000_000_000) < new FixedPoint8(1_000_000_000);
            Assert.IsFalse(lessThan);
            // <=
            var lessThanEqual1 = new FixedPoint8(1_000_000_000) <= new FixedPoint8(1_000_000_000);
            Assert.IsTrue(lessThanEqual1);
            // >
            var greaterThan = new FixedPoint8(1_000_000_000) > new FixedPoint8(1_000_000_000);
            Assert.IsFalse(greaterThan);
            // >=
            var greaterThanEqual1 = new FixedPoint8(1_000_000_000) >= new FixedPoint8(1_000_000_000);
            Assert.IsTrue(greaterThanEqual1);

            var lessThanEqual2 = new FixedPoint8(1_000_000_000) <= new FixedPoint8(2_000_000_000);
            Assert.IsTrue(lessThanEqual2);

            var greaterThanEqual2 = new FixedPoint8(1_000_000_000) >= new FixedPoint8(2_000_000_000);
            Assert.IsFalse(greaterThanEqual2);

        }


        [TestMethod()]
        public void IncrementDecrementTest()
        {
            var v1 = new FixedPoint8(250_000_000);
            v1++;
            Assert.IsTrue(v1.Equals(FixedPoint8.FromDouble(3.5)));

            var v2 = new FixedPoint8(250_000_000);
            v2--;
            Assert.IsTrue(v2.Equals(FixedPoint8.FromDouble(1.5)));
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

        public void DoubleDecimalCastTest()
        {
            // double ⇒ FixedPoint8
            double doubleValue = 0.01;
            var v1 = (FixedPoint8)doubleValue;
            Assert.IsTrue(v1.Equals(FixedPoint8Test.V001));

            // FixedPoint8 ⇒ double
            var valueFp8Double = FixedPoint8Test.V001;
            double v2 = (double)valueFp8Double;
            Assert.IsTrue(v2.Equals(0.01));

            // decimal ⇒ FixedPoint8
            decimal decimalValue = 0.12345678M;
            var v3 = (FixedPoint8)decimalValue;
            Assert.IsTrue(v3.Equals(new FixedPoint8(12_345_678)));

            // FixedPoint8 ⇒ decimal
            var valueFp8Decimal = FixedPoint8.MinValue;
            decimal v4 = (decimal)valueFp8Decimal;
            Assert.IsTrue(v4.Equals(-92233720368.54775808M));
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
        public void CastFromFp8Test()
        {
            var fp8sByteList = new Dictionary<FixedPoint8, sbyte>()
            {
                { FixedPoint8 .Zero,0},
                { FixedPoint8 .One,1},
                { new FixedPoint8(12_700_000_000),sbyte.MaxValue},
                { -FixedPoint8 .One,-1},
                { new FixedPoint8(-12_800_000_000),sbyte.MinValue},
            };

            foreach (KeyValuePair<FixedPoint8, sbyte> dic in fp8sByteList)
            {
                var value = (sbyte)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var fp8ByteList = new Dictionary<FixedPoint8, byte>()
            {
                { FixedPoint8 .Zero,0},
                { FixedPoint8 .One,1},
                { new FixedPoint8(25_500_000_000),byte.MaxValue},

            };

            foreach (KeyValuePair<FixedPoint8, byte> dic in fp8ByteList)
            {
                var value = (byte)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var fp8ShortList = new Dictionary<FixedPoint8, short>()
            {
                { FixedPoint8 .Zero,0},
                { FixedPoint8 .One,1},
                { new FixedPoint8(3_276_700_000_000),short.MaxValue},
                { -FixedPoint8 .One,-1},
                { new FixedPoint8(-3_276_800_000_000),short.MinValue},
            };

            foreach (KeyValuePair<FixedPoint8, short> dic in fp8ShortList)
            {
                var value = (short)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var fp8uShortList = new Dictionary<FixedPoint8, ushort>()
            {
                { FixedPoint8 .Zero,0},
                { FixedPoint8 .One,1},
                { new FixedPoint8(6_553_500_000_000),ushort.MaxValue},
            };

            foreach (KeyValuePair<FixedPoint8, ushort> dic in fp8uShortList)
            {
                var value = (ushort)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var fp8IntlList = new Dictionary<FixedPoint8, int>()
            {
                { FixedPoint8 .Zero,0},
                { FixedPoint8 .One,1},
                {new FixedPoint8(int.MaxValue),21},
                { -FixedPoint8 .One,-1},
                {new FixedPoint8(int.MinValue),-21},
            };

            foreach (KeyValuePair<FixedPoint8, int> dic in fp8IntlList)
            {
                var value = (int)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var fp8uIntlList = new Dictionary<FixedPoint8, uint>()
            {
                { FixedPoint8 .Zero,0u},
                { FixedPoint8 .One,1u},
                {new FixedPoint8(uint.MaxValue),42u},

            };

            foreach (KeyValuePair<FixedPoint8, uint> dic in fp8uIntlList)
            {
                var value = (uint)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var fp8LongList = new Dictionary<FixedPoint8, long>()
            {
                { FixedPoint8 .Zero,0l},
                { FixedPoint8 .One,1l},
                { FixedPoint8.MaxValue,long.MaxValue/100_000_000},
                { -FixedPoint8 .One,-1l},
                { FixedPoint8.MinValue,long.MinValue/100_000_000},
            };

            foreach (KeyValuePair<FixedPoint8, long> dic in fp8LongList)
            {
                var value = (long)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var fp8uLongList = new Dictionary<FixedPoint8, ulong>()
            {
                { FixedPoint8 .Zero,0ul},
                { FixedPoint8 .One,1ul},
            };

            foreach (KeyValuePair<FixedPoint8, ulong> dic in fp8uLongList)
            {
                var value = (ulong)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var fp8FloatlList = new Dictionary<FixedPoint8, float>()
            {
                { FixedPoint8 .Zero,0f},
                { FixedPoint8 .One,1f},
                { FixedPoint8.MaxValue,92233720368.54775807f},
                {new FixedPoint8(1_000_000),0.01f},
                { -FixedPoint8 .One,-1f},
                { FixedPoint8.MinValue,-92233720368.54775808f},
                {new FixedPoint8(-1_000_000),-0.01f},
            };

            foreach (KeyValuePair<FixedPoint8, float> dic in fp8FloatlList)
            {
                var value = (float)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var fp8DoubleList = new Dictionary<FixedPoint8, double>()
            {
                { FixedPoint8 .Zero,0},
                { FixedPoint8 .One,1},
                { FixedPoint8.MaxValue,92233720368.54775807},
                {new FixedPoint8(1_000_000),0.01},
                { -FixedPoint8 .One,-1},
                { FixedPoint8.MinValue,-92233720368.54775808},
                {new FixedPoint8(-1_000_000),-0.01},
            };

            foreach(KeyValuePair<FixedPoint8,double>dic in fp8DoubleList)
            {
                var value = (double)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var fp8DecimalList = new Dictionary<FixedPoint8, decimal>()
            {
                { FixedPoint8 .Zero,0m},
                { FixedPoint8 .One,1m},
                { FixedPoint8.MaxValue,92233720368.54775807m},
                {new FixedPoint8(1_000_000),0.01m},
                { -FixedPoint8 .One,-1m},
                { FixedPoint8.MinValue,-92233720368.54775808m},
                {new FixedPoint8(-1_000_000),-0.01m},
            }; 
            
            foreach (KeyValuePair<FixedPoint8,decimal>dic in fp8DecimalList)
            {
                var value = (decimal)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }  
        }


        [TestMethod]
        public void CastToFp8Test()
        {
            sbyte sbyteZero = 0;
            sbyte sbyteOne = 1;
            sbyte sbyteMOne = -1;
            var sbyteFp8Dic = new Dictionary<sbyte, FixedPoint8>()
            {
                { sbyteZero,FixedPoint8.Zero},
                { sbyteOne,FixedPoint8.One},
                { sbyte.MaxValue,new FixedPoint8(12700000000)},
                { sbyteMOne,-FixedPoint8.One},
                { sbyte.MinValue,new FixedPoint8(-12800000000)},
            };

            foreach (KeyValuePair<sbyte, FixedPoint8> dic in sbyteFp8Dic)
            {
                var value = (FixedPoint8)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var byteFp8Dic = new Dictionary<byte, FixedPoint8>()
            {
                { byte.MinValue,FixedPoint8.Zero},
                { byte.MaxValue,new FixedPoint8(25500000000)},
            };

            foreach (KeyValuePair<byte, FixedPoint8> dic in byteFp8Dic)
            {
                var value = (FixedPoint8)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            short shortZero = 0;
            short shortOne = 1;
            short shortMOne = -1;
            var shortFp8Dic = new Dictionary<short, FixedPoint8>()
            {
                { shortZero,FixedPoint8.Zero},
                { shortOne,FixedPoint8.One},
                { short.MaxValue,new FixedPoint8(3276700000000)},
                { shortMOne,-FixedPoint8.One},
                { short.MinValue,new FixedPoint8(-3276800000000)},
            };

            foreach (KeyValuePair<short, FixedPoint8> dic in shortFp8Dic)
            {
                var value = (FixedPoint8)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var ushortFp8Dic = new Dictionary<ushort, FixedPoint8>()
            {
                { ushort.MinValue,FixedPoint8.Zero},
                { ushort.MaxValue,new FixedPoint8(6553500000000)},
            };

            foreach (KeyValuePair<ushort, FixedPoint8> dic in ushortFp8Dic)
            {
                var value = (FixedPoint8)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var intFp8Dic = new Dictionary<int, FixedPoint8>()
            {
                { 0,FixedPoint8.Zero},
                { 1,FixedPoint8.One},
                { int.MaxValue,new FixedPoint8(214748364700000000)},
                { -1,-FixedPoint8.One},
                { int.MinValue,new FixedPoint8(-214748364800000000)},
            };

            foreach (KeyValuePair<int, FixedPoint8> dic in intFp8Dic)
            {
                var value = (FixedPoint8)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var uintFp8Dic = new Dictionary<uint, FixedPoint8>()
            {
                { 0u,FixedPoint8.Zero},
                { 1u,FixedPoint8.One},
                { uint.MaxValue,new FixedPoint8(429496729500000000)},
            };

            foreach (KeyValuePair<uint, FixedPoint8> dic in uintFp8Dic)
            {
                var value = (FixedPoint8)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var longFp8Dic = new Dictionary<long, FixedPoint8>()
            {
                { 0L,FixedPoint8.Zero},
                { 1L,FixedPoint8.One},
                { long.MaxValue/100000000,new FixedPoint8(9223372036800000000)},
                { -1L,-FixedPoint8.One},
                { long.MinValue/100000000,new FixedPoint8(-9223372036800000000)},
            };

            foreach (KeyValuePair<long, FixedPoint8> dic in longFp8Dic)
            {
                var value = (FixedPoint8)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var ulongFp8Dic = new Dictionary<ulong, FixedPoint8>()
            {
                { 0ul,FixedPoint8.Zero},
                { 1ul,FixedPoint8.One},
            };

            foreach (KeyValuePair<ulong, FixedPoint8> dic in ulongFp8Dic)
            {
                var value = (FixedPoint8)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var floatFp8Dic = new Dictionary<float, FixedPoint8>()
            {
                { 0f,FixedPoint8.Zero},
                { 1f,FixedPoint8.One},
                { 0.01f,new FixedPoint8(1_000_000)},
                { 123.456f,new FixedPoint8(12345600000)},
                { -1f,-FixedPoint8.One},
                { -0.01f,new FixedPoint8(-1_000_000)},
                { -123.456f,new FixedPoint8(-12345600000)},
            };

            foreach (KeyValuePair<float, FixedPoint8> dic in floatFp8Dic)
            {
                var value = (FixedPoint8)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var doubleFp8Dic = new Dictionary<double, FixedPoint8>()
            {
                { 0d,FixedPoint8.Zero},
                { 1d,FixedPoint8.One},
                { 0.01d,new FixedPoint8(1_000_000)},
                { 123.456d,new FixedPoint8(12345600000)},
                { -1d,-FixedPoint8.One},
                { -0.01d,new FixedPoint8(-1_000_000)},
                { -123.456d,new FixedPoint8(-12345600000)},
            };

            foreach (KeyValuePair<double, FixedPoint8> dic in doubleFp8Dic)
            {
                var value = (FixedPoint8)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }

            var decimalFp8Dic = new Dictionary<decimal, FixedPoint8>()
            {
                { 0m,FixedPoint8.Zero},
                { 1m,FixedPoint8.One},
                { 0.01m,new FixedPoint8(1_000_000)},
                { 123.456m,new FixedPoint8(12345600000)},
                { -1m,-FixedPoint8.One},
                { -0.01m,new FixedPoint8(-1_000_000)},
                { -123.456m,new FixedPoint8(-12345600000)},
            };

            foreach (KeyValuePair<decimal, FixedPoint8> dic in decimalFp8Dic)
            {
                var value = (FixedPoint8)dic.Key;
                Assert.IsTrue(value.Equals(dic.Value));
            }
        }



        //[TestMethod()] //
        //public void CastTest()
        //{
            //doubleCast
            //var fp8ToDouble1 = (double)FixedPoint8Test.V001;
            //Assert.IsTrue(fp8ToDouble1.Equals(0.01));
            //var fp8ToDouble2 = (double)-FixedPoint8Test.V1;
            //Assert.IsTrue(fp8ToDouble2.Equals(-1));
            //var fp8ToDouble3 = (double)new FixedPoint8(120_000_000);
            //Assert.IsTrue(fp8ToDouble3.Equals(1.2));
            //var fp8ToDouble4 = (double)new FixedPoint8(-120_000_000);
            //Assert.IsTrue(fp8ToDouble4.Equals(-1.2));

            ////decimalCast
            //var fp8ToDecimal1 = (decimal)FixedPoint8Test.V001;
            //Assert.IsTrue(fp8ToDecimal1.Equals(0.01M));
            //var fp8ToDecimal2 = (decimal)-FixedPoint8Test.V1;
            //Assert.IsTrue(fp8ToDecimal2.Equals(-1M));
            //var fp8ToDecimal3 = (decimal)new FixedPoint8(150_000_000);
            //Assert.IsTrue(fp8ToDecimal3.Equals(1.5M));
            //var fp8ToDecimal4 = (decimal)new FixedPoint8(-150_000_000);
            //Assert.IsTrue(fp8ToDecimal4.Equals(-1.5M));

            ////floatCast
            //var fp8ToFloat1 = (float)FixedPoint8Test.V001;
            //Assert.IsTrue(fp8ToFloat1.Equals(0.01F));
            //var fp8ToFloat2 = (float)-FixedPoint8Test.V1;
            //Assert.IsTrue(fp8ToFloat2.Equals(-1F));
            //var fp8ToFloat3 = (float)new FixedPoint8(150_000_000);
            //Assert.IsTrue(fp8ToFloat3.Equals(1.5F));
            //var fp8ToFloat4 = (float)new FixedPoint8(-150_000_000);
            //Assert.IsTrue(fp8ToFloat4.Equals(-1.5F));

            ////intCast
            //var fp8ToInt1 = (int)new FixedPoint8(0);
            //Assert.IsTrue(fp8ToInt1.Equals(0));
            //var fp8ToInt2 = (int)new FixedPoint8(int.MinValue);
            //Assert.IsTrue(fp8ToInt2.Equals(-21));
            //var fp8ToInt3 = (int)new FixedPoint8(int.MaxValue);
            //Assert.IsTrue(fp8ToInt3.Equals(21));
            //var fp8ToInt4 = (int)new FixedPoint8(190_000_000);
            //Assert.IsTrue(fp8ToInt4.Equals(1));
            //var fp8ToInt5 = (int)new FixedPoint8(-190_000_000);
            //Assert.IsTrue(fp8ToInt5.Equals(-1));

            ////uintCast
            //var fp8ToUint1 = (uint)new FixedPoint8(uint.MinValue);
            //Assert.IsTrue(fp8ToUint1.Equals(0));
            //var fp8ToUint2 = (uint)new FixedPoint8(uint.MaxValue);
            //Assert.IsTrue(fp8ToUint2.Equals(42));
            //var fp8ToUint3 = (uint)new FixedPoint8(120_000_000);
            //Assert.IsTrue(fp8ToUint3.Equals(1));

            ////longCast
            //var fp8ToLong1 = (long)FixedPoint8.Zero;
            //Assert.IsTrue(fp8ToLong1.Equals(0));
            //var fp8ToLong2 = (long)FixedPoint8.MinValue;
            //Assert.IsTrue(fp8ToLong2.Equals(-92233720368));
            //var fp8ToLong3 = (long)FixedPoint8.MaxValue;
            //Assert.IsTrue(fp8ToLong3.Equals(92233720368));
            //var fp8ToLong4 = (long)new FixedPoint8(150_000_000);
            //Assert.IsTrue(fp8ToLong4.Equals(1));
            //var fp8ToLong5 = (long)new FixedPoint8(-150_000_000);
            //Assert.IsTrue(fp8ToLong5.Equals(-1));

            ////ulongCast
            //var fp8ToUlong1 = (ulong)new FixedPoint8(0);
            //Assert.IsTrue(fp8ToUlong1.Equals(0));
            //var fp8ToUlong2 = (ulong)new FixedPoint8(100_000_000_000);
            //Assert.IsTrue(fp8ToUlong2.Equals(1000));
            //var fp8ToUlong3 = (ulong)new FixedPoint8(150_000_000);
            //Assert.IsTrue(fp8ToUlong3.Equals(1));


            ////shortCast
            //var fp8ToShort1 = (short)new FixedPoint8(0);
            //Assert.IsTrue(fp8ToShort1.Equals(0));
            //var fp8ToShort2 = (short)new FixedPoint8(-350_000_000_000);
            //Assert.IsTrue(fp8ToShort2.Equals(-3500));
            //var fp8ToShort3 = (short)new FixedPoint8(8_000_000_000);
            //Assert.IsTrue(fp8ToShort3.Equals(80));
            //var fp8ToShort4 = (short)new FixedPoint8(150_000_000);
            //Assert.IsTrue(fp8ToShort4.Equals(1));
            //var fp8ToShort5 = (short)new FixedPoint8(-150_000_000);
            //Assert.IsTrue(fp8ToShort5.Equals(-1));

            ////ushortCast
            //var fp8ToUshort1 = (ushort)new FixedPoint8(0);
            //Assert.IsTrue(fp8ToUshort1.Equals(0));
            //var fp8ToUshort2 = (ushort)new FixedPoint8(125_000_000_000);
            //Assert.IsTrue(fp8ToUshort2.Equals(1250));
            //var fp8ToUshort3 = (ushort)new FixedPoint8(150_000_000);
            //Assert.IsTrue(fp8ToUshort3.Equals(1));

            ////sbyteCast
            //var fp8ToSbyte1 = (sbyte)new FixedPoint8(0);
            //Assert.IsTrue(fp8ToSbyte1.Equals(0));
            //var fp8ToSbyte2 = (sbyte)new FixedPoint8(-12_800_000_000);
            //Assert.IsTrue(fp8ToSbyte2.Equals(-128));
            //var fp8ToSbyte3 = (sbyte)new FixedPoint8(12_700_000_000);
            //Assert.IsTrue(fp8ToSbyte3.Equals(127));
            //var fp8ToSbyte4 = (sbyte)new FixedPoint8(120_000_000);
            //Assert.IsTrue(fp8ToSbyte4.Equals(1));
            //var fp8ToSbyte5 = (sbyte)new FixedPoint8(-120_000_000);
            //Assert.IsTrue(fp8ToSbyte5.Equals(-1));

            ////byteCast
            //var fp8ToByte1 = (byte)new FixedPoint8(0);
            //Assert.IsTrue(fp8ToByte1.Equals(0));
            //var fp8ToByte2 = (byte)new FixedPoint8(25_500_000_000);
            //Assert.IsTrue(fp8ToByte2.Equals(255));
            //var fp8ToByte3 = (byte)new FixedPoint8(150_000_000);
            //Assert.IsTrue(fp8ToByte3.Equals(1));

            //FixedPoint8Cast
            //short s = 32767;
            //ushort us = 65535;
            //byte b = 255;
            //sbyte sb = 127;

            //var doubleToFp8 = (FixedPoint8)0.01d;
            //Assert.IsTrue(doubleToFp8.Equals(new FixedPoint8(1_000_000)));
            //var decimalToFp8 = (FixedPoint8)0.01m;
            //Assert.IsTrue(decimalToFp8.Equals(new FixedPoint8(1_000_000)));
            //var floatToFp8 = (FixedPoint8)0.01f;
            //Assert.IsTrue(floatToFp8.Equals(new FixedPoint8(1_000_000)));
            //var intToFp8 = (FixedPoint8)12345;
            //Assert.IsTrue(intToFp8.Equals(new FixedPoint8(1_234_500_000_000)));
            //var uintToFp8 = (FixedPoint8)12345u;
            //Assert.IsTrue(uintToFp8.Equals(new FixedPoint8(1_234_500_000_000)));
            //var longToFp8 = (FixedPoint8)12345L;
            //Assert.IsTrue(longToFp8.Equals(new FixedPoint8(1_234_500_000_000)));
            //var ulongToFp8 = (FixedPoint8)12345ul;
            //Assert.IsTrue(ulongToFp8.Equals(new FixedPoint8(1_234_500_000_000)));
            //var shortToFp8 = (FixedPoint8)s;
            //Assert.IsTrue(shortToFp8.Equals(new FixedPoint8(3_276_700_000_000)));
            //var ushortToFp8 = (FixedPoint8)us;
            //Assert.IsTrue(ushortToFp8.Equals(new FixedPoint8(6_553_500_000_000)));
            //var byteToFp8 = (FixedPoint8)b;
            //Assert.IsTrue(byteToFp8.Equals(new FixedPoint8(25_500_000_000)));
            //var sbyteToFp8 = (FixedPoint8)sb;
            //Assert.IsTrue(sbyteToFp8.Equals(new FixedPoint8(12_700_000_000)));

            //var mDoubleToFp8 = (FixedPoint8)(-0.01d);
            //Assert.IsTrue(mDoubleToFp8.Equals(new FixedPoint8(-1_000_000)));
            //var mDecimalToFp8 = (FixedPoint8)(-0.01m);
            //Assert.IsTrue(mDecimalToFp8.Equals(new FixedPoint8(-1_000_000)));
            //var mFloatToFp8 = (FixedPoint8)(-0.01f);
            //Assert.IsTrue(mFloatToFp8.Equals(new FixedPoint8(-1_000_000)));
            //var mIntToFp8 = (FixedPoint8)(-12345);
            //Assert.IsTrue(mIntToFp8.Equals(new FixedPoint8(-1_234_500_000_000)));
            //var mLongToFp8 = (FixedPoint8)(-12345L);
            //Assert.IsTrue(mLongToFp8.Equals(new FixedPoint8(-1_234_500_000_000)));
            //var mShortToFp8 = (FixedPoint8)(-s);
            //Assert.IsTrue(mShortToFp8.Equals(new FixedPoint8(-3_276_700_000_000)));
            //var mSbyteToFp8 = (FixedPoint8)(-sb);
            //Assert.IsTrue(mSbyteToFp8.Equals(new FixedPoint8(-12_700_000_000)));
        //}


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
