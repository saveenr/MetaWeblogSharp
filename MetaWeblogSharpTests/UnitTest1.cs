using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlRPC = MetaWeblogSharp.XmlRPC;

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
        public void RoundTrip_Double()
        {
            var s = new XmlRPC.Struct();
            var svm = new XmlRPC.Value(5.10);
            s["val_double"] = svm;

            var o1 = new XmlRPC.Value(s);

            var parent = new System.Xml.Linq.XElement("X");
            var value_el = o1.AddXmlElement(parent);

            var o2 = XmlRPC.Value.ParseXml(value_el);
            var s2 = (XmlRPC.Struct) o2.Data;

            Assert.IsTrue(s2.ContainsKey("val_double"));

            var z0 = s2.GetItem<XmlRPC.DoubleX>("val_double");
            Assert.AreEqual(
                s2.GetItem<XmlRPC.DoubleX>("val_double").Data,
                s.GetItem<XmlRPC.DoubleX>("val_double").Data);
        }

        [TestMethod]
        public void RoundTrip_Base64()
        {
            var s = new XmlRPC.Struct();
            var b0 = new XmlRPC.Base64Data(new byte[] {1, 2, 3, 4, 5});
            var svm = new XmlRPC.Value( b0);
            s["val_base64"] = svm;

            var o1 = new XmlRPC.Value(s);

            var parent = new System.Xml.Linq.XElement("X");
            var value_el = o1.AddXmlElement(parent);

            var o2 = XmlRPC.Value.ParseXml(value_el);
            var s2 = (XmlRPC.Struct)o2.Data;

            Assert.IsTrue(s2.ContainsKey("val_base64"));

            var z0 = s2.GetItem<MetaWeblogSharp.XmlRPC.Base64Data>("val_base64");

            for (int i = 0; i < b0.Bytes.Length; i++)
            {
                Assert.AreEqual(b0.Bytes[i],z0.Bytes[i]);
            }
        }

        [TestMethod]
        public void RoundTrip_DateTime()
        {
            var input_struct = new XmlRPC.Struct();
            var input_data = new XmlRPC.DateTimeX(System.DateTime.Now);
            var input_value = new XmlRPC.Value(input_data);
            input_struct["x"] = input_value;

            var o1 = new XmlRPC.Value(input_struct);

            var parent = new System.Xml.Linq.XElement("X");
            var value_el = o1.AddXmlElement(parent);

            var o2 = XmlRPC.Value.ParseXml(value_el);
            var output_struct = (XmlRPC.Struct)o2.Data;

            Assert.IsTrue(output_struct.ContainsKey("x"));

            var output_data = output_struct.GetItem<MetaWeblogSharp.XmlRPC.DateTimeX>("x");

        }
    }
}
