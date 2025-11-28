using System;
using System.Collections.Generic;
using UnityEngine;

public class Ambush : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttack;

    private ActionPriorityWrapper<UnitBase> onTurnStart;

    private List<String> unapplicableEnemies = new List<String>();

    /*
        Expects:
            Unit 0: unit to apply to
            Int 1: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onTurnStart = new ActionPriorityWrapper<UnitBase>();
        onTurnStart.priority = 64;
        onTurnStart.action = OnTurnStart;
        source.myEvents.onTurnStarted.Subscribe(onTurnStart);
        onAttack = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        onAttack.priority = 48;
        onAttack.action = OnAttack;
        source.myEvents.applyToOutgoingAttack.Subscribe(onAttack);
    }
    
    public override string GetAbilityName()
    {
        return "Ambush";
    }

    private void OnAttack(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        if (!unapplicableEnemies.Contains(attack.target.myId))
        {
            attack.damage += attack.baseDamage * 0.5f * level;
        }
    }

    private void OnTurnStart(UnitBase myUnit)
    {
        unapplicableEnemies = new List<string>();
        Dictionary<UnitBase, HashSet<String>> dict;
        if (myUnit.isFriendly)
        {
            dict = VisionManager.visionManager.visibleFriendlyUnits;
        }
        else
        {
            dict = VisionManager.visionManager.visibleEnemyUnits;
        }

        foreach (KeyValuePair<UnitBase, HashSet<String>> kvp in dict)
        {
            if (kvp.Key.Equals(myUnit))
            {
                foreach (String eId in kvp.Value)
                {
                    unapplicableEnemies.Add(eId);
                }
            }
        }
    }
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Ambush";
        ret.desc = "When this Unit attacks an enemy Unit that could not see it at the start of this Unit’s turn, it deals "+(50*level)+"% (base 50) additional damage.";
        ret.levelEffect = "+50% bonus damage per Level.";
        return ret;
    }
}
