using System.Collections;
using System.Collections.Generic;

namespace MetaWeblogSharp.XmlRPC
{
    public class ParameterList: IEnumerable<Value>
    {
        private List<Value> Parameters;
        
        public ParameterList()
        {
            this.Parameters = new List<Value>();
        }

        public void Add(Value value)
        {
            this.Parameters.Add(value);
        }

        public void Add(int value)
        {
            this.Parameters.Add(new IntegerValue(value));
        }

        public void Add(bool value)
        {
            this.Parameters.Add(new BooleanValue(value));
        }

        public void Add(System.DateTime value)
        {
            this.Parameters.Add(new DateTimeValue(value));
        }

        public void Add(double value)
        {
            this.Parameters.Add(new DoubleValue(value));
        }

        public void Add(Array value)
        {
            this.Parameters.Add(value);
        }

        public void Add(Struct value)
        {
            this.Parameters.Add(value);
        }

        public void Add(Base64Data value)
        {
            this.Parameters.Add(value);
        }

        public void Add(string value)
        {
            this.Parameters.Add(new StringValue(value));
        }

        public IEnumerator<Value> GetEnumerator()
        {
            return this.Parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Value this[int index]
        {
            get { return this.Parameters[index]; }
        }
    }
}