using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class DateTimeValue : Value
    {
        public System.DateTime Data;

        public DateTimeValue(System.DateTime value)
        {
            this.Data = value;
        }

        public static string TypeString
        {
            get { return "dateTime.iso8601"; }
        }

        protected override void AddToTypeEl(XElement parent)
        {
            var s = this.Data.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            parent.Value = s;
        }

        public static DateTimeValue TypeElToValue(XElement parent)
        {
            System.DateTime dt = System.DateTime.Now;
            if (System.DateTime.TryParse(parent.Value, out dt))
            {
                return new DateTimeValue(dt);
            }
            var x = System.DateTime.ParseExact(parent.Value, "yyyyMMddTHH:mm:ss", null);
            var y = new DateTimeValue(x);
            return y;
        }
    }
}