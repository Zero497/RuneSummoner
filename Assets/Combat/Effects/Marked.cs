using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Marked : Effect
{
    private UnitBase applier;

    private ActionPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode> onApplierMoved;

    private ActionPriorityWrapper<UnitBase> onApplierDied;

    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onHit;

    private bool applierHasDebilitating;

    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttack;
    
    private bool applierHasLasting;
    
    /*
        Expects:
            Unit 0: unit to apply to
            Unit 1: unit applying effect
            String 0: effect name
            Float 0: stacks to apply
     */
    public override void Initialize(SendData data)
    {
        stackDecayAmount = 10000;
        stackDecayTime = 200;
        base.Initialize(data);
        applier = data.unitData[1];
        onApplierDied = new ActionPriorityWrapper<UnitBase>();
        onApplierDied.priority = 80;
        onApplierDied.action = RemoveEffect;
        applier.myEvents.onDeath.Subscribe(onApplierDied);
        onApplierMoved = new ActionPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode>();
        onApplierMoved.priority = 70;
        onApplierMoved.action = OnApplierMoved;
        applier.myEvents.onMoveEnd.Subscribe(onApplierMoved);
        onHit = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        onHit.priority = 32;
        onHit.action = OnHit;
        source.myEvents.onAttacked.Subscribe(onHit);
        applierHasDebilitating = source.GetPassive(new SendData((int)PassiveAbility.PassiveAbilityDes.debilitatingMark)) != null;
        if (applierHasDebilitating)
        {
            onAttack = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
            onAttack.priority = 20;
            onAttack.action = OnAttack;
            source.myEvents.applyToOutgoingAttack.Subscribe(onAttack);
        }
        applierHasLasting = source.GetPassive(new SendData((int)PassiveAbility.PassiveAbilityDes.lastingMark)) != null;
    }

    private void OnAttack(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        if (applierHasDebilitating)
        {
            PassiveAbility deb = source.GetPassive(new SendData((int)PassiveAbility.PassiveAbilityDes.debilitatingMark));
            attack.damage -= attack.baseDamage * 0.25f * deb.GetLevel();
        }
    }

    public UnitBase GetApplier()
    {
        return applier;
    }

    /*
        Expects:
            String 0: effect name
            Unit 1: applier of effect
     */
    public override bool Equals(SendData data)
    {
        if (base.Equals(data))
        {
            if (applier.Equals(data.unitData[0]))
            {
                return true;
            }
        }
        return false;
    }

    public override bool Equals(Effect other)
    {
        if (base.Equals(other))
        {
            Marked otherA = (Marked)other;
            if (applier.Equals(otherA.GetApplier()))
                return true;
        }

        return false;
    }

    private void OnApplierMoved(UnitBase unit, HexTileUtility.DjikstrasNode node)
    {
        Dictionary<UnitBase, HashSet<String>> dict;
        if (applier.isFriendly)
        {
            dict = VisionManager.visionManager.visibleEnemyUnits;
        }
        else
        {
            dict = VisionManager.visionManager.visibleFriendlyUnits;
        }
        foreach (KeyValuePair<UnitBase, HashSet<String>> kvp in dict)
        {
            if (kvp.Key.Equals(unit))
            {
                if (!kvp.Value.Contains(applier.myId))
                {
                    RemoveEffect();
                }
            }
        }
    }

    public override void RemoveEffect()
    {
        applier.myEvents.onDeath.Unsubscribe(onApplierDied);
        applier.myEvents.onMoveEnd.Unsubscribe(onApplierMoved);
        source.myEvents.onAttacked.Unsubscribe(onHit);
        if(onAttack != null)
            source.myEvents.applyToOutgoingAttack.Unsubscribe(onAttack);
        base.RemoveEffect();
    }

    public override void AddStacks(int addStacks)
    {
        base.AddStacks(addStacks);
        //TODO stacks decay invidually
    }

    private void RemoveEffect(UnitBase unit)
    {
        RemoveEffect();
    }

    private void OnHit(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        attack.damage += attack.baseDamage * 0.25f * stacks;
        if(!applierHasLasting)
            RemoveEffect();
        else
        {
            AddStacks(-Mathf.CeilToInt(stacks*0.5f));
        }
    }
}
