using System;
using System.Collections.Generic;
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

    public List<string> activeAbilities;

    public List<string> passiveAbilities;

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
        activeAbilities = new List<string>();
        passiveAbilities = new List<string>();
    }

    public UnitSimple(string name, string id, int level, StatGrades statGrades)
    {
        this.name = name;
        nickname = this.name.FirstCharacterToUpper();
        this.id = id;
        this.level = level;
        this.statGrades = statGrades;
        activeAbilities = new List<string>();
        passiveAbilities = new List<string>();
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
