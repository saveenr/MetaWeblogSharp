using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class MethodResponse
    {
        public string RawData { get; private set; }
        public List<object> Parameters { get; private set; }

        private Fault parsefault(System.Xml.Linq.XElement fault_el)
        {
            var value_el = fault_el.Element("value");

            var fault_value = (Struct)XmlToValue(value_el);
            int fault_code = fault_value.GetItem<int>("faultCode",-1);
            string fault_string = fault_value.GetItem<string>("faultString",null);

            var f = new Fault();
            f.FaultCode = fault_code;
            f.FaultString = fault_string;
            f.RawData = fault_el.Document.ToString();
            return f;
        }

        public MethodResponse(string content)
        {
            this.Parameters = new List<object>();

            this.RawData = content;

            var doc = System.Xml.Linq.XDocument.Parse(this.RawData);
            var root = doc.Root;
            var fault_el = root.Element("fault");
            if (fault_el != null)
            {
                var f = parsefault(fault_el);

                string msg = string.Format("XMLRPC FAULT [{0}]: \"{1}\"", f.FaultCode, f.FaultString);
                var exc = new XmlRPCException(msg);
                exc.Fault = f;

                throw exc;
            }

            var params_el = root.Element("params");
            var param_els = params_el.Elements("param").ToList();

            foreach (var param_el in param_els)
            {
                var value_el = param_el.Element("value");

                var val = XmlToValue(value_el);
                this.Parameters.Add(val);
            }
        }

        private object XmlToValue(System.Xml.Linq.XElement value_el)
        {
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
                        var o = XmlToValue(value_el2);
                        list.Add(o);
                    }
                    return list;
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
                        var o = XmlToValue(value_el2);

                        dic[name] = o;
                    }
                    return dic;
                }
                else if (typename == "string")
                {
                    return input_value;
                }
                else if (typename == "dateTime.iso8601")
                {

                    System.DateTime dt = System.DateTime.Now;
                    if (System.DateTime.TryParse(input_value, out dt))
                    {
                        return dt;
                    }
                    return System.DateTime.ParseExact(input_value,"yyyyMMddTHH:mm:ss",null);
                }
                else if (typename == "int" | typename == "i4")
                {
                    return int.Parse(input_value);
                }
                else if (typename == "boolean")
                {
                    var i = int.Parse(input_value);
                    var b = (i != 0);
                    return b;
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
                return input_value;
            }
        }
    }
}