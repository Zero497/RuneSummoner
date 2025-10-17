using UnityEngine;

public class Skirmisher : PassiveAbility
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
        source.myMovement = new SkirmisherMove();
    }
}
