using UnityEngine;

public class Mechanical : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase, Effect, int> onEffectIncoming;

    /*
        Expects:
            Int 0: name of passive ability
            Unit 0: unit to apply to
            Int 1: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onEffectIncoming = new ActionPriorityWrapper<UnitBase, Effect, int>();
        onEffectIncoming.priority = 1;
        onEffectIncoming.action = OnEffectIncoming;
        source.myEvents.modifyIncomingEffect.Subscribe(onEffectIncoming);
    }
    
    public override string GetAbilityName()
    {
        return "Mechanical";
    }

    private void OnEffectIncoming(UnitBase myUnit, Effect incomingEffect, int stacks)
    {
        if (incomingEffect.isBuff) return;
        if (stacks == 1)
        {
            float rand = Random.Range(0, 1);
            if (rand >= Mathf.Min(0.3f + 0.2f * level, 0.9f))
            {
                incomingEffect.AddStacks(-1);
            }
        }
        else
        {
            int toRemove = Mathf.FloorToInt(stacks / (float)level+1);
            incomingEffect.AddStacks(-toRemove);
        }
    }
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Mechanical\n";
        ret += 
            "When inflicted with two or more debuff stacks, divide incoming stack count by "+(level+1)+" (2 base), rounded up. When inflicted with a single debuff stack (or a non-stacking debuff), have a "+(Mathf.Min(30+20*level, 90))+"% (50% base) chance not to gain that debuff.\n";
        ret += "Level Effect: Increase divisor for incoming stacks by 1 per Level. Reduce to chance to receive incoming single stack debuffs by 20% to a maximum of 90%.\n";
        return ret;
    }
}
