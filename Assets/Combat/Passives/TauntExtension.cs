using UnityEngine;

public class TauntExtension : PassiveAbility
{
    public override string GetAbilityName()
    {
        return "Taunt Extension";
    }
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Taunt Extension";
        ret.desc = 
            "Increases the AOE Range of this Unit's Taunt ability by "+(3*level)+" (3 base).";
        ret.levelEffect = "+3 AOE Range per Level.";
        return ret;
    }
}
