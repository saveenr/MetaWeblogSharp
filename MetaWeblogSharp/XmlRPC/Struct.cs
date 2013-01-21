﻿using System.Collections.Generic;

namespace MetaWeblogSharp.XmlRPC
{
    public class Struct : IEnumerable<KeyValuePair<string, object>>
    {
        private Dictionary<string, object> dic;

        public Struct()
        {
            this.dic = new Dictionary<string, object>();
        }

        public T GetItem<T>(string name, T defval)
        {
            if (this.dic.ContainsKey(name))
            {
                var o = this.dic[name];
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

        public object this[string index]
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

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return dic.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}