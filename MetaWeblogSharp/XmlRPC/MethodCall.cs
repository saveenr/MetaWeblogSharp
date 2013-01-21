using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class MethodCall
    {
        private List<Value> Parameters;
        public string Name { get; private set; }

        public MethodCall(string name)
        {
            this.Name = name;
            this.Parameters = new List<Value>();
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

        private void AddParameterX(object value)
        {
            var p = new Value(value);
            this.Parameters.Add(p);
        }

        public void AddParameter(Struct struct_)
        {
            this.AddParameterX(struct_);
        }

        public void AddParameter(Array array)
        {
            this.AddParameterX(array);
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

                p.AddXmlElement(param_el);
            }

            return doc;
        }

    }
}