using UnityEngine;

public class Sprint : ActiveAbility
{ 
    public override bool RunAction(SendData actionData)
    {
        if (source.PayCost(this))
        {
            source.moveRemaining += source.speed*(1+0.05f*source.abilityPower);
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
        throw new System.NotImplementedException();
    }

    public override bool RushCompletion()
    {
        throw new System.NotImplementedException();
    }
}
