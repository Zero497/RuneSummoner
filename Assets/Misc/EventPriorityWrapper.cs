using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EventPriorityWrapper<T>
{
    private List<ActionPriorityWrapper<T>> _actions = new List<ActionPriorityWrapper<T>>();
    
    public void Subscribe(ActionPriorityWrapper<T> callback)
    {
        _actions.Add(callback);
        _actions.Sort();
    }

    public void Unsubscribe(ActionPriorityWrapper<T> callback)
    {
        _actions.Remove(callback);
    }

    public void Invoke(T value)
    {
        var actions = _actions.ToArray();
        foreach (ActionPriorityWrapper<T> action in actions)
        {
            if(action != null && action.action != null)
                action.action.Invoke(value);
        }
    }
}

public class EventPriorityWrapper<T0, T1>
{
    private List<ActionPriorityWrapper<T0, T1>> _actions = new List<ActionPriorityWrapper<T0, T1>>();
    
    public void Subscribe(ActionPriorityWrapper<T0, T1> callback)
    {
        _actions.Add(callback);
        _actions.Sort();
    }

    public void Unsubscribe(ActionPriorityWrapper<T0, T1> callback)
    {
        _actions.Remove(callback);
    }

    public void Invoke(T0 value1, T1 value2)
    {
        var actions = _actions.ToArray();
        foreach (ActionPriorityWrapper<T0, T1> action in actions)
        {
            if(action != null && action.action != null)
                action.action.Invoke(value1, value2);
        }
    }
}

public class EventPriorityWrapper<T0, T1, T2>
{
    private List<ActionPriorityWrapper<T0, T1, T2>> _actions = new List<ActionPriorityWrapper<T0, T1, T2>>();
    
    public void Subscribe(ActionPriorityWrapper<T0, T1, T2> callback)
    {
        _actions.Add(callback);
        _actions.Sort();
    }

    public void Unsubscribe(ActionPriorityWrapper<T0, T1, T2> callback)
    {
        _actions.Remove(callback);
    }

    public void Invoke(T0 value1, T1 value2, T2 value3)
    {
        var actions = _actions.ToArray();
        foreach (ActionPriorityWrapper<T0, T1, T2> action in actions)
        {
            if(action != null && action.action != null)
                action.action.Invoke(value1, value2, value3);
        }
    }
}
