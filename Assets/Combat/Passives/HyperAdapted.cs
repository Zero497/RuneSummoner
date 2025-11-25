using System.Collections.Generic;
using UnityEngine;

public class HyperAdapted : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase, Effect, int> onEffectApplied;

    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onEffectApplied = new ActionPriorityWrapper<UnitBase, Effect, int>();
        onEffectApplied.priority = 72;
        onEffectApplied.action = OnEffectApplied;
        source.myEvents.onEffectApplied.Subscribe(onEffectApplied);
    }
    
    public override string GetAbilityName()
    {
        return "Hyper-Adapted";
    }

    private void OnEffectApplied(UnitBase myUnit, Effect appEffect, int stacks)
    {
        if (appEffect.effectName.Equals("resistant"))
        {
            appEffect.AddStacks(stacks*Mathf.FloorToInt(2.5f*level)-stacks);
            Effect toRemove = null;
            foreach (Effect effect in myUnit.activeEffects)
            {
                if (effect.effectName.Equals("resistant") && !effect.Equals(appEffect))
                {
                    toRemove = effect;
                    break;
                }
            }
            if (toRemove != null)
            {
                toRemove.RemoveEffect();
            }
        }
    }
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Hyper-Adapted\n";
        ret += 
            "This Unit can only have Resistant stacks for one Element (new instances replace old ones). Whenever it gains Resistant stacks, it gains "+(250*level)+"% (250% base) as many.\n";
        ret += "Level Effect: +250% Resistant applied per Level.\n";
        return ret;
    }
}
