using UnityEngine;

public class LastingMark : PassiveAbility
{
    public override string GetAbilityName()
    {
        return "Lasting Mark";
    }
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Lasting Mark\n";
        ret += 
            "When Mark stacks this Unit applied would be removed by an attack, only 50% as many Mark stacks (rounded up) are removed.\n";
        ret += "Level Effect: N/A\n";
        return ret;
    }
}
