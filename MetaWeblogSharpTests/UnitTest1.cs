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
            var svm = new XmlRPC.Value(5.10);
            s["val_double"] = svm;

            var o1 = new XmlRPC.Value(s);

            var parent = new System.Xml.Linq.XElement("X");
            var value_el = o1.AddXmlElement(parent);

            var o2 = XmlRPC.Value.ParseXml(value_el);
            var s2 = (XmlRPC.Struct) o2.Data;

            Assert.IsTrue(s2.ContainsKey("val_double"));

            var z0 = s2.GetItem<double>("val_double", 0.0);
            //Assert.AreEqual(
            //    s2.GetItem<double>("val_double",0.0),
            //    s.GetItem<double>("val_double", 0.0));
        }
    }
}
