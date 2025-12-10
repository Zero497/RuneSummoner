using System;
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
            if (ability.id.Equals("Physical Melee") || ability.id.Equals("Physical Ranged") ||
                ability.id.Equals("Magical Melee") || ability.id.Equals("Magical Ranged"))
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
        bool ret = false;
        if (!source.PayCost(basicAttack, false)) return false;
        float mod = 1;
        if (sentData.floatData.Count > 0) mod = sentData.floatData[0];
        for (int i = 0; i < level + 2; i++)
        {
            ret = basicAttack.RunSingleTarget(getValidTargets(), sentData.positionData[0], mod) || ret;
            if (ret)
            {
                source.PayCost(basicAttack);
            }
        }
        if (ret)
        {
            
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
            AttackData dat = (basicAttack.abilityData as AttackData);
            OverlayManager.instance.CreateOverlay(HexTileUtility.DjikstrasGetTilesInRange(TurnController.controller.mainMap, source.currentPosition, dat.range, 1),"AttackOverlay");
            prepped = true;
            ClickManager.clickManager.SetAction(this);
            return false;
        }
        ClickManager.clickManager.SetAction(null);
        prepped = false;
        return false;
    }

    public static AbilityText GetAbilityText(int level, float abilityPower, float physicalAttack, float magicalAttack, AbilityText basicAbilityText)
    {
        AbilityText ret = new AbilityText();
        AttackData attackData = Resources.Load<AttackData>("AttackData/Fury");
        ret.isAttack = true;
        ret.name = "Fury";
        ret.desc = attackData.description;
        ret.abilityType = "Attack";
        ret.range = basicAbilityText.range;
        string[] temp = basicAbilityText.damage.Split(" ");
        float temp2 = float.Parse(temp[0]) * 0.75f;
        temp2 = MathF.Round(temp2, 2);
        ret.damage = temp2+" "+temp[1]+" "+temp[2]+" "+temp[3]+" "+temp[4]+" x"+(2+level);
        temp = basicAbilityText.cost.Split(" ");
        temp2 = float.Parse(temp[0]) * 1.25f;
        temp2 = MathF.Round(temp2, 2);
        ret.cost = temp2+" "+temp[1]+" "+temp[2]+" "+temp[3];
        ret.targetType = "Single Enemy";
        ret.special = "Make "+(2+level)+" (3 base) Basic Attacks at a single target with 25% reduced damage and 25% increased Stamina or Mana Cost.";
        ret.apEffect = "+5% damage and cost per AP";
        ret.levelEffect = "Make 1 additional Basic Attack per level";
        ret.icon = Resources.Load<Sprite>("Icons/Fury");
        return ret;
    }
}
