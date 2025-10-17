using UnityEngine;

public class Observer : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase> onTurnStarted;

    private ActionPriorityWrapper<UnitBase> onTurnEnded;

    /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        OnTurnEnded(source);
        onTurnStarted = new ActionPriorityWrapper<UnitBase>();
        onTurnStarted.priority = 64;
        onTurnStarted.action = OnTurnStarted;
        source.myEvents.onTurnStarted.Subscribe(onTurnStarted);
        onTurnEnded = new ActionPriorityWrapper<UnitBase>();
        onTurnEnded.priority = 64;
        onTurnEnded.action = OnTurnEnded;
        source.myEvents.onTurnEnded.Subscribe(onTurnEnded);
    }

    private void OnTurnStarted(UnitBase myUnit)
    {
        myUnit.myCombatStats.AddSightRadius(-myUnit.myCombatStats.getSightRadius(true)*level);
    }

    private void OnTurnEnded(UnitBase myUnit)
    {
        myUnit.myCombatStats.AddSightRadius(myUnit.myCombatStats.getSightRadius(true)*level);
    }
}
