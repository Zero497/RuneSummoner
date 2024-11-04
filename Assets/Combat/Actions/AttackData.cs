using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData : ScriptableObject
{
    public AbilityData MyAbilityData;
    
    public DamageType damageType;
    
    public UnitData.Element element;

    public float damage;

    public string effectToApplySelf;
    
    public string effectToApplyTarget;
    
    public bool useUnitElement;

    public bool useWeaponElement;

    public enum DamageType
    {
        Physical,
        Magic,
        True
    }
}
