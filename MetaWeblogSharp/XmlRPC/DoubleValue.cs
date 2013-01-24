using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class DoubleValue : Value
    {
        public double Data;
        public DoubleValue(double dt)
        {
            this.Data = dt;
        }

        public static string TypeString
        {
            get { return "double"; }
        }

        protected override void AddToTypeEl(XElement parent)
        {
            parent.Value = this.Data.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DoubleValue TypeElToValue(XElement parent)
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
            return (this.Data == p.Data);
        }
    }
}