using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackData", menuName = "AbilityData/AttackData")]
public class AttackData : AbilityData
{
    public DamageType damageType;
    
    public Element damageElement;

    public float damage;

    public List<string> effectsToApplySelf;

    public List<int> baseStacksToApplyToSelf;
    
    public List<string> effectsToApplyTarget;

    public List<int> baseStacksToApplyToTarget;
    
    public bool useUnitElement;

    public bool useWeaponElement;

    public enum DamageType
    {
        Physical,
        Magic,
        True
    }
    
    public enum Element {
        neutral,
        aero,
        aqua,
        cryo,
        decay,
        electro,
        poison,
        pyro,
        terra
    }

    public static Element strToElement(string str)
    {
        str = str.ToLower();
        switch (str)
        {
            case "aero":
                return Element.aero;
            case "aqua":
                return Element.aqua;
            case "cryo":
                return Element.cryo;
            case "decay":
                return Element.decay;
            case "electro":
                return Element.electro;
            case "poison":
                return Element.poison;
            case "pyro":
                return Element.pyro;
            case "terra":
                return Element.terra;
            default:
                return Element.neutral;
        }
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
