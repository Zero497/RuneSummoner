using System.Collections.Generic;
using UnityEngine;

public class Divert : ActiveAbility
{
    private bool isActive = false;

    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttack;

    private ActionPriorityWrapper<UnitBase> onUnitAddedToCombat;

    private ActionPriorityWrapper<UnitBase> onTurnStarted;
    
    public override bool RunAction(SendData actionData)
    {
        return false;
    }

    private void OnTurnStarted(UnitBase myUnit)
    {
        isActive = false;
    }

    private void OnAttack(UnitBase attacker, Attack.AttackMessageToTarget attack)
    {
        if (HexTileUtility.AreAdjacent(source.currentPosition, attacker.currentPosition) &&
            HexTileUtility.AreAdjacent(source.currentPosition, attack.target.currentPosition) && isActive)
        {
            SendData markData = new SendData(attack.target);
            markData.AddUnit(source);
            markData.AddStr("marked");
            markData.AddFloat(1+2*level);
            attack.target.AddEffect(markData);
        }
    }

    private void OnUnitAddedToCombat(UnitBase unit)
    {
        if (unit.isFriendly == source.isFriendly)
        {
            unit.myEvents.applyToOutgoingAttack.Subscribe(onAttack);
        }
    }

    public override bool PrepAction()
    {
        isActive = !isActive;
        return true;
    }

    public override bool RushCompletion()
    {
        throw new System.NotImplementedException();
    }

    public override void Initialize(SendData sendData)
    {
        base.Initialize(sendData);
        onAttack = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>
        {
            priority = 80,
            action = OnAttack
        };
        List<UnitBase> subList;
        if (source.isFriendly)
            subList = MainCombatManager.manager.allFriendly;
        else
        {
            subList = MainCombatManager.manager.allEnemy;
        }
        foreach (UnitBase unit in subList)
        {
            if (!unit.Equals(source))
            {
                unit.myEvents.applyToOutgoingAttack.Subscribe(onAttack);
            }
        }

        onTurnStarted = new ActionPriorityWrapper<UnitBase>
        {
            priority = 32,
            action = OnTurnStarted
        };
        source.myEvents.onTurnStarted.Subscribe(onTurnStarted);

        onUnitAddedToCombat = new ActionPriorityWrapper<UnitBase>
        {
            priority = 80,
            action = OnUnitAddedToCombat
        };
        MainCombatManager.manager.onUnitAddedToCombat.Subscribe(onUnitAddedToCombat);
    }

    public override string GetID()
    {
        return "Divert";
    }
}
