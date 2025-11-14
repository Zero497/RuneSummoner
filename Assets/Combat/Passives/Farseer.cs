using UnityEngine;

public class Farseer : PassiveAbility
{
    /*
        Expects:
            Int 0: des of passive ability
            Unit 0: unit to apply to
            Int 0: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        source.myCombatStats.AddSightRadius(source.myCombatStats.getSightRadius(true)*0.5f*level);
    }
    
    public override string GetAbilityName()
    {
        return "Farseer";
    }
}
