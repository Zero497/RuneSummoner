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
    
    public List<string> baseActiveAbilities;

    public List<string> basePassiveAbilities;
    
    public Sprite portrait;

    public Sprite UnitSprite;

    public FSMNode defaultEntryState;

    public UpgradeTreeNode uniqueTree;

    public AttackData.Element DefaultDamageElement;

    public Grade myGrade;

    public enum Grade
    {
        poor,
        common,
        normal,
        rare,
        epic,
        legendary
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
        return Resources.Load<UpgradeTreeNode>("ElementTrees/"+myUnitType.ToString());
    }

    public UpgradeTreeNode GetCombatTree()
    {
        return Resources.Load<UpgradeTreeNode>("CombatTrees/" + myCombatType.ToString());
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
