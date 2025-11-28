using UnityEngine;

public class Resilient : PassiveAbility
{
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        source.myCombatStats.AddPhysicalDefense(source.myCombatStats.getPhysicalDefense(true)*0.5f*level);
    }
    
    public override string GetAbilityName()
    {
        return "Resilient";
    }
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Resilient";
        ret.desc = 
            "Increases Physical Defense by "+(50*level)+"% (50% base).";
        ret.levelEffect = "+50% Physical Defense per Level.";
        return ret;
    }
}
