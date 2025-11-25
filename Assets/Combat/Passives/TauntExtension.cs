using UnityEngine;

public class TauntExtension : PassiveAbility
{
    public override string GetAbilityName()
    {
        return "Taunt Extension";
    }
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Taunt Extension\n";
        ret += 
            "Increases the AOE Range of this Unit's Taunt ability by "+(3*level)+" (3 base).\n";
        ret += "Level Effect: +3 AOE Range per Level.\n";
        return ret;
    }
}
