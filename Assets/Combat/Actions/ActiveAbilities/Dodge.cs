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
}
