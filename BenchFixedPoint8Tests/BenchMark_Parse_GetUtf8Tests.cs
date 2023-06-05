using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitan.FixedPoint8.Tests
{
    [TestClass()]
    public class BenchMark_Parse_GetUtf8Tests
    {
        static readonly BenchMark_Parse_GetUtf8 instance = new();

        /////////////////////////////////////// ROS Parse

        [TestMethod()]
        public void ParseTests()
        {
            var result1 = instance.StringToInt();
            Assert.IsTrue(result1 == -1234);

            var result2 = instance.StringToDouble();
            Assert.IsTrue(result2 == -12.34);

            var result3 = instance.StringToDecimal();
            Assert.IsTrue(result3 == -12.34m);

            var result4 = instance.StringToFixedPoint8();
            Assert.IsTrue(result4 == new FixedPoint8(-1_234_000_000));         

            var result5 = instance.Utf8ToFixedPoint8();
            Assert.IsTrue(result5 == new FixedPoint8(-1_234_000_000));
        }      


        /////////////////////////////////////// GetUtf8
        
        
        [TestMethod()]
        public void Tostring_Utf8Tests()
        {
            var result1 = instance.IntToString();
            Assert.IsTrue(result1.Equals("-1234"));

            var result2 = instance.DoubleToString();
            Assert.IsTrue(result2.Equals("-12.34")); 
            
            var result3 = instance.DecimalToString();
            Assert.IsTrue(result3.Equals("-12.34"));
            
            var result4 = instance.FixedPoint8ToString();
            Assert.IsTrue(result4.Equals("-12.34")); 
            
            var result5 = instance.FixedPoint8ToUtf8();
            Assert.IsTrue(result5.Length.Equals(6));
        } 
    }
}