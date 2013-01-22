using System;
using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class Base64Data
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

        internal void AddToTypeEl(XElement type_el)
        {
            type_el.Add(Convert.ToBase64String(Bytes));
        }

        internal static Base64Data TypeElToValue(XElement type_el)
        {
            var bytes = Convert.FromBase64String(type_el.Value);
            var b = new Base64Data(bytes);
            return b;
        }
    }
}