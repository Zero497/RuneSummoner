using UnityEngine;

public class LastingMark : PassiveAbility
{
    public override string GetAbilityName()
    {
        return "Lasting Mark";
    }
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Lasting Mark";
        ret.desc = 
            "When Mark stacks this Unit applied would be removed by an attack, only 50% as many Mark stacks (rounded up) are removed.";
        ret.levelEffect = "N/A";
        return ret;
    }
}
