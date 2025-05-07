using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeQueue<T>
{
    public List<TimeNode<T>> queue = new List<TimeNode<T>>();

    public void Add(T thing, float timeToNext, int priority = 64)
    {
        queue.Add(new TimeNode<T>(thing, timeToNext, priority));
        queue.Sort();
    }

    public void Remove(T thing)
    {
        foreach (TimeNode<T> node in queue)
        {
            if (node.value.Equals(thing))
            {
                queue.Remove(node);
                return;
            }
        }
    }
    
    public (T, float) AdvanceAndPop()
    {
        TimeNode<T> rem = queue[0];
        for (int i = 1; i < queue.Count; i++)
        {
            queue[i].time -= rem.time;
        }
        queue.RemoveAt(0);
        return (rem.value, rem.time);
    }

    
}
