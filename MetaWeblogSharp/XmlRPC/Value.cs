﻿using System;
using System.Linq;
using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class Value
    {
        public object Data;

        public Value(System.DateTime data)
        {
            this.Data = data;
        }

        public Value(bool data)
        {
            this.Data = data;
        }

        public Value(int data)
        {
            this.Data = data;
        }

        public Value(double data)
        {
            this.Data = data;
        }

        public Value(string data)
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
                else if (typename == "string")
                {
                    return new Value(input_value);
                }
                else if (typename == "double")
                {
                    return new Value(double.Parse(input_value));
                }
                else if (typename == Base64Data.TypeString)
                {
                    var b = Base64Data.TypeElToValue(type_el);
                    return new Value(b);
                }
                else if (typename == "dateTime.iso8601")
                {

                    System.DateTime dt = System.DateTime.Now;
                    if (System.DateTime.TryParse(input_value, out dt))
                    {
                        return new Value(dt);
                    }
                    return new Value(System.DateTime.ParseExact(input_value, "yyyyMMddTHH:mm:ss", null));
                }
                else if (typename == "int" || typename == "i4")
                {
                    return new Value(int.Parse(input_value));
                }
                else if (typename == "boolean")
                {
                    var i = int.Parse(input_value);
                    var b = (i != 0);
                    return new Value(b);
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
            var type_el = new System.Xml.Linq.XElement(GetTypeStringFromObject(value.Data));
            value_el.Add(type_el);

            if (value.Data is string)
            {
                type_el.Add((string)value.Data);
            }
            else if (value.Data is int)
            {
                type_el.Add(value.Data.ToString());
            }
            else if (value.Data is double)
            {
                var d = (double) value.Data;
                type_el.Add(d.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else if (value.Data is bool)
            {
                var bv = (bool)value.Data;
                if (bv)
                {
                    type_el.Add("1");
                }
                else
                {
                    type_el.Add("0");
                }
            }
            else if (value.Data is Struct)
            {
                var struct_ = (Struct)value.Data;
                struct_.AddToTypeEl(type_el);
            }
            else if (value.Data is Base64Data)
            {
                var base64 = (Base64Data)value.Data;
                base64.AddToTypeEl(type_el);
            }
            else if (value.Data is XmlRPC.Array)
            {
                var array = (XmlRPC.Array)value.Data;

                array.AddToTypeEl(type_el);
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
            if (t is string)
            {
                return "string";
            }
            else if (t is int)
            {
                return "int";
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
            else if (t is bool)
            {
                return "boolean";
            }
            else if (t is double)
            {
                return "double";
            }
            else
            {
                string msg = string.Format("Unsupported type {0}", t.GetType().Name);
                throw new System.ArgumentException(msg);
            }
        }

    }
}