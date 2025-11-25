using UnityEngine;

public class Dodge : ActiveAbility
{ 
    public override bool RunAction(SendData actionData)
    {
        if (source.usedAbilityThisTurn) return false;
        if (!source.PayCost(this, false)) return false;
        SendData evadeData = new SendData(source);
        evadeData.AddStr("evade");
        evadeData.AddFloat(6+4*level);
        source.AddEffect(evadeData);
        return true;
    }

    public override Float GetStaminaCost(bool getBase = false)
    {
        return new Float(abilityData.staminaCost - abilityData.staminaCost * source.abilityPower * 0.01f);
    }

    public override string GetID()
    {
        return "Dodge";
    }

    public override bool PrepAction()
    {
        RunAction(new SendData(""));
        return true;
    }

    public override bool RushCompletion()
    {
        throw new System.NotImplementedException();
    }

    public static AbilityText GetAbilityText(int level, float abilityPower)
    {
        AbilityText ret = new AbilityText();
        AbilityData abData = Resources.Load<AbilityData>("AbilityData/Dodge");
        ret.name = "Dodge";
        ret.desc = abData.description;
        ret.abilityType = "Support";
        ret.range = "Self";
        float temp = abData.staminaCost*(1-0.01f*abilityPower);
        ret.cost = temp + " ("+abData.staminaCost+" base) Stamina";
        ret.targetType = "Self";
        ret.special =
            "Grants "+(6+4*level)+" (10 base) stacks of Evade.";
        ret.apEffect = "-1% Stamina Cost per AP";
        ret.levelEffect = "+4 Stacks of Evade per Level";
        return ret;
    }
}
