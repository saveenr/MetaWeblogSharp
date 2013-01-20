using System.Collections.Generic;

namespace MetaWeblogSharp.XmlRPC
{
    public class Array : IEnumerable<object>
    {
        private List<object> items;

        public Array()
        {
            this.items = new List<object>();
        }

        public Array(int capacity)
        {
            this.items = new List<object>(capacity);
        }

        public void Add(object o)
        {
            this.items.Add(o);
        }

        public void AddRange(IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                this.items.Add(item);
            }
        }

        public object this[int index]
        {
            get { return this.items[index]; }
        }

        public IEnumerator<object> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}