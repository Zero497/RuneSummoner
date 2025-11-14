using System.Collections.Generic;
using UnityEngine;

public class Taunt : ActiveAbility
{
    public override bool RunAction(SendData actionData)
    {
        if (!source.PayCost(this)) return false;
        foreach (UnitBase unit in MainCombatManager.manager.allEnemy)
        {
            if (HexTileUtility.GetTileDistance(source.currentPosition, unit.currentPosition) < GetAOERange())
            {
                if (unit.threatDict.ContainsKey(source))
                {
                    unit.threatDict[source] += GetThreatToAdd();
                }
                else
                {
                    unit.threatDict[source] = GetThreatToAdd();
                }
            }
        }
        return true;
    }

    private float GetThreatToAdd()
    {
        float threat = 200;
        threat *= 1+0.05f * source.abilityPower;
        threat *= 1+0.25f * (level - 1);
        return threat;
    }

    public override float GetAOERange(bool getBase = false)
    {
        TauntExtension te = source.GetPassive(new SendData((int)PassiveAbility.PassiveAbilityDes.tauntExtension)) as TauntExtension;
        if (te != null)
        {
            return base.GetAOERange(getBase) + Mathf.Floor(source.abilityPower/20.0f)+level-1+3*te.GetLevel();
        }
        return base.GetAOERange(getBase) + Mathf.Floor(source.abilityPower/20.0f)+level-1;
    }

    public override string GetID()
    {
        return "Taunt";
    }


    public override bool PrepAction()
    {
        OverlayManager.instance.ClearOverlays();
        if (!prepped)
        {
            OverlayManager.instance.CreateOverlay(HexTileUtility.DjikstrasGetTilesInRange(TurnController.controller.mainMap, source.currentPosition, GetAOERange(), 1),"AttackOverlay");
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
}
