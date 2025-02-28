using System;
using UnityEngine;

public class TimeNode<T> : IComparable<TimeNode<T>>
{
    public T value;

    public float time;

    public int priority;
    
    public TimeNode(T value, float time, int priority = 64)
    {
        this.value = value;
        this.time = time;
        this.priority = priority;
    }

    public int CompareTo(TimeNode<T> other)
    {
        if (other.time > time) return -1;
        if (other.time < time) return 1;
        if (other.priority > priority) return 1;
        if (other.priority < priority) return -1;
        return 0;
    }
}
