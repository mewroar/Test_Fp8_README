using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitan.FixedPoint8.Tests
{
    [TestClass()]
    public class BenchMark_CalcTests
    {
        static readonly BenchMark_Calc instance = new();

        /////////////////////////////////////// multiplication	

        [TestMethod()]
        public void Mul2Tests()
        {
            var result1 = instance.Mul2Int();
            Assert.IsTrue(result1.Equals(-2468)); 
            
            var result2 = instance.Mul2Double();
            Assert.IsTrue(result2.Equals(-24.68));
            
            var result3 = instance.Mul2Decimal();
            Assert.IsTrue(result3.Equals(-24.68m)); 
            
            var result4 = instance.Mul2FixedPoint8();
            Assert.IsTrue(result4.Equals(new FixedPoint8(-2_468_000_000)));
        }     
        
        [TestMethod()]
        public void Mul10Tests()
        {
            var result1 = instance.Mul10Int();
            Assert.IsTrue(result1.Equals(-12340)); 
            
            var result2 = instance.Mul10Double();
            Assert.IsTrue(result2.Equals(-123.4));
            
            var result3 = instance.Mul10Decimal();
            Assert.IsTrue(result3.Equals(-123.4m)); 
            
            var result4 = instance.Mul10FixedPoint8();
            Assert.IsTrue(result4.Equals(new FixedPoint8(-12_340_000_000)));
        }



        /////////////////////////////////////// addition

        [TestMethod()]
        public void Add2Tests()
        {
            var result1 = instance.Add2Int();
            Assert.IsTrue(result1.Equals(-1232));

            var result2 = instance.Add2Double();
            Assert.IsTrue(result2.Equals(-10.34));

            var result3 = instance.Add2Decimal();
            Assert.IsTrue(result3.Equals(-10.34m));

            var result4 = instance.Add2FixedPoint8();
            Assert.IsTrue(result4.Equals(new FixedPoint8(-1_034_000_000)));
        }

        [TestMethod()]
        public void Add10Tests()
        {
            var result1 = instance.Add10Int();
            Assert.IsTrue(result1.Equals(-1224));

            var result2 = instance.Add10Double();
            Assert.IsTrue(result2.Equals(-2.34));

            var result3 = instance.Add10Decimal();
            Assert.IsTrue(result3.Equals(-2.34m));

            var result4 = instance.Add10FixedPoint8();
            Assert.IsTrue(result4.Equals(new FixedPoint8(-234_000_000)));
        }


        /////////////////////////////////////// subtraction	

        [TestMethod()]
        public void Sub2Tests()
        {
            var result1 = instance.Sub2Int();
            Assert.IsTrue(result1.Equals(-1236));

            var result2 = instance.Sub2Double();
            Assert.IsTrue(result2.Equals(-14.34));

            var result3 = instance.Sub2Decimal();
            Assert.IsTrue(result3.Equals(-14.34m));

            var result4 = instance.Sub2FixedPoint8();
            Assert.IsTrue(result4.Equals(new FixedPoint8(-1_434_000_000)));
        }

        [TestMethod()]
        public void Sub10Tests()
        {
            var result1 = instance.Sub10Int();
            Assert.IsTrue(result1.Equals(-1244));

            var result2 = instance.Sub10Double();
            Assert.IsTrue(result2.Equals(-22.34));

            var result3 = instance.Sub10Decimal();
            Assert.IsTrue(result3.Equals(-22.34m));

            var result4 = instance.Sub10FixedPoint8();
            Assert.IsTrue(result4.Equals(new FixedPoint8(-2_234_000_000)));
        }


        /////////////////////////////////////// Less than

        [TestMethod()]
        public void LessThanTests()
        {
            var result1 = instance.LessThanInt();
            Assert.IsTrue(result1);

            var result2 = instance.LessThanDouble();
            Assert.IsTrue(result2);

            var result3 = instance.LessThanDecimal();
            Assert.IsTrue(result3);

            var result4 = instance.LessThanFixedPoint8();
            Assert.IsTrue(result4);
        }
    }
}