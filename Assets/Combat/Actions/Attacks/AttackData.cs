using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "AbilityData/AttackData")]
public class AttackData : AbilityData
{
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

    public static DamageType strToDType(string str)
    {
        str = str.ToLower();
        switch (str)
        {
            case "physical":
                return DamageType.Physical;
            case "magical":
                return DamageType.Magic;
        }

        return DamageType.True;
    }
}
