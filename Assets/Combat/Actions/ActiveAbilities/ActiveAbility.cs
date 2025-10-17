using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : UnitAction
{
    public int level;

    public override void Initialize(SendData sendData)
    {
        base.Initialize(sendData);
        level = (int)sendData.floatData[0];
    }
    
    public override Float GetStaminaCost(bool getBase = false)
    {
        if (getBase) return base.GetStaminaCost();
        return new Float(abilityData.staminaCost * (1 + 0.05f * source.abilityPower));
    }
    
    public override Float GetManaCost(bool getBase = false)
    {
        if (getBase) return base.GetStaminaCost();
        return new Float(abilityData.manaCost * (1 + 0.05f * source.abilityPower));
    }
    
    public Func<int, bool> getValidTargets()
    {
        switch (abilityData.targetType)
        {
            case AbilityData.TargetType.singleTargetEnemy:
            case AbilityData.TargetType.multiTargetEnemy:
            case AbilityData.TargetType.aoeEnemyOnly:
                return i => i != source.myTeam;
            case AbilityData.TargetType.singleTargetFriendly:
            case AbilityData.TargetType.multiTargetFriendly:
            case AbilityData.TargetType.aoeFriendlyOnly:
                return i => i == source.myTeam;
            case AbilityData.TargetType.singleTargetNeutral:
            case AbilityData.TargetType.multiTargetNeutral:
            case AbilityData.TargetType.aoeNeutral:
                return i => true; 
        }
        return null;
    }
}
