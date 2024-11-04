using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class UnitAction
{
    public AbilityData abilityData;
    
    public UnityEvent OnActionComplete = new UnityEvent();

    public bool inProgress = false;
    
    //attempt to run the action at the target position. return false on failure
    public abstract bool RunAction(Vector3Int target);

    public abstract bool PrepAction();

    public abstract bool RushCompletion();
    
    public abstract bool IsFree();
    
    public abstract string GetDescription();

    public static UnitAction GetAction(string actionName)
    {
        actionName = actionName.ToLower();
        switch (actionName)
        {
            case "move":
                return new Move();
        }
        return null;
    }

    public virtual void Initialize(string dataname)
    {
        abilityData = Resources.Load<AbilityData>("AbilityData/"+dataname);
    }
    
}
