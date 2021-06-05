using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Iterator
{
    public class Client
    {
        public void UseIterator()
        {
            ConcreteAggregate aggr = new ConcreteAggregate();
            aggr.Add("One");
            aggr.Add("Two");
            aggr.Add("Three");
            aggr.Add("Four");
            aggr.Add("Five");

            Iterator iterator = aggr.CreateIterator();
            while (iterator.Next())
            {
                string item = (string)iterator.Current;
                Console.WriteLine(item);
            }
        }
    }

    public interface Aggregate
    {
        Iterator CreateIterator();
    }

    public class ConcreteAggregate : Aggregate
    {
        private List<object> items = new List<object>();

        public Iterator CreateIterator()
        {
            return new ConcreteIterator(this);
        }

        public object this[int index]
        {
            get { return items[index]; }
        }

        public int Count
        {
            get { return items.Count; }
        }

        public void Add(object o)
        {
            items.Add(o);
        }
    }
    public interface Iterator
    {
        object Current { get; }
        bool Next();

    }

    public class ConcreteIterator : Iterator
    {
        private ConcreteAggregate aggregate;
        int index;

        public ConcreteIterator(ConcreteAggregate aggregate)
        {
            this.aggregate = aggregate;
            index = -1;
        }

        public bool Next()
        {
            index++;
            return index < aggregate.Count;
        }

        public object Current
        {
            get
            {
                if (index < aggregate.Count)
                    return aggregate[index];
                else
                    throw new InvalidOperationException();
            }
        }
    }
}
