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

        public void Add(object o)
        {
            this.items.Add(new Value(o));
        }

        public void AddRange(IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                this.items.Add(new Value(item));
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
    }
}