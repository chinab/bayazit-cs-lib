using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Bayazit.Collections
{
    public class BlockingQueue<T>
    {
        Queue<T> que = new Queue<T>();
        Semaphore sem = new Semaphore(0, int.MaxValue);

        public int Count
        {
            get { return que.Count; }
        }

        public void Enqueue(T item)
        {
            lock (que)
            {
                que.Enqueue(item);
            }

            sem.Release();
        }

        public T Dequeue()
        {
            sem.WaitOne();

            lock (que)
            {
                return que.Dequeue();
            }
        }
    }
}
