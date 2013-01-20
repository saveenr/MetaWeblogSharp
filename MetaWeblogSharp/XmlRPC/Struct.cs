using System.Collections.Generic;

namespace MetaWeblogSharp.XmlRPC
{
    public class Struct
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
                    string msg = string.Format("Expected type {0} instead got {1}", typeof (T).Name, vt.Name);
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

        public IEnumerable<KeyValuePair<string, object>>  Members
        {
           get {
               foreach (var o in this.dic)
               {
                   yield return o;
               }
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
    }
}