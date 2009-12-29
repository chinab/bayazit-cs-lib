using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bayazit.Collections
{
    class PriorityQueue<TItem, TPriority> where TPriority : IComparable
    {
        private SortedList<TPriority, Queue<TItem>> pq = new SortedList<TPriority, Queue<TItem>>();
        public int Count { get; private set; }
        public bool Empty
        {
            get { return Count == 0; }
        }

        public void Enqueue(TItem item, TPriority priority)
        {
            ++Count;
            if (!pq.ContainsKey(priority)) pq[priority] = new Queue<TItem>();
            pq[priority].Enqueue(item);
        }

        public TItem Dequeue()
        {
            --Count;
            var queue = pq.Last().Value;
            if (queue.Count == 1) pq.Remove(pq.Last().Key);
            return queue.Dequeue();
        }
    }

    class PriorityQueue<TItem> : PriorityQueue<TItem, int> { }
}
