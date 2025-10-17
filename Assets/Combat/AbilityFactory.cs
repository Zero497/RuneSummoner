using System.Collections.Generic;
using UnityEngine;


public static class AbilityFactory 
{
    public static ActiveAbility getActiveAbility(string id, UnitBase unit, int level = 1)
    {
        ActiveAbility ability = null;
        SendData data = new SendData(unit);
        data.AddStr(id);
        data.AddFloat(level);
        switch (id)
        {
            case "PhysicalMelee":
            case "PhysicalRanged":
            case "MagicalMelee":
            case "MagicalRanged":
                ability = new Attack();
                break;
            default:
                return null;
        }
        
        ability.Initialize(data);
        return ability;
    }

    public static PassiveAbility getPassiveAbility(string id, UnitBase unit, int level = 1)
    {
        SendData data = new SendData(unit);
        data.AddStr(id);
        data.AddFloat(level);
        PassiveAbility ability = null;
        switch (id)
        {
            case "adaptable":
                ability = new Adaptable();
                break;
            default:
                return null;
        }
        
        ability.Initialize(data);
        return ability;
    }
}
