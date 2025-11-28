using UnityEngine;

public class DebilitatingMark : PassiveAbility
{
    public override string GetAbilityName()
    {
        return "Debilitating Mark";
    }
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Debilitating Mark";
        ret.desc = 
            "Units Marked by this Unit deal "+(level*25)+"% (25% base) less damage with attacks.";
        ret.levelEffect = "+25% damage reduction per Level.";
        return ret;
    }
}
