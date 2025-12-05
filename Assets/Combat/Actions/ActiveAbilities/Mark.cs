using System;
using Unity.VisualScripting;
using UnityEngine;

public class Mark : ActiveAbility
{
    public override bool RunAction(SendData sentData)
    {
        if (source.usedAbilityThisTurn) return false;
        bool ret = true;
        if (!source.PayCost(this, false)) return false;
        Func<int, bool> ValidTarget = getValidTargets();
        UnitBase unitAtPosition = MainCombatManager.manager.getUnitAtPosition(sentData.positionData[0]);
        if (unitAtPosition == null) return false;
        if (ValidTarget(unitAtPosition.myTeam))
        {
            SendData markData = new SendData(unitAtPosition);
            markData.AddUnit(source);
            markData.AddStr("marked");
            markData.AddFloat(1+2*level);
            unitAtPosition.AddEffect(markData);
            source.PayCost(this);
            source.usedAbilityThisTurn = true;
        }
        else
        {
            ret = false;
        }
        OverlayManager.instance.ClearOverlays();
        ClickManager.clickManager.SetAction(null);
        return ret;
    }

    public override bool PrepAction()
    {
        OverlayManager.instance.ClearOverlays();
        if (!prepped)
        {
            OverlayManager.instance.CreateOverlay(HexTileUtility.DjikstrasGetTilesInRange(TurnController.controller.mainMap, source.currentPosition, abilityData.range, 1),"AttackOverlay");
            prepped = true;
            ClickManager.clickManager.SetAction(this);
            return true;
        }
        ClickManager.clickManager.SetAction(null);
        prepped = false;
        return false;
    }

    public override bool RushCompletion()
    {
        throw new System.NotImplementedException();
    }

    public override string GetID()
    {
        return "Mark";
    }
    
    public static AbilityText GetAbilityText(int level, float abilityPower)
    {
        AbilityText ret = new AbilityText();
        AbilityData abData = Resources.Load<AbilityData>("AbilityData/Mark");
        ret.name = "Mark";
        ret.desc = abData.description;
        ret.abilityType = "Debuff";
        ret.range = "Sight";
        ret.cost = abData.staminaCost+" Stamina";
        ret.targetType = "Single Enemy";
        ret.special =
            "The target gains Marked "+(1+2*level)+" (3 base).";
        ret.apEffect = "None.";
        ret.levelEffect = "+2 stacks of Marked per Level.";
        ret.icon = Resources.Load<Sprite>("Icons/Mark");
        return ret;
    }
}
