using System.Collections.Generic;
using UnityEngine;

public class Evade : Effect
{
    private bool hasExposingEvade = false;
    
    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttacked;

    private ActionPriorityWrapper<UnitBase> onTurnStarted;
    
    

    /*
       Expects:
           Unit 0: unit to apply to
           String 0: effect name
           Float 0: stacks to apply
    */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onAttacked = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>
        {
            priority = 120,
            action = OnAttacked
        };
        source.myEvents.onAttacked.Subscribe(onAttacked);
        onTurnStarted = new ActionPriorityWrapper<UnitBase>
        {
            priority = 70,
            action = OnTurnStarted
        };
        source.myEvents.onTurnStarted.Subscribe(onTurnStarted);
        hasExposingEvade = source.GetPassive(new SendData((int)PassiveAbility.PassiveAbilityDes.exposingEvade)) != null;
    }

    public override void RemoveEffect()
    {
        source.myEvents.onAttacked.Unsubscribe(onAttacked);
        source.myEvents.onTurnStarted.Unsubscribe(onTurnStarted);
        base.RemoveEffect();
    }

    public override void AddStacks(int stacks)
    {
        base.AddStacks(stacks);
        if (base.stacks > 18)
        {
            base.stacks = 18;
        }
    }

    private void OnAttacked(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        float rand = Random.Range(0, 1);
        if (rand > 0.05f * stacks)
        {
            attack.damage = 0;
            attack.baseDamage = 0;
            attack.effectsToApplyTarget = new List<string>();
            if (hasExposingEvade)
            {
                int level = source.GetPassive(new SendData((int)PassiveAbility.PassiveAbilityDes.exposingEvade)).GetLevel();
                SendData markData = new SendData(attack.source);
                markData.AddUnit(source);
                markData.AddStr("marked");
                markData.AddFloat(1+2*level);
                attack.source.AddEffect(markData);
            }
        }
    }

    private void OnTurnStarted(UnitBase myUnit)
    {
        RemoveEffect();
    }
}
