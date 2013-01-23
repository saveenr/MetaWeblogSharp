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

        protected override void AddToTypeEl(XElement parent)
        {
            parent.Value = this.Data;
        }

        public static StringValue TypeElToValue(XElement parent)
        {
            var bv = new StringValue(parent.Value);
            return bv;
        }

        public static implicit operator StringValue(string v)
        {
            return new StringValue( v);
        }

        private static StringValue ns;
        private static StringValue es;

        public static StringValue NullString
        {
            get
            {
                if (StringValue.ns == null)
                {
                    ns = new StringValue(null);
                }
                return ns;
            }
        }

        public static StringValue EmptyString
        {
            get
            {
                if (StringValue.es == null)
                {
                    es = new StringValue(string.Empty);
                }
                return es;
            }
        }
    }
}