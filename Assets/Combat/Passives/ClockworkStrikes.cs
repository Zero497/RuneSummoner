using System.Collections.Generic;
using UnityEngine;

public class ClockworkStrikes : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttack;

    private ActionPriorityWrapper<UnitBase> onTurnEnd;

    private Dictionary<Attack, int> attacksLastTurn;

    private List<Attack> attacksThisTurn;

    /*
        Expects:
            Int 0: name of passive ability
            Unit 0: unit to apply to
            Int 1: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onAttack = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        onAttack.priority = 42;
        onAttack.action = OnAttack;
        source.myEvents.applyToOutgoingAttack.Subscribe(onAttack);
        onTurnEnd = new ActionPriorityWrapper<UnitBase>();
        onTurnEnd.priority = 72;
        onTurnEnd.action = OnTurnEnd;
        source.myEvents.onTurnEnded.Subscribe(onTurnEnd);
        attacksLastTurn = new Dictionary<Attack, int>();
        attacksThisTurn = new List<Attack>();
    }
    
    public override string GetAbilityName()
    {
        return "Clockwork Strikes";
    }

    private void OnTurnEnd(UnitBase myUnit)
    {
        foreach (Attack attack in attacksThisTurn)
        {
            if (attacksLastTurn.ContainsKey(attack))
            {
                attacksLastTurn[attack]++;
            }
            else
            {
                attacksLastTurn[attack] = 1;
            }
        }
        foreach (Attack key in attacksLastTurn.Keys)
        {
            if (!attacksThisTurn.Contains(key))
            {
                attacksLastTurn.Remove(key);
            }
        }
    }

    private void OnAttack(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        if (attacksLastTurn.ContainsKey(attack.sourceAttack))
        {
            attack.damage += attack.baseDamage * Mathf.Max(0.15f * attacksLastTurn[attack.sourceAttack], 0.6f) * level;
        }
        attacksThisTurn.Add(attack.sourceAttack);
    }
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Clockwork Strikes";
        ret.desc = 
            "For each previous turn in succession this Unit has used a particular attack, that attack’s damage is increased by "+(level*15)+"% (15% base), to a maximum of "+(level*60)+"% (60% base).";
        ret.levelEffect = "+15% damage increase per turn and +60% max damage increase amount per Level.";
        return ret;
    }
}
