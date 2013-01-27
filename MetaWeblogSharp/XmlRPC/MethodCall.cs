using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{

    public class ParameterList: IEnumerable<Value>
    {
        private List<Value> Parameters;
        
        public ParameterList()
        {
            this.Parameters = new List<Value>();
        }

        public void AddParameter(Value value)
        {
            this.Parameters.Add(value);
        }

        public void AddParameter(int value)
        {
            this.Parameters.Add(new IntegerValue(value));
        }

        public void AddParameter(bool value)
        {
            this.Parameters.Add(new BooleanValue(value));
        }

        public void AddParameter(System.DateTime value)
        {
            this.Parameters.Add(new DateTimeValue(value));
        }

        public void AddParameter(double value)
        {
            this.Parameters.Add(new DoubleValue(value));
        }

        public void AddParameter(Array value)
        {
            this.Parameters.Add(value);
        }

        public void AddParameter(Struct value)
        {
            this.Parameters.Add(value);
        }

        public void AddParameter(Base64Data value)
        {
            this.Parameters.Add(value);
        }

        public void AddParameter(string value)
        {
            this.Parameters.Add(new StringValue(value));
        }

        public IEnumerator<Value> GetEnumerator()
        {
            return this.Parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Value this[int index]
        {
            get { return this.Parameters[index]; }
        }
    }
    public class MethodCall
    {
        public ParameterList Parameters { get; private set; }
        public string Name { get; private set; }

        public MethodCall(string name)
        {
            this.Name = name;
            this.Parameters = new ParameterList();
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