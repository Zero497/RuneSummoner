using UnityEngine;

public class Resilient : PassiveAbility
{
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        source.myCombatStats.AddPhysicalDefense(source.myCombatStats.getPhysicalDefense(true)*0.5f*level);
    }
}
