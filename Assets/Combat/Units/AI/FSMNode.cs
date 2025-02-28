using System.Collections.Generic;
using UnityEngine;

public abstract class FSMNode : ScriptableObject
{
    public List<FSMNode> transitionStates;
    
    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnTurnStarted();

    public FSMNode transition(string stateName)
    {
        OnExit();
        foreach (FSMNode node in transitionStates)
        {
            if (node.name.Equals(stateName))
            {
                node.OnEnter();
                return node;
            }
        }
        return null;
    }

    public FSMNode transition(int index)
    {
        if (index > transitionStates.Count)
        {
            return null;
        }
        OnExit();
        transitionStates[index].OnEnter();
        return transitionStates[index];
    }
}
