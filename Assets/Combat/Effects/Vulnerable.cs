using UnityEngine;

public class Vulnerable : Effect
{
    private ActionPriorityWrapper<UnitBase> onTurnStarted;

    private ActionPriorityWrapper<UnitBase, float, Float> onIncomingDamage;

    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onTurnStarted = new ActionPriorityWrapper<UnitBase>();
        onTurnStarted.priority = 6;
        onTurnStarted.action = OnTurnStarted;
        source.myEvents.onTurnStarted.Subscribe(onTurnStarted);
        onIncomingDamage = new ActionPriorityWrapper<UnitBase, float, Float>();
        onIncomingDamage.priority = 120;
        onIncomingDamage.action = OnIncomingDamage;
        source.myEvents.modifyIncomingDamageAfterDef.Subscribe(onIncomingDamage);
    }

    public override void RemoveEffect()
    {
        source.myEvents.onTurnStarted.Unsubscribe(onTurnStarted);
        source.myEvents.modifyIncomingDamageAfterDef.Unsubscribe(onIncomingDamage);
        base.RemoveEffect();
    }

    private void OnTurnStarted(UnitBase myUnit)
    {
        this.RemoveEffect();
    }

    private void OnIncomingDamage(UnitBase myUnit, float initdamage, Float damage)
    {
        damage.flt *= 1 + 0.1f * stacks;
    }
}
