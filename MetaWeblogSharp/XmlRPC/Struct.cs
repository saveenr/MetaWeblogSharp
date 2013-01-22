using System.Collections.Generic;
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
                        o = int.Parse((string) o);

                    }
                    else
                    {
                        string msg = string.Format("Expected type {0} instead got {1}", typeof(T).Name, vt.Name);
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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static string TypeString
        {
            get { return "struct"; }
        }

        internal void AddToTypeEl(XElement type_el)
        {
            foreach (var pair in this)
            {
                var member_el = new System.Xml.Linq.XElement("member");
                type_el.Add(member_el);

                var name_el = new System.Xml.Linq.XElement("name");
                member_el.Add(name_el);
                name_el.Value = pair.Key;

                pair.Value.AddXmlElement(member_el);
            }
        }
    }
}