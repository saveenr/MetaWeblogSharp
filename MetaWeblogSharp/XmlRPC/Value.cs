using System;
using System.Linq;
using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{

    public class DateTimeX : Value
    {
        public System.DateTime Data;

        public DateTimeX(System.DateTime value)
        {
            this.Data = value;
        }

        public static string TypeString
        {
            get { return "dateTime.iso8601"; }
        }
        
        public override void AddToTypeEl(XElement parent)
        {
            var s = this.Data.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            parent.Value = s;
        }

        public static DateTimeX TypeElToValue(XElement parent)
        {
            System.DateTime dt = System.DateTime.Now;
            if (System.DateTime.TryParse(parent.Value, out dt))
            {
                return new DateTimeX(dt);
            }
            var x = System.DateTime.ParseExact(parent.Value, "yyyyMMddTHH:mm:ss", null);
            var y = new DateTimeX(x);
            return y;
        }
    }

    public class BooleanX : Value
    {
        public bool Data;
        
        public BooleanX (bool value)
        {
            this.Data = value;
        }

        public static string TypeString
        {
            get { return "boolean"; }
        }

        public override void AddToTypeEl(XElement parent)
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

        public static BooleanX TypeElToValue(XElement type_el)
        {
            var i = int.Parse(type_el.Value);
            var b = (i != 0);
            var bv = new BooleanX(b);
            return bv;
        }
    }

    public class IntegerX : Value
    {
        public int Data;
        public IntegerX(int dt)
        {
            this.Data = dt;
        }

        public static string TypeString
        {
            get { return "int"; }
        }

        public override void AddToTypeEl(XElement parent)
        {
            parent.Value = this.Data.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static IntegerX TypeElToValue(XElement parent)
        {
            var bv = new IntegerX(int.Parse(parent.Value));
            return bv;
        }
    }

    public class DoubleX : Value
    {
        public double Data;
        public DoubleX(double dt)
        {
            this.Data = dt;
        }

        public static string TypeString
        {
            get { return "double"; }
        }

        public override void AddToTypeEl(XElement parent)
        {
            parent.Value = this.Data.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DoubleX TypeElToValue(XElement parent)
        {
            var bv = new DoubleX(double.Parse(parent.Value));
            return bv;
        }
    }

    public class StringX : Value
    {
        public string Data;
        public StringX(string dt)
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

        public static StringX TypeElToValue(XElement parent)
        {
            var bv = new StringX(parent.Value);
            return bv;
        }
    }


    public abstract class Value
    {
        public abstract void AddToTypeEl(XElement parent);

        public static XmlRPC.Value ParseXml(System.Xml.Linq.XElement value_el)
        {
            if (value_el.Name != "value")
            {
                string msg = string.Format("XML Element should have name \"value\" instead found \"{0}\"", value_el.Name);
                throw new XmlRPCException();
            }
            var input_value = value_el.Value;
            if (value_el.HasElements)
            {
                var type_el = value_el.Elements().First();
                string typename = type_el.Name.ToString();
                if (typename == Array.TypeString)
                {
                    return Array.TypeElToValue(type_el);
                }
                else if (typename == Struct.TypeString)
                {
                    return Struct.TypeElToValue(type_el);
                }
                else if (typename == StringX.TypeString)
                {
                    return StringX.TypeElToValue(type_el);
                }
                else if (typename == DoubleX.TypeString)
                {
                    return DoubleX.TypeElToValue(type_el);
                }
                else if (typename == Base64Data.TypeString)
                {
                    return Base64Data.TypeElToValue(type_el);
                }
                else if (typename == DateTimeX.TypeString)
                {
                    return DateTimeX.TypeElToValue(type_el);
                }
                else if (typename == IntegerX.TypeString || typename == "i4")
                {
                    return IntegerX.TypeElToValue(type_el);
                }
                else if (typename == BooleanX.TypeString )
                {
                    return BooleanX.TypeElToValue(type_el);
                }
                else
                {
                    string msg = string.Format("Unsupported type: {0}", typename.ToString());
                    throw new XmlRPCException(msg);
                }
            }
            else
            {
                // no sub elements must be a string
                return new StringX(input_value);
            }
        }

        public System.Xml.Linq.XElement AddXmlElement(System.Xml.Linq.XElement parent)
        {
            var value_el = new System.Xml.Linq.XElement("value");
            var type_el = new System.Xml.Linq.XElement(GetTypeString());
            value_el.Add(type_el);

            this.AddToTypeEl(type_el);

            parent.Add(value_el);

            return value_el;
        }

        private string GetTypeString()
        {
            if (this is StringX)
            {
                return StringX.TypeString;
            }
            else if (this is IntegerX)
            {
                return IntegerX.TypeString;
            }
            else if (this is XmlRPC.Struct)
            {
                return Struct.TypeString;
            }
            else if (this is XmlRPC.Array)
            {
                return Array.TypeString;
            }
            else if (this is Base64Data)
            {
                return Base64Data.TypeString;
            }
            else if (this is BooleanX)
            {
                return BooleanX.TypeString;
            }
            else if (this is DoubleX)
            {
                return DoubleX.TypeString;
            }
            else if (this is DateTimeX)
            {
                return DateTimeX.TypeString;
            }
            else
            {
                string msg = string.Format("Unsupported type {0}", GetType().Name);
                throw new System.ArgumentException(msg);
            }
        }

    }
}