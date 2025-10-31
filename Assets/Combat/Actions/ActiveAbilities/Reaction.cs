using UnityEngine;

public abstract class Reaction : ActiveAbility
{
    protected bool isActive;

    public override bool RunAction(SendData actionData)
    {
        isActive = !isActive;
        return true;
    }
}
