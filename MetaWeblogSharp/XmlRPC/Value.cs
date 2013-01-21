using System;

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

        public Value(byte [] data)
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

        public void AddXmlElement(System.Xml.Linq.XElement parent)
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
            else if (value.Data is byte[])
            {
                var bytes = (byte[])value.Data;
                string s = System.Convert.ToBase64String(bytes);
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
            else if (t == typeof(byte[]))
            {
                return "base64";
            }
            else if (t == typeof(bool))
            {
                return "boolean";
            }
            else
            {
                string msg = string.Format("Unsupported type {0}", t.Name);
                throw new System.ArgumentException(msg);
            }
        }

    }
}