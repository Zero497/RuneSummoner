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
    
    public Func<int, bool> getValidTargets()
    {
        switch (abilityData.targetType)
        {
            case AbilityData.TargetType.singleTargetEnemy:
            case AbilityData.TargetType.multiTargetEnemy:
            case AbilityData.TargetType.aoeEnemyOnly:
                return i => i != source.myTeam;
            case AbilityData.TargetType.singleTargetFriendly:
            case AbilityData.TargetType.multiTargetFriendly:
            case AbilityData.TargetType.aoeFriendlyOnly:
                return i => i == source.myTeam;
            case AbilityData.TargetType.singleTargetNeutral:
            case AbilityData.TargetType.multiTargetNeutral:
            case AbilityData.TargetType.aoeNeutral:
                return i => true; 
        }
        return null;
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
        AttackMessageToTarget outgoingAttack = PrepareMessage(mod);
        foreach (UnitBase unit in targets)
        {
            unit.ReceiveAttack(outgoingAttack);
        }
    }

    private AttackMessageToTarget PrepareMessage(float mod=1)
    {
        AttackMessageToTarget retval = new AttackMessageToTarget(abilityData as AttackData);
        if ((abilityData as AttackData).useUnitElement)
        {
            retval.element = source.myElement;
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
            return false;
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
        abilityData = Resources.Load<AbilityData>("AttackData/"+sendData.strData[0]);
    }

    public class AttackMessageToTarget
    {
        public AbilityData MyAbilityData;
    
        public AttackData.DamageType damageType;
    
        public UnitData.Element element;

        public float damage;
    
        public Effect effectToApplyTarget;

        public AttackMessageToTarget(AttackData defaultData)
        {
            damageType = defaultData.damageType;
            element = defaultData.element;
            damage = defaultData.damage;
            effectToApplyTarget = Effect.GetEffect(defaultData.effectToApplyTarget);
        }
    }
}
