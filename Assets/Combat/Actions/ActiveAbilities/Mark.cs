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
}
