using System.Collections.Generic;

namespace MetaWeblogSharp.XmlRPC
{
    public class Struct: Dictionary<string,object>
    {
        public T GetItem<T>(string name, T defval)
        {
            if (this.ContainsKey(name))
            {
                var o = this[name];
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

    }
}