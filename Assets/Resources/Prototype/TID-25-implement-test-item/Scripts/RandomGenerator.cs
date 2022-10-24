using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
    public class RandomGenerator<T>
    {
        List<T> randomlist = new List<T>();
        

        public void Add(in T item)
        {
            randomlist.Add(item);
        }
        public void Add(in T item, int count)
        {
            for(var i = 0; i<count; ++i)
            {
                randomlist.Add(item);
            }
        }
        public void AddRange(in List<T> items)
        {
            randomlist.AddRange(items);
        }
        
        public T GetItem()
        {
            int i = Random.Range(0, randomlist.Count);
            return randomlist[i];
        }
        public T GetAndRemoveItem()
        {
            int i = Random.Range(0, randomlist.Count);
            T item = randomlist[i];
            randomlist.RemoveAt(i);
            return item;
        }
    }
}

