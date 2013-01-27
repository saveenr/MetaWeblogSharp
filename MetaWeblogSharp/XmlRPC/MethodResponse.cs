using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class MethodResponse
    {
        public ParameterList Parameters { get; private set; }
        
        public MethodResponse(string content)
        {
            this.Parameters = new ParameterList();

            var doc = System.Xml.Linq.XDocument.Parse(content);
            var root = doc.Root;
            var fault_el = root.Element("fault");
            if (fault_el != null)
            {
                var f = Fault.ParseXml(fault_el);

                string msg = string.Format("XMLRPC FAULT [{0}]: \"{1}\"", f.FaultCode, f.FaultString);
                var exc = new XmlRPCException(msg);
                exc.Fault = f;

                throw exc;
            }

            var params_el = root.GetElement("params");
            var param_els = params_el.Elements("param").ToList();

            foreach (var param_el in param_els)
            {
                var value_el = param_el.GetElement("value");

                var val = XmlRPC.Value.ParseXml(value_el);
                this.Parameters.AddParameter( val );
            }
        }
    }
}