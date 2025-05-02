using UnityEngine;

public class FSM 
{
    public FSMNode curState;

    public UnitBase unit;

    public FSM(UnitBase myUnit, FSMNode startState)
    {
        unit = myUnit;
        curState = startState;
    }

    public void OnTurnStarted()
    {
        curState.OnTurnStarted(unit);
    }

    public bool transition(string stateName)
    {
        return curState.transition(stateName, unit) != null;
    }
}
