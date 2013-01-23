using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class Struct : Value, IEnumerable<KeyValuePair<string, Value>>
    {
        private Dictionary<string, Value> dic;

        public Struct()
        {
            this.dic = new Dictionary<string, Value>();
        }

        internal bool TryGetValue(string name, out Value v)
        {
            return this.dic.TryGetValue(name, out v);
        }

        internal Value TryGetValue(string name)
        {
            Value v=null;
            var b = this.dic.TryGetValue(name, out v);
            return v;
        }

        private void checktype<T>(Value v)
        {
            var expected = typeof (T);
            var actual = v.GetType();
            if (expected != actual)
            {
                string msg = String.Format("Expected type {0} instead got {1}", expected.Name, actual.Name);
                throw new XmlRPCException(msg);
            }
        }

        public T TryGet<T>(string name) where T:Value
        {
            var v = this.TryGetValue(name);
            if (v == null)
            {
                return null;
            }

            this.checktype<T>(v);

            return (T)v;
        }

        public T Get<T>(string name, T defval) where T : Value
        {
            var v = this.TryGetValue(name);
            if (v == null)
            {
                return defval;
            }

            this.checktype<T>(v);

            return (T)v;
        }

        public Value Get(string name)
        {
            var v = this.TryGetValue(name);
            if (v == null)
            {
                string msg = String.Format("Struct does not contains {0}", name);
                throw new XmlRPCException(msg);
            }
            return v;
        }

        public T Get<T>(string name) where T : Value
        {
            var v = this.Get(name);
            this.checktype<T>(v);
            return (T)v;
        }

        public Value this[string index]
        {
            //get {  /* return the specified index here */ }
            set
            {
                this.dic[index] = value;
            }
        }
        
        public int Count
        {
            get { return this.dic.Count; }
        }

        public bool ContainsKey(string name)
        {

            return this.dic.ContainsKey(name);
        }

        public IEnumerator<KeyValuePair<string, Value>> GetEnumerator()
        {
            return dic.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static string TypeString
        {
            get { return "struct"; }
        }

        protected override void AddToTypeEl(XElement parent)
        {
            foreach (var pair in this)
            {
                var member_el = new XElement("member");
                parent.Add(member_el);

                var name_el = new XElement("name");
                member_el.Add(name_el);
                name_el.Value = pair.Key;

                pair.Value.AddXmlElement(member_el);
            }
        }

        public static Struct TypeElToValue(XElement type_el)
        {
            var member_els = type_el.Elements("member").ToList();
            var struct_ = new Struct();
            foreach (var member_el in member_els)
            {
                var name_el = member_el.Element("name");
                string name = name_el.Value;

                var value_el2 = member_el.Element("value");
                var o = Value.ParseXml(value_el2);

                struct_[name] = o;
            }
            return struct_;
        }
    }
}