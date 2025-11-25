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
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Resilient\n";
        ret += 
            "Increases Physical Defense by "+(50*level)+"% (50% base).\n";
        ret += "Level Effect: +50% Physical Defense per Level.\n";
        return ret;
    }
}
