using System.Collections.Generic;
using UnityEngine;


public static class AbilityFactory 
{
    public static ActiveAbility getActiveAbility(string id, UnitBase unit)
    {
        ActiveAbility ability = null;
        SendData data = new SendData(unit);
        data.AddStr(id);
        switch (id)
        {
            case "PhysicalMelee":
            case "PhysicalRanged":
            case "MagicalMelee":
            case "MagicalRanged":
                ability = new Attack();
                break;
            case "BlitzPhysical":
                ability = new AttackBlitz();
                data.AddStr("physical");
                break;
            case "BlitzMagical":
                ability = new AttackBlitz();
                data.AddStr("magical");
                break;
        }
        if (ability == null) return null;
        
        ability.Initialize(data);
        return ability;
    }
}
