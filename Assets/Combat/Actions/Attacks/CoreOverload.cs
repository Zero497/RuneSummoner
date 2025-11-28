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
            mult += source.magicalAttack * 3 / 100;
        }
        else if (retval.damageType == AttackData.DamageType.Physical)
        {
            mult += source.physicalAttack * 3/ 100;
        }
        retval.baseDamage =  (0.5f + 0.25f * level)* manaToBePaid;
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

    public static AbilityText GetAbilityText(int level, float abilityPower, float magicalAttack)
    {
        AbilityText ret = new AbilityText();
        AbilityData abData = Resources.Load<AbilityData>("AttackData/Core Overload");
        ret.isAttack = true;
        ret.isAOE = true;
        ret.name = "Core Overload";
        ret.desc = abData.description;
        ret.abilityType = "Attack";
        ret.range = "Self";
        float temp = (0.5f + 0.25f * level)*(1+0.03f*magicalAttack);
        ret.damage = temp + "(0.5 base) Magical Electro per Mana Spent";
        ret.cost = "100% current Mana";
        ret.targetType = "AOE All";
        ret.aoeRange = (abData.aoeRange + Mathf.FloorToInt(abilityPower / 20))+" ("+abData.aoeRange+" base)";
        ret.special = "This Unit Dies";
        ret.apEffect = "+1 AOE range per 20 AP";
        ret.levelEffect = "+0.25 Magical Electro damage per Mana per Level";
        ret.icon = Resources.Load<Sprite>("Icons/CoreOverload");
        return ret;
    }
}
