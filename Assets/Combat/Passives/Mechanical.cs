using UnityEngine;

public class Mechanical : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase, Effect, int> onEffectIncoming;

    /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onEffectIncoming = new ActionPriorityWrapper<UnitBase, Effect, int>();
        onEffectIncoming.priority = 1;
        onEffectIncoming.action = OnEffectIncoming;
        source.myEvents.modifyIncomingEffect.Subscribe(onEffectIncoming);
    }

    private void OnEffectIncoming(UnitBase myUnit, Effect incomingEffect, int stacks)
    {
        if (incomingEffect.isBuff) return;
        if (stacks == 1)
        {
            float rand = Random.Range(0, 1);
            if (rand >= 0.3f + 0.2f * level)
            {
                incomingEffect.AddStacks(-1);
            }
        }
        else
        {
            int toRemove = Mathf.FloorToInt(stacks / (float)level);
            incomingEffect.AddStacks(-toRemove);
        }
    }
}
