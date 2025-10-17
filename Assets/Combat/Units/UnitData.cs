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
    
    public Sprite portrait;

    public FSMNode defaultEntryState;

    public UpgradeTreeNode uniqueTree;

    public Element defaultDamageElement;
    
    public enum Element
    {
        none,
        beastial,
        humanoid,
        construct,
    }

    public Element strToElement(string str)
    {
        str = str.ToLower();
        switch (str)
        {
            case "beastial":
                return Element.beastial;
            case "humanoid":
                return Element.humanoid;
            case "construct":
                return Element.construct;
        }
        return Element.none;
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
