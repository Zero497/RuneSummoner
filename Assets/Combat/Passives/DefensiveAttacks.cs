using System.Collections.Generic;
using UnityEngine;

public class DefensiveAttacks : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase> onTurnStarted;

    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttack;

    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onHit;

    private HashSet<UnitBase> hitLastTurn;

    /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        hitLastTurn = new HashSet<UnitBase>();
        onTurnStarted = new ActionPriorityWrapper<UnitBase>();
        onTurnStarted.priority = 80;
        onTurnStarted.action = OnTurnStarted;
        source.myEvents.onTurnStarted.Subscribe(onTurnStarted);
        onAttack = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        onAttack.priority = 70;
        onAttack.action = OnAttack;
        source.myEvents.applyToOutgoingAttack.Subscribe(onAttack);
        onHit = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        onHit.priority = 42;
        onHit.action = OnHit;
        source.myEvents.onAttacked.Subscribe(onHit);
    }

    private void OnTurnStarted(UnitBase myUnit)
    {
        hitLastTurn = new HashSet<UnitBase>();
    }

    private void OnAttack(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        hitLastTurn.Add(attack.target);
    }

    private void OnHit(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        if (hitLastTurn.Contains(attack.source))
        {
            attack.damage -= attack.baseDamage * 0.25f * level;
        }
    }
}
