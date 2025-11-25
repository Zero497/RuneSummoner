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
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Farseer\n";
        ret += 
            "Increases Sight Radius by "+(50*level)+"% (50% base) (rounded down)\n";
        ret += "Level Effect: +50% Sight Radius per Level.\n";
        return ret;
    }
}
