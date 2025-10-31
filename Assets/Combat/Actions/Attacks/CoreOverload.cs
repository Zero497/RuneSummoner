using UnityEngine;

public class CoreOverload : Attack
{
    private float manaToBePaid = 0;
    
    public override bool RunAction(SendData sentData)
    {
        if (source.usedAbilityThisTurn) return false;
        float mod = 1;
        if (sentData.floatData.Count > 0) mod = sentData.floatData[0];
        manaToBePaid = source.currentMana;
        RunAOE(getValidTargets(), source.currentPosition, mod);
        source.PayCost(source.currentMana, false);
        source.usedAbilityThisTurn = true;
        source.Die();
        OverlayManager.instance.ClearOverlays();
        ClickManager.clickManager.SetAction(null);
        return true;
    }

    protected override float AbilityPowerBonus(float damage)
    {
        return 0;
    }

    public override float GetAOERange(bool getBase = false)
    {
        if(getBase)
            return base.GetAOERange(getBase);
        return base.GetAOERange(getBase) + Mathf.Floor(source.abilityPower / 20.0f);
    }

    protected override AttackMessageToTarget PrepareMessage(UnitBase target, float mod = 1)
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
            mult += source.magicalAttack / 100;
        }
        else if (retval.damageType == AttackData.DamageType.Physical)
        {
            mult += source.physicalAttack / 100;
        }
        retval.baseDamage = (0.5f + 0.25f * level) * manaToBePaid;
        retval.damage = retval.baseDamage;
        retval.baseDamage *= mult * mod;
        retval.damage *= mult * mod;
        source.ModifyOutgoingAttack(retval);
        return retval;
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
}
