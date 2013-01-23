using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class StringValue : Value
    {
        public string Data;

        public StringValue(string dt)
        {
            this.Data = dt;
        }

        public static string TypeString
        {
            get { return "string"; }
        }

        public override void AddToTypeEl(XElement parent)
        {
            parent.Value = this.Data;
        }

        public static StringValue TypeElToValue(XElement parent)
        {
            var bv = new StringValue(parent.Value);
            return bv;
        }
    }
}