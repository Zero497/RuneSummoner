using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityData : ScriptableObject
{
    public bool isFree;
    
    public int range;

    public int aoeRange;
    
    public TargetType targetType;
    
    public string description;
    
    public enum TargetType
    {
        singleTargetEnemy = 0,
        multiTargetEnemy = 1,
        singleTargetFriendly = 2,
        multiTargetFriendly = 3,
        singleTargetNeutral = 4,
        multiTargetNeutral = 5,
        aoeEnemyOnly = 6,
        aoeFriendlyOnly = 7,
        aoeNeutral = 8,
        self = 9
    }
}
