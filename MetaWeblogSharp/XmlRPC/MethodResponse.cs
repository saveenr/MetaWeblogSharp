using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class MethodResponse
    {
        public string RawData { get; private set; }
        public List<Value> Parameters { get; private set; }

        private Fault parsefault(System.Xml.Linq.XElement fault_el)
        {
            var value_el = fault_el.Element("value");
            var fault_value = (Struct)XmlRPC.Value.ParseXml(value_el);
            int fault_code = fault_value.Get<IntegerValue>("faultCode").Data;
            string fault_string = fault_value.Get<StringValue>("faultString").Data;

            var f = new Fault();
            f.FaultCode = fault_code;
            f.FaultString = fault_string;
            f.RawData = fault_el.Document.ToString();
            return f;
        }

        public MethodResponse(string content)
        {
            this.Parameters = new List<Value>();

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

                var val = XmlRPC.Value.ParseXml(value_el);
                this.Parameters.Add( val );
            }
        }
    }
}