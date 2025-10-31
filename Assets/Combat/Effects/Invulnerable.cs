using UnityEngine;

public class Invulnerable : Effect
{
    private ActionPriorityWrapper<UnitBase, float, Float> modIncomingDamage;

    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        modIncomingDamage = new ActionPriorityWrapper<UnitBase, float, Float>();
        modIncomingDamage.priority = 200;
        modIncomingDamage.action = ModIncomingDamage;
        source.myEvents.modifyIncomingDamageAfterDef.Subscribe(modIncomingDamage);
    }

    public override void RemoveEffect()
    {
        source.myEvents.modifyIncomingDamageAfterDef.Unsubscribe(modIncomingDamage);
        base.RemoveEffect();
    }

    private void ModIncomingDamage(UnitBase myUnit, float baseDamage, Float finalDamage)
    {
        finalDamage.flt = 0;
    }
}
