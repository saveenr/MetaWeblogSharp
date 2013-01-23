using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class BooleanValue : Value
    {
        public bool Data;
        
        public BooleanValue (bool value)
        {
            this.Data = value;
        }

        public static string TypeString
        {
            get { return "boolean"; }
        }

        protected override void AddToTypeEl(XElement parent)
        {
            if (this.Data)
            {
                parent.Add("1");
            }
            else
            {
                parent.Add("0");
            }
        }

        public static BooleanValue TypeElToValue(XElement type_el)
        {
            var i = int.Parse(type_el.Value);
            var b = (i != 0);
            var bv = new BooleanValue(b);
            return bv;
        }
    }
}