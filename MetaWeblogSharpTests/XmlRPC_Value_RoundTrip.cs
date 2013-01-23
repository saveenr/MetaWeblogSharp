using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetaWeblogSharp.XmlRPC;
using X=MetaWeblogSharp.XmlRPC;

namespace MetaWeblogSharpTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var struct_ = new MetaWeblogSharp.XmlRPC.Struct();
        }

        [TestMethod]
        public void RoundTrip_Array()
        {
            var src = new X.Array();
            src.Add(0);
            src.Add(18);
            src.Add(-18);
            src.Add(1.0893);
            src.Add(-1.0893);
            src.Add(true);
            src.Add(false);
            src.Add(System.DateTime.Now);
            src.Add(new Base64Data(new byte[ ] {0,1,2,3}));
            src.Add(new Struct());
            src.Add(new X.Array());

            var dest = RoundTrip(src);

            for (int i = 0; i < src.Count; i++)
            {
                var s = src[i];
                var d = dest[i];

                Assert.AreEqual(src.GetType(),dest.GetType());
            }
        }

        [TestMethod]
        public void RoundTrip_Base64()
        {
            var src = new Base64Data(new byte[] {1, 2, 3, 4, 5});
            var dest = RoundTrip(src);

            for (int i = 0; i < src.Bytes.Length; i++)
            {
                Assert.AreEqual(src.Bytes[i],dest.Bytes[i]);
            }
        }

        [TestMethod]
        public void RoundTrip_Int()
        {
            var src = new IntegerValue(7);
            var dest = RoundTrip(src);
            Assert.AreEqual(src.Data,dest.Data);
        }

        [TestMethod]
        public void RoundTrip_Double()
        {
            var src = new DoubleValue(6.02);
            var dest = RoundTrip(src);
            Assert.AreEqual(src.Data, dest.Data);
        }

        [TestMethod]
        public void RoundTrip_Bool_true()
        {
            var src = new BooleanValue(true);
            var dest = RoundTrip(src);
            Assert.AreEqual(src.Data, dest.Data);
        }

        [TestMethod]
        public void RoundTrip_Bool_false()
        {
            var src = new BooleanValue(false);
            var dest = RoundTrip(src);
            Assert.AreEqual(src.Data, dest.Data);
        }

        public T RoundTrip<T>(T src_value) where T : MetaWeblogSharp.XmlRPC.Value
        {
            // Create Source Document
            var src_parent = new System.Xml.Linq.XElement("X");
            var src_value_el = src_value.AddXmlElement(src_parent);

            var desr_value_o = Value.ParseXml(src_value_el);
            var dest_value = (T)desr_value_o;
            return dest_value;
        }
    }
}
