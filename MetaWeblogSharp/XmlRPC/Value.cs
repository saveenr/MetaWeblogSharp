using System;
using System.Linq;

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

        public Value(string data)
        {
            this.Data = data;
        }

        public Value(BinaryData data)
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
                if (typename == "array")
                {
                    var data_el = type_el.Element("data");

                    var value_els = data_el.Elements("value").ToList();
                    var list = new XmlRPC.Array();
                    foreach (var value_el2 in value_els)
                    {
                        var o = ParseXml(value_el2);
                        list.Add(o);
                    }
                    return new Value(list);
                }
                else if (typename == "struct")
                {
                    var member_els = type_el.Elements("member").ToList();
                    var dic = new Struct();
                    foreach (var member_el in member_els)
                    {
                        var name_el = member_el.Element("name");
                        string name = name_el.Value;

                        var value_el2 = member_el.Element("value");
                        var o = ParseXml(value_el2);

                        dic[name] = o;
                    }
                    return new Value(dic);
                }
                else if (typename == "string")
                {
                    return new Value(input_value);
                }
                else if (typename == "double")
                {
                    return new Value(input_value);
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
            var type_el = new System.Xml.Linq.XElement(type_to_name(value.Data.GetType()));
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
                foreach (var pair in struct_)
                {

                    var member_el = new System.Xml.Linq.XElement("member");
                    type_el.Add(member_el);

                    var name_el = new System.Xml.Linq.XElement("name");
                    member_el.Add(name_el);
                    name_el.Value = pair.Key;

                    pair.Value.AddXmlElement(member_el);

                }
            }
            else if (value.Data is BinaryData)
            {
                var bytes = (BinaryData)value.Data;
                string s = System.Convert.ToBase64String(bytes.Bytes);
                type_el.Add(s);
            }
            else if (value.Data is XmlRPC.Array)
            {
                var data_el = new System.Xml.Linq.XElement("data");
                type_el.Add(data_el);
                var list = (XmlRPC.Array)value.Data;
                foreach (XmlRPC.Value item in list)
                {
                    item.AddXmlElement(data_el);
                }
            }
            else
            {
                string msg = string.Format("Unknown type {0}", value.GetType().Name);
                throw new ArgumentException(msg);
            }

            parent.Add(value_el);

            return value_el;
        }

        private static string type_to_name(System.Type t)
        {
            if (t == typeof(string))
            {
                return "string";
            }
            else if (t == typeof(int))
            {
                return "int";
            }
            else if (t == typeof(XmlRPC.Struct))
            {
                return "struct";
            }
            else if (t == typeof(XmlRPC.Array))
            {
                return "array";
            }
            else if (t == typeof(BinaryData))
            {
                return "base64";
            }
            else if (t == typeof(bool))
            {
                return "boolean";
            }
            else if (t == typeof(double))
            {
                return "double";
            }
            else
            {
                string msg = string.Format("Unsupported type {0}", t.Name);
                throw new System.ArgumentException(msg);
            }
        }

    }
}