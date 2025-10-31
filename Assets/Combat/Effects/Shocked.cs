using UnityEngine;

public class Shocked : Effect
{
    private ActionPriorityWrapper<UnitBase, float, Float> modStamCost;

    private ActionPriorityWrapper<UnitBase, Float> onPayStam;

    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        modStamCost = new ActionPriorityWrapper<UnitBase, float, Float>();
        modStamCost.priority = 120;
        modStamCost.action = ModStamCost;
        source.myEvents.modStamCost.Subscribe(modStamCost);
        onPayStam = new ActionPriorityWrapper<UnitBase, Float>();
        onPayStam.priority = 72;
        onPayStam.action = OnPayStam;
        source.myEvents.onPayStam.Subscribe(onPayStam);
    }

    public override void RemoveEffect()
    {
        source.myEvents.modStamCost.Unsubscribe(modStamCost);
        source.myEvents.onPayStam.Unsubscribe(onPayStam);
        base.RemoveEffect();
    }

    private void ModStamCost(UnitBase myUnit, float originalCost, Float cost)
    {
        cost.flt *= 1 + 0.05f * stacks;
    }

    private void OnPayStam(UnitBase myUnit, Float stamPaid)
    {
        RemoveEffect();
    }
}
