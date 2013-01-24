using System;
using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class Base64Data : Value
    {
        public readonly byte[] Bytes;

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

        internal static Base64Data XmlToValue(XElement type_el)
        {
            var bytes = Convert.FromBase64String(type_el.Value);
            var b = new Base64Data(bytes);
            return b;
        }

        public static implicit operator Base64Data(byte [] v)
        {
            return new Base64Data(v);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as Base64Data;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match:
            if (this.Bytes != p.Bytes)
            {
                if (this.Bytes.Length != p.Bytes.Length)
                {
                    return false;
                }

                for (int i = 0; i < this.Bytes.Length; i++)
                {
                    if (this.Bytes[i] != p.Bytes[i])
                    {
                        return false;
                    }
                } 
                
                return true;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this.Bytes.GetHashCode();
        }
    }
}