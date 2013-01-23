using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class IntegerValue : Value
    {
        public int Data;
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

        public static IntegerValue TypeElToValue(XElement parent)
        {
            var bv = new IntegerValue(int.Parse(parent.Value));
            return bv;
        }

        public static string AlternateTypeString
        {
            get { return "i4"; }            
        }
    }
}