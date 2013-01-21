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
        public void TestMethod2()
        {
            var s = new XmlRPC.Struct();
            s["val_double"] = new XmlRPC.Value(5);

            var o1 = new XmlRPC.Value(s);

            var parent = new System.Xml.Linq.XElement("X");
            var value_el = o1.AddXmlElement(parent);

            var o2 = XmlRPC.Value.ParseXml(value_el);
            var s2 = (XmlRPC.Struct) o2.Data;

            Assert.IsTrue(s2.ContainsKey("val_double"));
        }
    }
}
