using UnityEngine;

public class Farseer : PassiveAbility
{
    /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        source.myCombatStats.AddSightRadius(source.myCombatStats.getSightRadius(true)*0.5f*level);
    }
}
