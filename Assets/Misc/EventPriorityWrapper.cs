using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventPriorityWrapper<T> : MonoBehaviour
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
        foreach (ActionPriorityWrapper<T> action in _actions)
        {
            action.action.Invoke(value);
        }
    }
}

public class EventPriorityWrapper<T0, T1> : MonoBehaviour
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
        foreach (ActionPriorityWrapper<T0, T1> action in _actions)
        {
            action.action.Invoke(value1, value2);
        }
    }
}
