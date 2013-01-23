using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MetaWeblogSharp.XmlRPC
{
    public class Struct : IEnumerable<KeyValuePair<string, Value>>
    {
        private Dictionary<string, Value> dic;

        public Struct()
        {
            this.dic = new Dictionary<string, Value>();
        }

        public StringX GetString(string name, string defval)
        {
            if (this.dic.ContainsKey(name))
            {
                var o_ = this.dic[name];
                var o = o_.Data;
                var vt = o.GetType();
                if (vt != typeof(StringX))
                {
                    string msg = String.Format("Expected type {0} instead got {1}", typeof(StringX).Name, vt.Name);
                    throw new XmlRPCException(msg);
                }
                var v = (StringX)o;
                return v;
            }
            else
            {
                return new StringX(defval);
            }
        }

        public T GetItem<T>(string name, T defval)
        {
            if (this.dic.ContainsKey(name))
            {
                var o_ = this.dic[name];
                var o = o_.Data;
                var vt = o.GetType();
                if (vt != typeof (T))
                {
                    if (typeof (T) == typeof (int) && vt==typeof(string))
                    {
                        // handle the one-off case where someone gave a string when an int was needed
                        o = Int32.Parse((string) o);

                    }
                    else
                    {
                        string msg = String.Format("Expected type {0} instead got {1}", typeof(T).Name, vt.Name);
                        throw new XmlRPCException(msg);                        
                    }
                }
                var v = (T)o;
                return v;
            }
            else
            {
                return defval;
            }
        }

        public T GetItem<T>(string name)
        {
            if (this.dic.ContainsKey(name))
            {
                var o_ = this.dic[name];
                var o = o_.Data;
                var vt = o.GetType();
                if (vt != typeof(T))
                {
                    if (typeof(T) == typeof(int) && vt == typeof(string))
                    {
                        // handle the one-off case where someone gave a string when an int was needed
                        o = Int32.Parse((string)o);

                    }
                    else
                    {
                        string msg = String.Format("Expected type {0} instead got {1}", typeof(T).Name, vt.Name);
                        throw new XmlRPCException(msg);
                    }
                }
                var v = (T)o;
                return v;
            }
            else
            {
                throw new XmlRPCException("Struct did not contain key");
            }
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

        public void AddToTypeEl(XElement type_el)
        {
            foreach (var pair in this)
            {
                var member_el = new XElement("member");
                type_el.Add(member_el);

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