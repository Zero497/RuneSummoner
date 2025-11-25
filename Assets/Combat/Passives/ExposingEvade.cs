using UnityEngine;

public class ExposingEvade : PassiveAbility
{
    public override string GetAbilityName()
    {
        return "Exposing Evade";
    }
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Exposing Evade\n";
        ret += 
            "When this Unit avoids damage from an attack due to Evade, it applies Marked "+(3*level)+" (3 base) to the attacker.\n";
        ret += "Level Effect: +3 Marked applied per Level.\n";
        return ret;
    }
}
