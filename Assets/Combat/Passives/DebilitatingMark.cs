using UnityEngine;

public class DebilitatingMark : PassiveAbility
{
    public override string GetAbilityName()
    {
        return "Debilitating Mark";
    }
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Debilitating Mark\n";
        ret += 
            "Units Marked by this Unit deal "+(level*25)+"% (25% base) less damage with attacks.\n";
        ret += "Level Effect: +25% damage reduction per Level.\n";
        return ret;
    }
}
