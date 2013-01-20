using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class MethodCall
    {
        private List<object> Parameters;
        public string Name { get; private set; }

        public MethodCall(string name)
        {
            this.Name = name;
            this.Parameters = new List<object>();
        }

        public void AddParameter(bool value)
        {
            this.AddParameterX(value);
        }

        public void AddParameter(int value)
        {
            this.AddParameterX(value);
        }

        public void AddParameter(string value)
        {
            this.AddParameterX(value);
        }

        public void AddParameter(byte [] bytes)
        {
            this.AddParameterX(bytes);
        }

        public void AddParameterX(object value)
        {
            this.Parameters.Add(value);
        }

        public void AddParameter(Struct dic)
        {
            this.Parameters.Add(dic);            
        }

        public System.Xml.Linq.XDocument CreateDocument()
        {
            var doc = new System.Xml.Linq.XDocument();
            var root = new System.Xml.Linq.XElement("methodCall");

            doc.Add(root);

            var method = new System.Xml.Linq.XElement("methodName");
            root.Add(method);

            method.Add(this.Name);

            var params_el = new System.Xml.Linq.XElement("params");
            root.Add(params_el);

            foreach (var p in this.Parameters)
            {
                var param_el = new System.Xml.Linq.XElement("param");
                params_el.Add(param_el);

                AddValueEl(param_el,p);
            }

            return doc;
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
            else if (t == typeof(byte []))
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

        private static void AddValueEl(XElement parent, object value)
        {
            var value_el = new System.Xml.Linq.XElement("value");
            var type_el = new System.Xml.Linq.XElement(type_to_name(value.GetType()));
            value_el.Add(type_el);
            
            if (value is string)
            {
                type_el.Add((string) value);
            }
            else if (value is int)
            {
                type_el.Add(value.ToString());
            }
            else if (value is bool)
            {
                var bv = (bool) value;
                if (bv)
                {
                    type_el.Add("1");                   
                }
                else
                {
                    type_el.Add("0");                                       
                }
            }
            else if (value is Struct)
            {
                var struct_ = (Struct)value;
                foreach (var pair in struct_)
                {

                    var member_el = new System.Xml.Linq.XElement("member");
                    type_el.Add(member_el);

                    var name_el = new System.Xml.Linq.XElement("name");
                    member_el.Add(name_el);
                    name_el.Value = pair.Key;

                    AddValueEl(member_el,pair.Value);

                }
            }
            else if (value is byte[])
            {
                var bytes = (byte[]) value;
                string s = System.Convert.ToBase64String(bytes);
                type_el.Add(s);
            }
            else if (value is XmlRPC.Array)
            {
                var data_el = new System.Xml.Linq.XElement("data");
                type_el.Add(data_el);
                var list = (XmlRPC.Array) value;
                foreach (var item in list)
                {
                    AddValueEl(data_el,item);
                }
            }
            else
            {
                string msg = string.Format("Unknown type {0}", value.GetType().Name);
                throw new WarningException(msg);
            }

            parent.Add(value_el);
        }
    }
}