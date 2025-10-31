using UnityEngine;

public class Spikes : Effect
{
    private int dtype;

    private int element;

    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttacked;

    /*
        Expects:
            Unit 0: unit to apply to
            String 0: effect name
            Float 0: stacks to apply
            Float 1: damage type (enum as int)
            Float 2: damage element (enum as int)
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onAttacked = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        onAttacked.priority = 48;
        onAttacked.action = OnAttacked;
        source.myEvents.onAttacked.Subscribe(onAttacked);
    }

    public int GetDtype()
    {
        return dtype;
    }

    public int GetElement()
    {
        return element;
    }

    public override void RemoveEffect()
    {
        source.myEvents.onAttacked.Unsubscribe(onAttacked);
        base.RemoveEffect();
    }

    public override bool Equals(Effect other)
    {
        if (base.Equals(other))
        {
            Spikes otherA = (Spikes)other;
            return (otherA.GetDtype() == dtype && otherA.GetElement() == element);
        }
        return false;
    }

    /*
        Expects:
            String 0: effect name
            Float 1: damage type (enum as int)
            Float 2: damage element (enum as int)
     */
    public override bool Equals(SendData data)
    {
        if (base.Equals(data))
        {
            return (Mathf.FloorToInt(data.floatData[1]) == dtype && Mathf.FloorToInt(data.floatData[2]) == element);
        }
        return false;
    }

    private void OnAttacked(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        if (HexTileUtility.AreAdjacent(myUnit.currentPosition, attack.source.currentPosition))
        {
            attack.source.TakeDamage((AttackData.DamageType) dtype, (AttackData.Element) element, stacks);
        }
    }
}
