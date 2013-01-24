using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class IntegerValue : Value
    {
        public readonly int Data;
        public IntegerValue(int dt)
        {
            this.Data = dt;
        }

        public static string TypeString
        {
            get { return "int"; }
        }

        protected override void AddToTypeEl(XElement parent)
        {
            parent.Value = this.Data.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static IntegerValue XmlToValue(XElement parent)
        {
            var bv = new IntegerValue(int.Parse(parent.Value));
            return bv;
        }

        public static string AlternateTypeString
        {
            get { return "i4"; }            
        }

        public static implicit operator IntegerValue(int v)
        {
            return new IntegerValue(v);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as IntegerValue;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (this.Data== p.Data);
        }

        public override int GetHashCode()
        {
            return this.Data.GetHashCode();
        }
    }
}