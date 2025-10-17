using System.Collections.Generic;
using UnityEngine;

public class Fury : Attack
{
    private Attack basicAttack;
    
    public override void Initialize(SendData sendData)
    {
        base.Initialize(sendData);
        foreach (ActiveAbility ability in source.activeAbilities)
        {
            if (ability.id.Equals("PhysicalMelee") || ability.id.Equals("PhysicalRanged") ||
                ability.id.Equals("MagicalMelee") || ability.id.Equals("MagicalRanged"))
            {
                basicAttack = (Attack) ability;
            }
        }
    }

    public override float GetRange(bool getBase = false)
    {
        return basicAttack.GetRange();
    }

    public override bool RunAction(SendData sentData)
    {
        if (source.usedAbilityThisTurn) return false;
        bool ret = true;
        if (!source.PayCost(this, false)) return false;
        float mod = 1;
        if (sentData.floatData.Count > 0) mod = sentData.floatData[0];
        for(int i = 0; i<level+2; i++)
            ret = RunSingleTarget(getValidTargets(), sentData.positionData[0], mod);
        if (ret)
        {
            source.PayCost(this);
            source.usedAbilityThisTurn = true;
        }
        OverlayManager.instance.ClearOverlays();
        ClickManager.clickManager.SetAction(null);
        return ret;
    }

    public override Float GetStaminaCost(bool getBase = false)
    {
        return new Float(basicAttack.GetStaminaCost(getBase).flt * 1.25f * (level+2));
    }
    
    public override Float GetManaCost(bool getBase = false)
    {
        return new Float(basicAttack.GetManaCost(getBase).flt * 1.25f * (level+2));
    }
    
    public override bool PrepAction()
    {
        OverlayManager.instance.ClearOverlays();
        if (!prepped)
        {
            OverlayManager.instance.CreateOverlay(HexTileUtility.DjikstrasGetTilesInRange(TurnController.controller.mainMap, source.currentPosition, (basicAttack.abilityData as AttackData).range, 1),"AttackOverlay");
            prepped = true;
            ClickManager.clickManager.SetAction(this);
            return false;
        }
        ClickManager.clickManager.SetAction(null);
        prepped = false;
        return false;
    }
}
