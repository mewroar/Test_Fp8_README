using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitan.FixedPoint8.Tests
{
    [TestClass()]
    public class BenchMark_SerializerTests
    {
        static readonly BenchMark_Serializer instance = new();

        /////////////////////////////////////// Reader
        
        [TestMethod()]
        public void ReadIntTest()
        {
            var result = instance.ReadInt();
            Assert.IsTrue(result == -1234);
        }

        [TestMethod()]
        public void ReadDoubleTest()
        {
            var result = instance.ReadDouble();
            Assert.IsTrue(result == -12.34);
        }

        [TestMethod()]
        public void ReadFixedPoint8Test()
        {
            var result = instance.ReadFixedPoint8();
            Assert.IsTrue(result == new FixedPoint8(-1234000000));
        }

        /////////////////////////////////////// Deserialize
        
        [TestMethod()]
        public void DeserializeIntTest()
        {
            var result = instance.DeserializeInt();
            Assert.IsTrue(result == -1234);
        }

        [TestMethod()]
        public void DeserializeDoubleTest()
        {
            var result = instance.DeserializeDouble();
            Assert.IsTrue(result == -12.34);
        } 
        
        [TestMethod()]
        public void DeserializeDecimalTest()
        {
            var result = instance.DeserializeDecimal();
            Assert.IsTrue(result == -12.34m);
        }

        [TestMethod()]
        public void DeserializeFixedPoint8Test()
        {
            var result = instance.DeserializeFixedPoint8();
            Assert.IsTrue(result == new FixedPoint8(-1234000000));
        }

        /////////////////////////////////////// Writer

        [TestMethod()]
        public void WriterIntTest()
        {
            instance.WriteInt();
        }

        [TestMethod()]
        public void WriterDoubleTest()
        {
            instance.WriteDouble();
        }

        [TestMethod()]
        public void WriteFixedPoint8Test()
        {
            instance.WriteFixedPoint8();
        }

        /////////////////////////////////////// Serialize

        [TestMethod()]
        public void SerializeIntTest()
        {
            instance.SerializeInt();
        }

        [TestMethod()]
        public void SerializeDoubleTest()
        {
            instance.SerializeDouble();
        }

        [TestMethod()]
        public void SerializeDecimalTest()
        {
            instance.SerializeDecimal();
        }

        [TestMethod()]
        public void SerializeFixedPoint8Test()
        {
            instance.SerializeFixedPoint8();
        }

    }
}