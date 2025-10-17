using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//VALID IDS: PhysicalMelee, PhysicalRanged, MagicalMelee, MagicalRanged
public class Attack : ActiveAbility
{
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
    }

    protected virtual float ApplyAbilityPower(float damage)
    {
        return damage * (1 + 0.05f * source.abilityPower);
    }

    private AttackMessageToTarget PrepareMessage(UnitBase target, float mod=1)
    {
        AttackMessageToTarget retval = new AttackMessageToTarget(abilityData as AttackData, this);
        retval.source = source;
        retval.target = target;
        if ((abilityData as AttackData).useUnitElement)
        {
            retval.element = source.baseData.defaultDamageElement;
        }
        float mult = 1;
        if (retval.damageType == AttackData.DamageType.Magic)
        {
            mult += source.magicalAttack / 100;
        }
        else if (retval.damageType == AttackData.DamageType.Physical)
        {
            mult += source.physicalAttack / 100;
        }
        retval.damage = ApplyAbilityPower(retval.damage);
        retval.damage *= mult * mod;
        source.ModifyOutgoingAttack(retval);
        return retval;
    }

    public override bool PrepAction()
    {
        OverlayManager.instance.ClearOverlays();
        if (!prepped)
        {
            OverlayManager.instance.CreateOverlay(HexTileUtility.DjikstrasGetTilesInRange(TurnController.controller.mainMap, source.currentPosition, (abilityData as AttackData).range, 1),"AttackOverlay");
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

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    private string ParseDescription(string description)
    {
        throw new System.NotImplementedException();
    }

    public override void Initialize(SendData sendData)
    {
        id = sendData.strData[0];
        source = sendData.unitData[0];
        abilityData = Resources.Load<AbilityData>("AttackData/"+sendData.strData[1]);
        level = (int)sendData.floatData[0];
    }

    public class AttackMessageToTarget
    {
        public UnitBase source;

        public UnitBase target;

        public Attack sourceAttack;
    
        public AttackData.DamageType damageType;
    
        public UnitData.Element element;

        public float baseDamage;

        public float damage;
    
        public List<string> effectsToApplyTarget;

        public List<int> stacksToApply;

        public AttackMessageToTarget(AttackData defaultData, Attack att)
        {
            damageType = defaultData.damageType;
            element = defaultData.element;
            baseDamage = defaultData.damage;
            damage = defaultData.damage;
            sourceAttack = att;
        }
    }
}
