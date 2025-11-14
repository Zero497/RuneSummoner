using System.Collections.Generic;
using UnityEngine;

public class Guardian : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> modIncomingAttacks;

    private ActionPriorityWrapper<UnitBase> onUnitAddedToCombat;
    
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        modIncomingAttacks = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        modIncomingAttacks.priority = 200;
        modIncomingAttacks.action = ModIncomingAttack;
        List<UnitBase> units;
        if (source.isFriendly)
        {
            units = MainCombatManager.manager.allFriendly;
        }
        else
        {
            units = MainCombatManager.manager.allEnemy;
        }
        foreach (UnitBase unit in units)
        {
            unit.myEvents.onAttacked.Subscribe(modIncomingAttacks);
        }

        onUnitAddedToCombat = new ActionPriorityWrapper<UnitBase>();
        onUnitAddedToCombat.priority = 72;
        onUnitAddedToCombat.action = OnUnitAddedToCombat;
        MainCombatManager.manager.onUnitAddedToCombat.Subscribe(onUnitAddedToCombat);
    }
    
    public override string GetAbilityName()
    {
        return "Guardian";
    }

    private void OnUnitAddedToCombat(UnitBase newUnit)
    {
        if (newUnit.isFriendly == source.isFriendly)
        {
            newUnit.myEvents.onAttacked.Subscribe(modIncomingAttacks);
        }
    }

    private void ModIncomingAttack(UnitBase target, Attack.AttackMessageToTarget attack)
    {
        if (target != source && HexTileUtility.AreAdjacent(target.currentPosition, source.currentPosition) && target.baseData.myCombatType != UnitData.CombatType.tank)
        {
            source.TakeDamage(attack.damageType, attack.damageElement, attack.damage * 0.5f * (1-0.2f*level));
            attack.damage *= 0.5f;
        }
    }
}
