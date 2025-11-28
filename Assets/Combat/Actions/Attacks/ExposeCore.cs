using System;
using UnityEngine;

public class ExposeCore : Attack
{
    protected override AttackMessageToTarget PrepareMessage(UnitBase target, float mod = 1)
    {
        AttackMessageToTarget msg = base.PrepareMessage(target, mod);
        msg.stacksToApply[0] += 5 * (level - 1);
        return msg;
    }

    public override float GetRange(bool getBase = false)
    {
        if(getBase)
            return base.GetRange(getBase);
        return base.GetRange(getBase) + (level - 1);
    }

    public static AbilityText GetAbilityText(int level, float abilityPower, float magicalAttack)
    {
        AbilityText ret = new AbilityText();
        AttackData attackData = Resources.Load<AttackData>("AttackData/Expose Core");
        ret.isAttack = true;
        ret.name = "Expose Core";
        ret.desc = attackData.description;
        ret.abilityType = "Attack";
        ret.range = (attackData.range+(level-1)).ToString();
        float temp = attackData.damage*(1+0.05f*abilityPower)+attackData.damage*Attack.physMagAttBonus*magicalAttack;
        temp = MathF.Round(temp, 2);
        ret.damage = temp + "("+attackData.damage+" base) Magical Electro";
        ret.cost = attackData.manaCost+" Mana";
        ret.targetType = "Single Enemy";
        ret.special = "The target gains Shocked "+(5*level)+" (5 base). This Unit gains Vulnerable 3.";
        ret.apEffect = "+5% damage and cost per AP";
        ret.levelEffect = "+1 Range per Level +5 Shocked applied per Level";
        ret.icon = Resources.Load<Sprite>("Icons/ExposeCore");
        return ret;
    }
}
