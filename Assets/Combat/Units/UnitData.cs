using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    public string unitDescription;
    
    public UnitType myUnitType;

    [FormerlySerializedAs("myType")] public CombatType myCombatType;
    
    public float summonCost;
    
    public float health;
    
    public int initiative;
    
    public float abilityPower;

    public float magicalAttack;
    
    public float physicalAttack;
    
    public float magicalDefence;
    
    public float physicalDefence;

    public float mana;

    public float manaRegen;
    
    public int sightRadius;

    public int speed;

    public float stamina;
    
    public float staminaRegen;
    
    public List<ActiveAbility.ActiveAbilityDes> baseActiveAbilities;

    public List<PassiveAbility.PassiveAbilityDes> basePassiveAbilities;
    
    public Sprite portrait;

    public Sprite UnitSprite;

    public FSMNode defaultEntryState;

    public UpgradeTreeNode superRoot;

    public AttackData.Element DefaultDamageElement;

    public Grade myGrade;

    public float GetStatValue(Stat stat)
    {
        switch (stat)
        {
            case Stat.health:
                return health;
            case Stat.initiative:
                return initiative;
            case Stat.mana:
                return mana;
            case Stat.speed:
                return speed;
            case Stat.stamina:
                return stamina;
            case Stat.abilityPower:
                return abilityPower;
            case Stat.magicalAttack:
                return magicalAttack;
            case Stat.magicalDefense:
                return magicalDefence;
            case Stat.manaRegen:
                return manaRegen;
            case Stat.physicalAttack:
                return physicalAttack;
            case Stat.physicalDefense:
                return physicalDefence;
            case Stat.sightRadius:
                return sightRadius;
            case Stat.staminaRegen:
                return staminaRegen;
            default:
                return 0;
        }
    }

    public enum Stat
    {
        health,
        currentHealth,
        mana,
        currentMana,
        manaRegen,
        stamina,
        currentStamina,
        staminaRegen,
        abilityPower,
        physicalAttack,
        physicalDefense,
        magicalAttack,
        magicalDefense,
        initiative,
        speed,
        sightRadius
    }

    public enum Grade
    {
        poor,
        common,
        normal,
        rare,
        epic,
        legendary
    }

    public static string GradeToColorString(Grade grade)
    {
        string word = "";
        string color = "";
        switch (grade)
        {
            case Grade.poor:
                word = "Poor";
                color = "6B6B6B";
                break;
            case Grade.common:
                word = "Common";
                color = "D9DCE3";
                break;
            case Grade.normal:
                word = "Normal";
                color = "4FB36A";
                break;
            case Grade.rare:
                word = "Rare";
                color = "3A7BFF";
                break;
            case Grade.epic:
                word = "Epic";
                color = "A25CFF";
                break;
            case Grade.legendary:
                word = "Legendary";
                color = "FF9A1F";
                break;
            default:
                return "";
        }
        return "<color=#"+color+">"+word+"</color>";
    }
    
    public static string GradeToColor(Grade grade)
    {
        string color = "";
        switch (grade)
        {
            case Grade.poor:
                color = "6B6B6B";
                break;
            case Grade.common:
                color = "D9DCE3";
                break;
            case Grade.normal:
                color = "4FB36A";
                break;
            case Grade.rare:
                color = "3A7BFF";
                break;
            case Grade.epic:
                color = "A25CFF";
                break;
            case Grade.legendary:
                color = "FF9A1F";
                break;
            default:
                return "";
        }
        return "<color=#"+color+">";
    }
    
    public enum UnitType
    {
        none,
        aetheric,
        angelic,
        beastial,
        construct,
        demonic,
        draconic,
        aero,
        aqua,
        cryo,
        electro,
        pyro,
        terra,
        humanoid,
        undead
    }

    public UnitType strToElement(string str)
    {
        str = str.ToLower();
        switch (str)
        {
            case "beastial":
                return UnitType.beastial;
            case "aetheric":
                return UnitType.aetheric;
            case "angelic":
                return UnitType.angelic;
            case "construct":
                return UnitType.construct;
            case "demonic":
                return UnitType.demonic;
            case "draconic":
                return UnitType.draconic;
            case "aero":
                return UnitType.aero;
            case "aqua":
                return UnitType.aqua;
            case "cryo":
                return UnitType.cryo;
            case "electro":
                return UnitType.electro;
            case "pyro":
                return UnitType.pyro;
            case "terra":
                return UnitType.terra;
            case "humanoid":
                return UnitType.humanoid;
            case "undead":
                return UnitType.undead;
        }
        return UnitType.none;
    }

    public enum CombatType
    {
        balanced,
        scout,
        tank
    }

    public UpgradeTreeNode GetElementTree()
    {
        return Resources.Load<UpgradeTreeNode>("ElementTrees/"+myUnitType.ToString()+"R");
    }

    public UpgradeTreeNode GetCombatTree()
    {
        return Resources.Load<UpgradeTreeNode>("CombatTrees/" + myCombatType.ToString()+"R");
    }
    
    public UpgradeTreeNode GetUniqueTree()
    {
        return Resources.Load<UpgradeTreeNode>("UnitTrees/" + name.ToString()+"R");
    }

    public static int CompareByInitiative(UnitData item1, UnitData item2)
    {
        if (item1 == null && item2 == null) return 0;
        if (item1 == null) return -1;
        if (item2 == null) return 1;
        if (item1.initiative < item2.initiative) return -1;
        if (item1.initiative > item2.initiative) return 1;
        return 0;
    }

    public static UnitData GetUnitData(string name)
    {
        return Resources.Load<UnitData>(name);
    }
}
