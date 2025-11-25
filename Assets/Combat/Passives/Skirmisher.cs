using UnityEngine;

public class Skirmisher : PassiveAbility
{
    /*
        Expects:
            Int 0: name of passive ability
            Unit 0: unit to apply to
            Int 1: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        source.myMovement = new SkirmisherMove();
    }
    
    public override string GetAbilityName()
    {
        return "Skirmisher";
    }
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Skirmisher\n";
        ret += 
            "This Unit can use any remaining movement after using an ability.\n";
        ret += "Level Effect: N/A\n";
        return ret;
    }
}
