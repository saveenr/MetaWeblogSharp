using System;
using System.Linq;
using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
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
                else if (typename == StringValue.TypeString)
                {
                    return StringValue.TypeElToValue(type_el);
                }
                else if (typename == DoubleValue.TypeString)
                {
                    return DoubleValue.TypeElToValue(type_el);
                }
                else if (typename == Base64Data.TypeString)
                {
                    return Base64Data.TypeElToValue(type_el);
                }
                else if (typename == DateTimeValue.TypeString)
                {
                    return DateTimeValue.TypeElToValue(type_el);
                }
                else if (typename == IntegerValue.TypeString || typename == IntegerValue.AlternateTypeString)
                {
                    return IntegerValue.TypeElToValue(type_el);
                }
                else if (typename == BooleanValue.TypeString )
                {
                    return BooleanValue.TypeElToValue(type_el);
                }
                else
                {
                    string msg = string.Format("Unsupported type: {0}", typename);
                    throw new XmlRPCException(msg);
                }
            }
            else
            {
                // no <type> element provided. Treat the content as a string
                return new StringValue(input_value);
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
            if (this is StringValue)
            {
                return StringValue.TypeString;
            }
            else if (this is IntegerValue)
            {
                return IntegerValue.TypeString;
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
            else if (this is BooleanValue)
            {
                return BooleanValue.TypeString;
            }
            else if (this is DoubleValue)
            {
                return DoubleValue.TypeString;
            }
            else if (this is DateTimeValue)
            {
                return DateTimeValue.TypeString;
            }
            else
            {
                string msg = string.Format("Unsupported type {0}", GetType().Name);
                throw new System.ArgumentException(msg);
            }
        }

    }
}