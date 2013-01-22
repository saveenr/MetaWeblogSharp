using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static string TypeString
        {
            get { return "array"; }
        }

        internal void AddToTypeEl(XElement type_el)
        {
            var data_el = new XElement("data");
            type_el.Add(data_el);
            foreach (Value item in this)
            {
                item.AddXmlElement(data_el);
            }
        }

        internal static Array TypeElToValue(XElement type_el)
        {
            var data_el = type_el.Element("data");

            var value_els = data_el.Elements("value").ToList();
            var list = new XmlRPC.Array();
            foreach (var value_el2 in value_els)
            {
                var o = Value.ParseXml(value_el2);
                list.Add(o);
            }
            return list;
        }
    }
}