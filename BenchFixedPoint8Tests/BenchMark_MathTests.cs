using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitan.FixedPoint8.Tests
{
    [TestClass()]
    public class BenchMark_MathTests
    {
        static readonly BenchMark_Math instance = new();

        // Round
        [TestMethod()]
        public void FixedPoint8RoundTest()
        {
            var result = instance.FixedPoint8Round();
            Assert.IsTrue(result == FixedPoint8.FromDecimal(2m));
        }

        [TestMethod()]
        public void MathRoundDecimalTest()
        {
            var result = instance.MathRoundDecimal();
            Assert.IsTrue(result == 2m);
        }

        [TestMethod()]
        public void MathRoundDoubleTest()
        {
            var result = instance.MathRoundDouble();
            Assert.IsTrue(result == 2);
        }
        
        [TestMethod()]
        public void FixedPoint8Round2Test()
        {
            var result = instance.FixedPoint8Round2();
            Assert.IsTrue(result == FixedPoint8.FromDecimal(1.57m));
        }

        [TestMethod()]
        public void MathRound2DecimalTest()
        {
            var result = instance.MathRound2Decimal();
            Assert.IsTrue(result == 1.57m);
        } 

        [TestMethod()]
        public void MathRound2DoubleTest()
        {
            var result = instance.MathRound2Double();
            Assert.IsTrue(result == 1.57);
        }


        // Floor
        [TestMethod()]
        public void FixedPoint8FloorTest()
        {
            var result = instance.FixedPoint8Floor();
            Assert.IsTrue(result == FixedPoint8.FromDecimal(1m));
        }

        [TestMethod()]
        public void MathFloorDecimalTest()
        {
            var result = instance.MathFloorDecimal();
            Assert.IsTrue(result == 1m);
        }

        [TestMethod()]
        public void MathFloorDoubleTest()
        {
            var result = instance.MathFloorDouble();
            Assert.IsTrue(result == 1);
        }

        [TestMethod()]
        public void FixedPoint8Floor2Test()
        {
            var result = instance.FixedPoint8Floor2();
            Assert.IsTrue(result == FixedPoint8.FromDecimal(1.56m));
        }

        [TestMethod()]
        public void MathFloor2DecimalTest()
        {
            var result = instance.MathFloor2Decimal();
            Assert.IsTrue(result == 1.56m);
        }

        [TestMethod()]
        public void MathFloor2DoubleTest()
        {
            var result = instance.MathFloor2Double();
            Assert.IsTrue(result == 1.56);
        }



        // Truncate
        [TestMethod()]
        public void FixedPoint8TruncateTest()
        {
            var result = instance.FixedPoint8Truncate();
            Assert.IsTrue(result == FixedPoint8.FromDecimal(1m));
        }

        [TestMethod()]
        public void MathTruncateDecimalTest()
        {
            var result = instance.MathTruncateDecimal();
            Assert.IsTrue(result == 1m);
        }

        [TestMethod()]
        public void MathTruncateDoubleTest()
        {
            var result = instance.MathTruncateDouble();
            Assert.IsTrue(result == 1);
        }

        [TestMethod()]
        public void FixedPoint8Truncate2Test()
        {
            var result = instance.FixedPoint8Truncate2();
            Assert.IsTrue(result == FixedPoint8.FromDecimal(1.56m));
        }

        [TestMethod()]
        public void MathTruncate2DecimalTest()
        {
            var result = instance.MathTruncate2Decimal();
            Assert.IsTrue(result == 1.56m);
        }

        [TestMethod()]
        public void MathTruncate2DoubleTest()
        {
            var result = instance.MathTruncate2Double();
            Assert.IsTrue(result == 1.56);
        }



        // Ceiling
        [TestMethod()]
        public void FixedPoint8CeilingTest()
        {
            var result = instance.FixedPoint8Ceiling();
            Assert.IsTrue(result == FixedPoint8.FromDecimal(2m));
        }

        [TestMethod()]
        public void MathCeilingDecimalTest()
        {
            var result = instance.MathCeilingDecimal();
            Assert.IsTrue(result == 2m);
        }

        [TestMethod()]
        public void MathCeilingDoubleTest()
        {
            var result = instance.MathCeilingDouble();
            Assert.IsTrue(result == 2);
        }

        [TestMethod()]
        public void FixedPoint8Ceiling2Test()
        {
            var result = instance.FixedPoint8Ceiling2();
            Assert.IsTrue(result == FixedPoint8.FromDecimal(1.57m));
        }

        [TestMethod()]
        public void MathCeiling2DecimalTest()
        {
            var result = instance.MathCeiling2Decimal();
            Assert.IsTrue(result == 1.57m);
        }

        [TestMethod()]
        public void MathCeiling2DoubleTest()
        {
            var result = instance.MathCeiling2Double();
            Assert.IsTrue(result == 1.57);
        }

    }
}