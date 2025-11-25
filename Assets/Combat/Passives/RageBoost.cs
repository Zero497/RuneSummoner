using UnityEngine;

public class RageBoost : PassiveAbility
{
    public override string GetAbilityName()
    {
        return "Rage Boost";
    }
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Rage Boost\n";
        ret += 
            "When this Unit's Rage ability triggers, it also gains Ability Power Up "+(2*level)+" (base 2) per 5 stacks of Physical/Magical Attack Up it gains. When this Unit's turn begins, if it hasn't taken damage since its last turn, it loses all stacks of Ability Power Up.\n";
        ret += "Level Effect: +2 Ability Power Up applied per Level.\n";
        return ret;
    }
}
