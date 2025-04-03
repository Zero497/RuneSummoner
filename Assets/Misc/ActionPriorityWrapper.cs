using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionPriorityWrapper<T> : IComparable<ActionPriorityWrapper<T>>
{
    public int priority;
    
    public UnityAction<T> action;
    
    public int CompareTo(ActionPriorityWrapper<T> other)
    {
        if (priority < other.priority) return -1;
        else if(priority > other.priority) return 1;
        return 0;
    }
}

public class ActionPriorityWrapper<T0, T1> : IComparable<ActionPriorityWrapper<T0, T1>>
{
    public int priority;
    
    public UnityAction<T0, T1> action;
    
    public int CompareTo(ActionPriorityWrapper<T0, T1> other)
    {
        if (priority < other.priority) return -1;
        else if(priority > other.priority) return 1;
        return 0;
    }
}

public class ActionPriorityWrapper<T0, T1, T2> : IComparable<ActionPriorityWrapper<T0, T1, T2>>
{
    public int priority;
    
    public UnityAction<T0, T1, T2> action;
    
    public int CompareTo(ActionPriorityWrapper<T0, T1, T2> other)
    {
        if (priority < other.priority) return -1;
        else if(priority > other.priority) return 1;
        return 0;
    }
}