using UnityEngine;

public class ExposingEvade : PassiveAbility
{
    public override string GetAbilityName()
    {
        return "Exposing Evade";
    }
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Exposing Evade";
        ret.desc = 
            "When this Unit avoids damage from an attack due to Evade, it applies Marked "+(3*level)+" (3 base) to the attacker.";
        ret.levelEffect = "+3 Marked applied per Level.";
        return ret;
    }
}
