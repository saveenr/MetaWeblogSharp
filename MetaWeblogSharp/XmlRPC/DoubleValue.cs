using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class DoubleValue : Value
    {
        public readonly double Double;

        public DoubleValue(double d)
        {
            this.Double = d;
        }

        public static string TypeString
        {
            get { return "double"; }
        }

        protected override void AddToTypeEl(XElement parent)
        {
            parent.Value = this.Double.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DoubleValue XmlToValue(XElement parent)
        {
            var bv = new DoubleValue(double.Parse(parent.Value));
            return bv;
        }

        public static implicit operator DoubleValue(double v)
        {
            return new DoubleValue(v);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as DoubleValue;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (this.Double == p.Double);
        }

        public override int GetHashCode()
        {
            return this.Double.GetHashCode();
        }
    }
}