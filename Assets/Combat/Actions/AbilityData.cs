using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityData : ScriptableObject
{
    public int range;
    
    public TargetType targetType;
    
    public enum TargetType
    {
        singleTargetEnemy,
        multiTargetEnemy,
        singleTargetFriendly,
        multiTargetFriendly,
        singleTargetNeutral,
        multiTargetNeutral,
        aoeEnemyOnly,
        aoeFriendlyOnly,
        aoeNeutral,
        self
    }
}
