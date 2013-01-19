using System.Collections.Generic;

namespace MetaWeblogSharp.XmlRPC
{
    public class Struct: Dictionary<string,object>
    {
        public int GetInt(string name)
        {
            if (this.ContainsKey(name))
            {
                return (int)this[name];
            }
            else
            {
                return 0;
            }
        }

        public string GetString(string name)
        {
            if (this.ContainsKey(name))
            {
                return (string)this[name];
            }
            else
            {
                return null;
            }
        }
    }
}