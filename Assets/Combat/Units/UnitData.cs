using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    public string unitDescription;
    
    public Element myElement;

    public CombatType myType;
    
    public float summonCost;
    
    public float health;
    public float healthPerLevel;
    
    public int initiative;
    
    public float abilityPower;
    public float abilityPowerPerLevel;

    public float magicalAttack;
    public float magicalAttackPerLevel;
    
    public float physicalAttack;
    public float physicalAttackPerLevel;
    
    public float magicalDefence;
    public float magicalDefencePerLevel;
    
    public float physicalDefence;
    public float physicalDefencePerLevel;

    public float mana;
    public float manaPerLevel;

    public float manaRegen;
    public float manaRegenPerLevel;
    
    public float sightRadius;

    public float speed;

    public float stamina;
    public float staminaPerLevel;
    
    public float staminaRegen;
    public float staminaRegenPerLevel;
    
    public List<string> abilities;
    
    public Sprite portrait;

    public FSMNode defaultEntryState;

    public UpgradeTreeNode uniqueTree;
    
    public enum Element
    {
        none,
        beast,
        human,
        machine,
    }

    public enum CombatType
    {
        balanced,
        fastAttack,
        tank
    }

    public UpgradeTreeNode GetElementTree()
    {
        return Resources.Load<UpgradeTreeNode>("ElementTrees/"+myElement.ToString());
    }

    public UpgradeTreeNode GetCombatTree()
    {
        return Resources.Load<UpgradeTreeNode>("CombatTrees/" + myType.ToString());
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
