using System;
using System.Linq;
using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public abstract class XValue
    {
        public abstract void AddToTypeEl(XElement type_el);
    }

    public class DateTimeX : XValue
    {
        public System.DateTime Data;
        public DateTimeX(System.DateTime dt)
        {
            this.Data = dt;
        }

        public static string TypeString
        {
            get { return "dateTime.iso8601"; }
        }
        
        public override void AddToTypeEl(XElement type_el)
        {
            var s = this.Data.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            type_el.Value = s;
        }

        public static DateTimeX TypeElToValue(XElement type_el)
        {
            System.DateTime dt = System.DateTime.Now;
            if (System.DateTime.TryParse(type_el.Value, out dt))
            {
                return new DateTimeX(dt);
            }
            var x = System.DateTime.ParseExact(type_el.Value, "yyyyMMddTHH:mm:ss", null);
            var y = new DateTimeX(x);
            return y;
        }
    }

    public class BooleanX : XValue
    {
        public bool Data;
        public BooleanX (bool dt)
        {
            this.Data = dt;
        }

        public static string TypeString
        {
            get { return "boolean"; }
        }

        public override void AddToTypeEl(XElement type_el)
        {
            if (this.Data)
            {
                type_el.Add("1");
            }
            else
            {
                type_el.Add("0");
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

    public class IntegerX : XValue
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

        public override void AddToTypeEl(XElement type_el)
        {
            type_el.Value = this.Data.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static IntegerX TypeElToValue(XElement type_el)
        {
            var bv = new IntegerX(int.Parse(type_el.Value));
            return bv;
        }
    }

    public class DoubleX : XValue
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

        public override void AddToTypeEl(XElement type_el)
        {
            type_el.Value = this.Data.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DoubleX TypeElToValue(XElement type_el)
        {
            var bv = new DoubleX(double.Parse(type_el.Value));
            return bv;
        }
    }

    public class StringX : XValue
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

        public override void AddToTypeEl(XElement type_el)
        {
            type_el.Value = this.Data;
        }

        public static StringX TypeElToValue(XElement type_el)
        {
            var bv = new StringX(type_el.Value);
            return bv;
        }
    }


    public class Value
    {
        public object Data;

        public Value(DateTimeX data)
        {
            this.Data = data;
        }

        public Value(bool data)
        {
            this.Data = new BooleanX(data);
        }

        public Value(BooleanX data)
        {
            this.Data = data;
        }

        public Value(int data)
        {
            this.Data = new IntegerX(data);
        }

        public Value(IntegerX data)
        {
            this.Data = data;
        }
        
        public Value(double data)
        {
            this.Data = new DoubleX(data);
        }

        public Value(DoubleX data)
        {
            this.Data = data;
        }


        public Value(string data)
        {
            this.Data = new StringX(data);
        }

        public Value(StringX data)
        {
            this.Data = data;
        }

        public Value(Base64Data data)
        {
            this.Data = data;
        }

        public Value(Array data)
        {
            this.Data = data;
        }

        public Value(Struct data)
        {
            this.Data = data;
        }

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
                    var array = Array.TypeElToValue(type_el);
                    return new Value(array);
                }
                else if (typename == Struct.TypeString)
                {
                    var struct_ = Struct.TypeElToValue(type_el);
                    return new Value(struct_);
                }
                else if (typename == StringX.TypeString)
                {
                    var sxx = StringX.TypeElToValue(type_el);
                    return new Value(sxx);
                }
                else if (typename == DoubleX.TypeString)
                {
                    var xxx = DoubleX.TypeElToValue(type_el);
                    return new Value(xxx);
                }
                else if (typename == Base64Data.TypeString)
                {
                    var b = Base64Data.TypeElToValue(type_el);
                    return new Value(b);
                }
                else if (typename == DateTimeX.TypeString)
                {
                    var dt = DateTimeX.TypeElToValue(type_el);
                    return new Value(dt);
                }
                else if (typename == IntegerX.TypeString)
                {
                    return new Value(IntegerX.TypeElToValue(type_el));
                }
                else if (typename == BooleanX.TypeString )
                {
                    return new Value( BooleanX.TypeElToValue(type_el));
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
                return new Value(input_value);
            }
        }

        public System.Xml.Linq.XElement AddXmlElement(System.Xml.Linq.XElement parent)
        {
            var value = this;
            var value_el = new System.Xml.Linq.XElement("value");
            object vdata = value.Data;
            var type_el = new System.Xml.Linq.XElement(GetTypeStringFromObject(vdata));
            value_el.Add(type_el);

            if (vdata is StringX)
            {
                var s = (StringX) vdata;
                type_el.Add(s.Data);
            }
            else if (vdata is IntegerX)
            {
                var i = (IntegerX) vdata;
                i.AddToTypeEl(type_el);
            }
            else if (vdata is DoubleX)
            {
                var d = (DoubleX) vdata;
                d.AddToTypeEl(type_el);
            }
            else if (vdata is BooleanX)
            {
                var bv = (BooleanX)vdata;
                bv.AddToTypeEl(type_el);
            }
            else if (vdata is Struct)
            {
                var struct_ = (Struct)vdata;
                struct_.AddToTypeEl(type_el);
            }
            else if (vdata is Base64Data)
            {
                var base64 = (Base64Data)vdata;
                base64.AddToTypeEl(type_el);
            }
            else if (vdata is XmlRPC.Array)
            {
                var array = (XmlRPC.Array)vdata;

                array.AddToTypeEl(type_el);
            }
            else if (vdata is DateTimeX)
            {
                var dt = (DateTimeX)vdata;

                dt.AddToTypeEl(type_el);
            }
            else
            {
                string msg = string.Format("Unknown type {0}", value.GetType().Name);
                throw new ArgumentException(msg);
            }

            parent.Add(value_el);

            return value_el;
        }

        private static string GetTypeStringFromObject(object t)
        {
            if (t is StringX)
            {
                return StringX.TypeString;
            }
            else if (t is IntegerX)
            {
                return IntegerX.TypeString;
            }
            else if (t is XmlRPC.Struct)
            {
                return Struct.TypeString;
            }
            else if (t is XmlRPC.Array)
            {
                return Array.TypeString;
            }
            else if (t is Base64Data)
            {
                return Base64Data.TypeString;
            }
            else if (t is BooleanX)
            {
                return BooleanX.TypeString;
            }
            else if (t is DoubleX)
            {
                return DoubleX.TypeString;
            }
            else if (t is DateTimeX)
            {
                return DateTimeX.TypeString;
            }
            else
            {
                string msg = string.Format("Unsupported type {0}", t.GetType().Name);
                throw new System.ArgumentException(msg);
            }
        }

    }
}