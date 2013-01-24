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

        public static BooleanValue XmlToValue(XElement type_el)
        {
            var i = int.Parse(type_el.Value);
            var b = (i != 0);
            var bv = new BooleanValue(b);
            return bv;
        }

        public static implicit operator BooleanValue (bool v)
        {
            return new BooleanValue(v);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as BooleanValue;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (this.Data == p.Data);
        }
    }
}