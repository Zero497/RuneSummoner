using UnityEngine;

public class APUp : Effect
{
    private int mod;
    
    /*
       Expects:
           Unit 0: unit to apply to
           String 0: effect name
           Float 0: stacks to apply
           Float 1: 1 for mod up, -1 for mod down
    */
    public override void Initialize(SendData data)
    {
        stackDecayAmount = 1;
        stackDecayTime = 100;
        mod = Mathf.FloorToInt(data.floatData[1]);
        base.Initialize(data);
        source.myCombatStats.AddAbilityPower(source.myCombatStats.getAbilityPower(true)*stacks*0.05f*mod);
    }

    public override void RemoveEffect()
    {
        source.myCombatStats.AddAbilityPower(-source.myCombatStats.getAbilityPower(true)*stacks*0.05f);
        base.RemoveEffect();
    }

    public override void AddStacks(int addStacks)
    {
        source.myCombatStats.AddAbilityPower(-source.myCombatStats.getAbilityPower(true)*addStacks*0.05f*mod);
        if (stacks < 0)
        {
            mod *= -1;
            stacks *= -1;
        }
        base.AddStacks(addStacks);
    }
    
    /*
        Expects:
            String 0: effect name
            String 1: stat name to modify
     */
    public override bool MatchForMerge(SendData data)
    {
        return data.strData[0].Equals(effectName);
    }

    public override bool MatchForMerge(Effect effect)
    {
        return effect.effectName.Equals(effectName);
    }

    public int GetMod()
    {
        return mod;
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
            return true;
        }

        return false;
    }
    
    public override bool MergeEffects(Effect effect)
    {
        if (MatchForMerge(effect))
        {
            APUp effectA = (APUp)effect;
            if (Mathf.FloorToInt(mod) == Mathf.FloorToInt(effectA.GetMod()))
            {
                AddStacks((int)effect.getStacks());
            }
            else
            {
                AddStacks(-(int)effect.getStacks());
            }
            return true;
        }

        return false;
    }

    public override bool Equals(Effect other)
    {
        if (other == null) return false;
        if (other.effectName.Equals(effectName))
        {
            APUp otherA = (APUp)other;
            return (Mathf.FloorToInt(otherA.GetMod()) == Mathf.FloorToInt(mod));
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
            return (Mathf.FloorToInt(data.floatData[1]) == Mathf.FloorToInt(mod));
        }
        return false;
    }
}
