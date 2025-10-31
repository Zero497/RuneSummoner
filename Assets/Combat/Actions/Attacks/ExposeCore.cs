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
}
