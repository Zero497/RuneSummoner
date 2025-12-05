using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//VALID IDS: PhysicalMelee, PhysicalRanged, MagicalMelee, MagicalRanged
public class Attack : ActiveAbility
{
    private ActiveAbility _activeAbilityImplementation;

    public static float physMagAttBonus = 0.03f;

    public override bool RunAction(SendData sentData)
    {
        if (source.usedAbilityThisTurn) return false;
        bool ret = true;
        if (!source.PayCost(this, false)) return false;
        float mod = 1;
        if (sentData.floatData.Count > 0) mod = sentData.floatData[0];
        switch (abilityData.targetType)
        {
            case AbilityData.TargetType.singleTargetEnemy:
            case AbilityData.TargetType.singleTargetFriendly:
            case AbilityData.TargetType.singleTargetNeutral:
                ret = RunSingleTarget(getValidTargets(), sentData.positionData[0], mod);
                break;
            case AbilityData.TargetType.aoeNeutral:
            case AbilityData.TargetType.aoeEnemyOnly:
            case AbilityData.TargetType.aoeFriendlyOnly:
                ret = RunAOE(getValidTargets(), sentData.positionData[0], mod);
                break;
        }
        if (ret)
        {
            source.PayCost(this);
            source.usedAbilityThisTurn = true;
        }
        OverlayManager.instance.ClearOverlays();
        ClickManager.clickManager.SetAction(null);
        return ret;
    }

    protected virtual bool RunAOE(Func<int, bool> validTarget, Vector3Int position, float mod = 1)
    {
        List<UnitBase> targets = new List<UnitBase>();
        foreach (UnitBase unit in MainCombatManager.manager.allEnemy)
        {
            if (validTarget(unit.myTeam) && HexTileUtility.GetTileDistance(position, unit.currentPosition) < GetAOERange())
            {
                targets.Add(unit);
            }
        }
        foreach (UnitBase unit in MainCombatManager.manager.allFriendly)
        {
            if (validTarget(unit.myTeam) && HexTileUtility.GetTileDistance(position, unit.currentPosition) < GetAOERange())
            {
                targets.Add(unit);
            }
        }
        RunAttack(targets, mod);
        return targets.Count > 0;
    }

    protected virtual bool RunSingleTarget(Func<int, bool> validTarget, Vector3Int position, float mod=1)
    {
        UnitBase unitAtPosition = MainCombatManager.manager.getUnitAtPosition(position);
        if (unitAtPosition != null && validTarget(unitAtPosition.myTeam))
        {
            RunAttack(new List<UnitBase>{unitAtPosition}, mod);
            return true;
        }
        return false;
    }

    public void RunAttack(List<UnitBase> targets, float mod = 1)
    {
        foreach (UnitBase unit in targets)
        {
            AttackMessageToTarget outgoingAttack = PrepareMessage(unit, mod);
            unit.ReceiveAttack(outgoingAttack);
        }
        ApplyEffectsToSelf();
    }

    protected virtual void ApplyEffectsToSelf()
    {
        AttackData myAData = abilityData as AttackData;
        if (myAData.effectsToApplySelf != null)
        {
            for (int i = 0; i < myAData.effectsToApplySelf.Count; i++)
            {
                SendData data = new SendData(myAData.effectsToApplySelf[i]);
                data.AddFloat(myAData.baseStacksToApplyToSelf[i]);
                source.AddEffect(data);
            }
        }
    }

    protected virtual float AbilityPowerBonus(float damage)
    {
        return damage * (0.05f * source.abilityPower);
    }

    protected virtual AttackMessageToTarget PrepareMessage(UnitBase target, float mod=1)
    {
        AttackMessageToTarget retval = new AttackMessageToTarget(abilityData as AttackData, this);
        retval.source = source;
        retval.target = target;
        if ((abilityData as AttackData).useUnitElement)
        {
            retval.damageElement = source.baseData.DefaultDamageElement;
        }
        float mult = 1;
        if (retval.damageType == AttackData.DamageType.Magic)
        {
            mult += source.magicalAttack  * 3 / 100;
        }
        else if (retval.damageType == AttackData.DamageType.Physical)
        {
            mult += source.physicalAttack * 3 / 100;
        }
        float abilityPowerBonus = AbilityPowerBonus(retval.baseDamage);
        retval.baseDamage *= mult * mod;
        retval.damage *= mult * mod;
        retval.baseDamage += abilityPowerBonus;
        retval.damage += abilityPowerBonus;
        source.ModifyOutgoingAttack(retval);
        return retval;
    }

