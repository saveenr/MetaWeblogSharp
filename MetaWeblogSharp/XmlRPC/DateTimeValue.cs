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

        public static DateTimeValue XmlToValue(XElement parent)
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

        public static implicit operator DateTimeValue(System.DateTime v)
        {
            return new DateTimeValue(v);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as DateTimeValue;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (this.Data.Day == p.Data.Day && this.Data.Month == p.Data.Month && this.Data.Year == p.Data.Year) &&
                (this.Data.Hour == p.Data.Hour&& this.Data.Minute== p.Data.Minute&& this.Data.Second== p.Data.Second);
        }
    }
}