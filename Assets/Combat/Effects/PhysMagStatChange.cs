using UnityEngine;

public class PhysMagStatChange : Effect
{
    private string statToModify;

    private float mod = 1;

    private ActionPriorityWrapper<Effect, int> myAction;
    
    /*
        Expects:
            Unit 0: unit to apply to
            String 0: effect name
            String 1: stat name to modify
            Float 0: stacks to apply
            Float 1: -1 to mod down or 1 to mod up
     */
    public override void Initialize(SendData data)
    {
        stackDecayAmount = 1;
        stackDecayTime = 25;
        base.Initialize(data);
        statToModify = data.strData[1];
        mod = data.floatData[1];
        OnStacksChanged(stacks);
        myAction = new ActionPriorityWrapper<Effect, int>();
        myAction.priority = 4;
        myAction.action = OnStacksChanged;
        onStackAddedOrRemoved.Subscribe(myAction);
    }

    public string GetStatToMod()
    {
        return statToModify;
    }

    public float GetMod()
    {
        return mod;
    }

    public override void RemoveEffect()
    {
        onStackAddedOrRemoved.Unsubscribe(myAction);
        OnStacksChanged(-stacks);
        base.RemoveEffect();
    }

    /*
        Expects:
            String 0: effect name
            String 1: stat name to modify
     */
    public override bool MatchForMerge(SendData data)
    {
        if (data.strData[0].Equals(effectName))
        {
            if (data.strData[0].Equals(statToModify))
            {
                return true;
            }
        }

        return false;
    }

    public override bool MatchForMerge(Effect effect)
    {
        if (effect.effectName.Equals(effectName))
        {
            PhysMagStatChange effectA = (PhysMagStatChange)effect;
            if (effectA.GetStatToMod().Equals(statToModify))
                return true;
        }

        return false;
    }

    /*
        Expects:
            String 0: effect name
            String 1: stat name to modify
            Float 0: stacks to merge
            Float 1: -1 to mod down or 1 to mod up
     */
    public override bool MergeEffects(SendData data)
    {
        if (MatchForMerge(data))
        {
            if (Mathf.FloorToInt(mod) == Mathf.FloorToInt(data.floatData[1]))
            {
                AddStacks((int)data.floatData[0]);
            }
            else
            {
                AddStacks(-(int)data.floatData[0]);
            }
            if(stacks == 0)
                RemoveEffect();
            else if (stacks < 0)
                mod *= -1;
            return true;
        }

        return false;
    }
    
    public override bool MergeEffects(Effect effect)
    {
        if (MatchForMerge(effect))
        {
            PhysMagStatChange effectA = (PhysMagStatChange)effect;
            if (Mathf.FloorToInt(mod) == Mathf.FloorToInt(effectA.GetMod()))
            {
                AddStacks((int)effect.getStacks());
            }
            else
            {
                AddStacks(-(int)effect.getStacks());
            }
            if(stacks == 0)
                RemoveEffect();
            else if (stacks < 0)
                mod *= -1;
            return true;
        }

        return false;
    }

    public override bool Equals(Effect other)
    {
        if (other == null) return false;
        if (other.effectName.Equals(effectName))
        {
            PhysMagStatChange otherA = (PhysMagStatChange)other;
            return (Mathf.FloorToInt(otherA.GetMod()) == Mathf.FloorToInt(mod)) &&
                   otherA.GetStatToMod().Equals(statToModify);
        }
        return false;
    }
    
    /*
        Expects:
            String 0: effect name
            String 1: stat name to modify
            Float 1: mod
     */
    public override bool Equals(SendData data)
    {
        if (base.Equals(data))
        {
            return (Mathf.FloorToInt(data.floatData[1]) == Mathf.FloorToInt(mod)) &&
                   data.strData[1].Equals(statToModify);
        }
        return false;
    }

    private void OnStacksChanged(int changeAmount)
    {
        float baseVal = source.myCombatStats.GetStat(statToModify, true);
        source.myCombatStats.ChangeStat(statToModify,baseVal*0.05f*changeAmount*mod);
    }
    
    private void OnStacksChanged(Effect me,int changeAmount)
    {
        float baseVal = source.myCombatStats.GetStat(statToModify, true);
        source.myCombatStats.ChangeStat(statToModify,baseVal*0.05f*changeAmount*mod);
    }
}
