using System;
using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class Base64Data : Value
    {
        public byte[] Bytes;

        public Base64Data(byte[] bytes)
        {
            this.Bytes = bytes;
        }

        public static string TypeString
        {
            get { return "base64"; }
        }

        protected override void AddToTypeEl(XElement parent)
        {
            parent.Add(Convert.ToBase64String(Bytes));
        }

        internal static Base64Data TypeElToValue(XElement type_el)
        {
            var bytes = Convert.FromBase64String(type_el.Value);
            var b = new Base64Data(bytes);
            return b;
        }
    }
}