using System;
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
    
    public static AbilityText GetAbilityText(int level, float abilityPower)
    {
        AbilityText ret = new AbilityText();
        AbilityData abData = Resources.Load<AbilityData>("AbilityData/Electric Shroud");
        ret.name = "Taunt";
        ret.desc = abData.description;
        ret.abilityType = "Debuff";
        ret.range = "Self";
        float temp = abData.staminaCost*(1+0.05f*abilityPower);
        temp = MathF.Round(temp, 2);
        ret.cost = temp + " ("+abData.staminaCost+" base) Stamina";
        ret.isAOE = true;
        temp = 2 + Mathf.FloorToInt(abilityPower / 20) + level;
        ret.aoeRange = temp + " (3 base)";
        ret.targetType = "AOE Enemy";
        temp = 200 * (0.75f + 0.05f * abilityPower + 0.25f * level);
        temp = MathF.Round(temp, 2);
        ret.special =
            "Free action. Raises threat against targets by "+temp+" (200 base).";
        ret.apEffect = "Threat amount +5% per AP. Cost +5% per AP. AOE Range +1 per 20 AP.";
        ret.levelEffect = "AOE Range +1 per Level, Threat amount +25% per Level after AP increases.";
        ret.icon = Resources.Load<Sprite>("Icons/Taunt");
        return ret;
    }
}
