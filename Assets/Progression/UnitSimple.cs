using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSimple : IEquatable<UnitSimple>, IComparable<UnitSimple>
{
    public string name;

    public string id;

    public string nickname;

    public int level;

    public int currentExp;

    public int availableUpgradePoints;

    [XmlIgnore][NonSerialized]public Dictionary<ActiveAbility.ActiveAbilityDes, int> activeAbilities;

    [XmlIgnore][NonSerialized]public Dictionary<PassiveAbility.PassiveAbilityDes, int> passiveAbilities;

    public List<string> acquiredUpgrades;

    public StatGrades statGrades;

    public float ExpToNextLevel()
    {
        return ExpToNextLevel(level);
    }

    public static float ExpToNextLevel(int level)
    {
        if (level == 1)
            return 100;
        return ExpToNextLevel(level-1) + 100 * 1.5f * (level - 1);
    }

    public UnitSimple()
    {
        statGrades = new StatGrades();
    }

    public UnitSimple(string name, string id, int level, StatGrades statGrades)
    {
        this.name = name;
        nickname = this.name.FirstCharacterToUpper();
        this.id = id;
        this.level = level;
        this.statGrades = statGrades;
        UnitData myData = GetMyUnitData();
        acquiredUpgrades = new List<string>();
        if (myData.superRoot != null)
        {
            for (int i = 0; i < myData.superRoot.branches.Count; i++)
            {
                acquiredUpgrades.Add(i.ToString());
            }
        }
        InitAbilities();
    }
    
    public void UnlockUpgrade(string upgradeStr, UnitData myData = null, bool unlock = true)
    {
        if(myData == null)
            myData = GetMyUnitData();
        char[] chars = upgradeStr.ToCharArray();
        UpgradeTreeNode node = myData.superRoot;
        for (int i = 0; i < chars.Length; i++)
        {
            int ind = int.Parse(chars[i].ToString());
            if (node.branches.Count <= ind)
            {
                Debug.LogError("Unit got invalid upgrade string");
            }
            node = node.branches[ind];
        }
        foreach (ActiveAbility.ActiveAbilityDes active in node.activeGrant)
        {
            if (activeAbilities.ContainsKey(active))
            {
                activeAbilities[active]++;
            }
            else
            {
                activeAbilities.Add(active, 1);
            }
        }
        foreach (PassiveAbility.PassiveAbilityDes passive in node.passiveGrant)
        {
            if (passiveAbilities.ContainsKey(passive))
            {
                passiveAbilities[passive]++;
            }
            else
            {
                passiveAbilities.Add(passive, 1);
            }
        }

        if (unlock && availableUpgradePoints > 0)
        {
            acquiredUpgrades.Add(upgradeStr);
            availableUpgradePoints--;
            UnitManager.ModUnit(this);
        }
    }

    public void InitAbilities()
    {
        activeAbilities = new Dictionary<ActiveAbility.ActiveAbilityDes, int>();
        passiveAbilities = new Dictionary<PassiveAbility.PassiveAbilityDes, int>();
        UnitData myData = GetMyUnitData();
        foreach (ActiveAbility.ActiveAbilityDes active in myData.baseActiveAbilities)
        {
            activeAbilities.Add(active, 1);
        }
        foreach (PassiveAbility.PassiveAbilityDes passive in myData.basePassiveAbilities)
        {
            passiveAbilities.Add(passive, 1);
        }
        foreach (string upgradeStr in acquiredUpgrades)
        {
            UnlockUpgrade(upgradeStr, myData, false);
        }
    }

    public UnitData GetMyUnitData()
    {
        return Resources.Load<UnitData>("UnitData/"+name);
    }
    
    public bool Equals(UnitSimple other)
    {
        if (other == null) return false;
        return id.Equals(other.id);
    }

    public int CompareTo(UnitSimple other)
    {
        if (name.Equals(other.name))
        {
            return level.CompareTo(other.level);
        }
        return name.CompareTo(other.name);
    }
}
