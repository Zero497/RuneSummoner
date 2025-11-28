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
            Int 0: name of passive ability
            Unit 0: unit to apply to
            Int 1: level of ability
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
    
    public override string GetAbilityName()
    {
        return "Defensive Attacks";
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
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Defensive Attacks";
        ret.desc = 
            "After attacking a Unit, this Unit takes "+(25*level)+"% (25% base) less damage from attacks made by that Unit until this Unit’s next turn.";
        ret.levelEffect = "+25% damage reduction per Level.";
        return ret;
    }
}
