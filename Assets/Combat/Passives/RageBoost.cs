using UnityEngine;

public class RageBoost : PassiveAbility
{
    public override string GetAbilityName()
    {
        return "Rage Boost";
    }
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Rage Boost";
        ret.desc = 
            "When this Unit's Rage ability triggers, it also gains Ability Power Up "+(2*level)+" (base 2) per 5 stacks of Physical/Magical Attack Up it gains. When this Unit's turn begins, if it hasn't taken damage since its last turn, it loses all stacks of Ability Power Up.";
        ret.levelEffect = "+2 Ability Power Up applied per Level.";
        return ret;
    }
}
