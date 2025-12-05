using System;
using UnityEngine;

public class Sprint : ActiveAbility
{ 
    public override bool RunAction(SendData actionData)
    {
        if (source.PayCost(this, false) && !usedThisTurn)
        {
            source.PayCost(this);
            source.moveRemaining += source.speed*(1+0.05f*source.abilityPower);
            usedThisTurn = true;
            return true;
        }
        return false;
    }

    public override Float GetStaminaCost(bool getBase = false)
    {
        return new Float(Mathf.Max(base.GetStaminaCost(getBase).flt*(1.2f-0.2f*level), 0));
    }

    public override string GetID()
    {
        return "Sprint";
    }

    public override bool PrepAction()
    {
        return RunAction(new SendData(""));
    }

    public override bool RushCompletion()
    {
        throw new System.NotImplementedException();
    }
    
    public static AbilityText GetAbilityText(int level, float abilityPower)
    {
        AbilityText ret = new AbilityText();
        AbilityData abData = Resources.Load<AbilityData>("AbilityData/Sprint");
        ret.name = "Sprint";
        ret.desc = abData.description;
        ret.abilityType = "Support";
        ret.range = "Self";
        float temp = abData.staminaCost*(1+0.05f*abilityPower)*(1.2f-0.2f*level);
        temp = MathF.Round(temp, 2);
        ret.cost = temp + " ("+abData.staminaCost+" base) Stamina";
        ret.targetType = "Self";
        ret.special =
            "Free action. Increase Speed this turn by "+(100+5*abilityPower)+"% (100% base).";
        ret.apEffect = "Speed increase raised by 5% (additive, rounds down) and cost increased by 5%.";
        ret.levelEffect = "Stamina cost reduced by 20% per Level, after AP increases.";
        ret.icon = Resources.Load<Sprite>("Icons/Sprint");
        return ret;
    }
}
