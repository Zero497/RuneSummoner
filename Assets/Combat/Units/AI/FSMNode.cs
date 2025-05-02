using System.Collections.Generic;
using UnityEngine;

public abstract class FSMNode : ScriptableObject
{
    public List<FSMNode> transitionStates;

    public abstract void OnEnter(UnitBase unit);

    public abstract void OnExit(UnitBase unit);

    public abstract void OnTurnStarted(UnitBase unit);

    public FSMNode transition(string stateName, UnitBase unit)
    {
        OnExit(unit);
        foreach (FSMNode node in transitionStates)
        {
            if (node.name.Equals(stateName))
            {
                node.OnEnter(unit);
                return node;
            }
        }
        return null;
    }

    public FSMNode transition(int index, UnitBase unit)
    {
        if (index > transitionStates.Count)
        {
            return null;
        }
        OnExit(unit);
        transitionStates[index].OnEnter(unit);
        return transitionStates[index];
    }
}
