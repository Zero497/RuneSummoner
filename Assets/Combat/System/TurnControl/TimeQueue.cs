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
