using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class UnitAction : IEquatable<UnitAction>
{
    public AbilityData abilityData;
    
    public EventPriorityWrapper<UnitAction> OnActionComplete = new EventPriorityWrapper<UnitAction>();

    public bool inProgress = false;

    public bool prepped = false;

    public string id => GetID();

    public UnitBase source;
    
    //attempt to run the action at the target position. return false on failure
    public abstract bool RunAction(SendData actionData);

    public abstract bool PrepAction();

    public abstract bool RushCompletion();

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

    public virtual float GetRange(bool getBase = false)
    {
        return abilityData.range;
    }

    public virtual float GetAOERange(bool getBase = false)
    {
        return abilityData.aoeRange;
    }

    public virtual Float GetStaminaCost(bool getBase = false)
    {
        return new Float(abilityData.staminaCost);
    }
    
    public virtual Float GetManaCost(bool getBase = false)
    {
        return new Float(abilityData.manaCost);
    }

    /*
     Expects:
        Unit 0: unit to apply to
    */
    public virtual void Initialize(SendData sendData)
    {
        source = sendData.unitData[0];
        abilityData = Resources.Load<AbilityData>("AbilityData/"+GetID());
    }

    public bool Equals(UnitAction other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return id.Equals(other.id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(abilityData, OnActionComplete, inProgress, prepped, id, source);
    }

    public abstract string GetID();
}
