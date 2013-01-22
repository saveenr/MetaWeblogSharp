using System.Collections.Generic;

namespace MetaWeblogSharp.XmlRPC
{
    public class Array : IEnumerable<Value>
    {
        private List<Value> items;

        public Array()
        {
            this.items = new List<Value>();
        }

        public Array(int capacity)
        {
            this.items = new List<Value>(capacity);
        }

        public void Add(Value v)
        {
            this.items.Add(v);
        }

        public void AddRange(IEnumerable<Value> items)
        {
            foreach (var item in items)
            {
                this.items.Add(item);
            }
        }

        public Value this[int index]
        {
            get { return this.items[index]; }
        }

        public IEnumerator<Value> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static string TypeString
        {
            get { return "array"; }
        }

    }
}