    public override bool PrepAction()
    {
        OverlayManager.instance.ClearOverlays();
        if (!prepped)
        {
            OverlayManager.instance.CreateOverlay(HexTileUtility.DjikstrasGetTilesInRange(TurnController.controller.mainMap, source.currentPosition, GetRange(), 1),"AttackOverlay");
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
        return true;
    }

    private string ParseDescription(string description)
    {
        throw new System.NotImplementedException();
    }

    public override void Initialize(SendData sendData)
    {
        source = sendData.unitData[0];
        myDes = (ActiveAbilityDes)sendData.intData[0];
        string path = "AttackData/" + GetID();
        abilityData = Resources.Load<AbilityData>(path);
        level = sendData.intData[1];
    }

    public override string GetID()
    {
        switch (myDes)
        {
            case ActiveAbilityDes.magicalMelee:
                return "Magical Melee";
            case ActiveAbilityDes.fury:
                return "Fury";
            case ActiveAbilityDes.coreOverload:
                return "Core Overload";
            case ActiveAbilityDes.exposeCore:
                return "Expose Core";
            case ActiveAbilityDes.magicalRanged:
                return "Magical Ranged";
            case ActiveAbilityDes.physicalMelee:
                return "Physical Melee";
            case ActiveAbilityDes.physicalRanged:
                return "Physical Ranged";
            default:
                return "Attack";
        }
    }

    public static AbilityText GetAbilityText(int level, float abilityPower, float physicalAttack, float magicalAttack, ActiveAbilityDes des, AttackData.Element defaultElement)
    {
        AbilityText ret = new AbilityText();
        ret.isAttack = true;
        AttackData attackData = null;
        float temp;
        switch (des)
        {
            case ActiveAbilityDes.physicalMelee:
                attackData = Resources.Load<AttackData>("AttackData/Physical Melee");
                ret.name = "Basic Attack (Physical Melee)";
                temp = attackData.damage*(1+0.05f*abilityPower)+attackData.damage*0.03f*physicalAttack;
                temp = MathF.Round(temp, 2);
                ret.damage = temp + " ("+attackData.damage+" base) Physical "+defaultElement.ToString();
                temp = attackData.staminaCost * (1 + 0.05f * abilityPower);
                temp = MathF.Round(temp, 2);
                ret.cost = temp+" ("+attackData.staminaCost+" base) Stamina";
                ret.icon = Resources.Load<Sprite>("Icons/PhysicalMelee");
                break;
            case ActiveAbilityDes.physicalRanged:
                attackData = Resources.Load<AttackData>("AttackData/Physical Ranged");
                ret.name = "Basic Attack (Physical Ranged)";
                temp = attackData.damage*(1+0.05f*abilityPower)+attackData.damage*0.03f*physicalAttack;
                temp = MathF.Round(temp, 2);
                ret.damage = temp + " ("+attackData.damage+" base) Physical "+defaultElement.ToString();
                temp = attackData.staminaCost * (1 + 0.05f * abilityPower);
                temp = MathF.Round(temp, 2);
                ret.cost = temp+" ("+attackData.staminaCost+" base) Stamina";
                ret.icon = Resources.Load<Sprite>("Icons/PhysicalRanged");
                break;
            case ActiveAbilityDes.magicalMelee:
                attackData = Resources.Load<AttackData>("AttackData/Magical Melee");
                ret.name = "Basic Attack (Magical Melee)";
                temp = attackData.damage*(1+0.05f*abilityPower)+attackData.damage*0.03f*magicalAttack;
                temp = MathF.Round(temp, 2);
                ret.damage = temp + " ("+attackData.damage+" base) Magical "+defaultElement.ToString();
                temp = attackData.manaCost * (1 + 0.05f * abilityPower);
                temp = MathF.Round(temp, 2);
                ret.cost = temp+" ("+attackData.manaCost+" base) Mana";
                ret.icon = Resources.Load<Sprite>("Icons/MagicalMelee");
                break;
            case ActiveAbilityDes.magicalRanged:
                attackData = Resources.Load<AttackData>("AttackData/Magical Ranged");
                ret.name = "Basic Attack (Magical Ranged)";
                temp = attackData.damage*(1+0.05f*abilityPower)+attackData.damage*0.03f*magicalAttack;
                temp = MathF.Round(temp, 2);
                ret.damage = temp + " ("+attackData.damage+" base) Magical "+defaultElement.ToString();
                temp = attackData.manaCost * (1 + 0.05f * abilityPower);
                temp = MathF.Round(temp, 2);
                ret.cost = temp+" ("+attackData.manaCost+" base) Mana";
                ret.icon = Resources.Load<Sprite>("Icons/MagicalRanged");
                break;
        }
        ret.range = attackData.range+"";
        ret.desc = attackData.description;
        ret.abilityType = "Attack";
        ret.targetType = "Single Enemy";
        ret.special = "";
        ret.apEffect = "+5% damage and cost per AP";
        ret.levelEffect = "";
        return ret;
    }

    public class AttackMessageToTarget
    {
        public UnitBase source;

        public UnitBase target;

        public Attack sourceAttack;
    
        public AttackData.DamageType damageType;
    
        public AttackData.Element damageElement;

        public float baseDamage;

        public float damage;
    
        public List<string> effectsToApplyTarget;

        public List<int> stacksToApply;

        public AttackMessageToTarget(AttackData defaultData, Attack att)
        {
            damageType = defaultData.damageType;
            damageElement = defaultData.damageElement;
            baseDamage = defaultData.damage;
            damage = defaultData.damage;
            effectsToApplyTarget = new List<string>(defaultData.effectsToApplyTarget);
            stacksToApply = new List<int>(defaultData.baseStacksToApplyToTarget);
            sourceAttack = att;
        }
    }
}
