using UnityEngine;

public abstract class Reaction : ActiveAbility
{
    protected bool isActive;

    private ActionPriorityWrapper<UnitBase> onTurnStarted;

    public override void Initialize(SendData sendData)
    {
        base.Initialize(sendData);
        onTurnStarted = new ActionPriorityWrapper<UnitBase>();
        onTurnStarted.priority = 72;
        onTurnStarted.action = Reset;
        source.myEvents.onTurnStarted.Subscribe(onTurnStarted);
        
    }

    private void Reset(UnitBase unit)
    {
        isActive = false;
    }

    public override bool RunAction(SendData actionData)
    {
        isActive = !isActive;
        return true;
    }
}
