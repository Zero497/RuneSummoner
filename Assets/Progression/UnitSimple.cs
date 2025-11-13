using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitSimple : IEquatable<UnitSimple>, IComparable<UnitSimple>
{
    public string name;

    public string id;

    public int level;

    public int currentExp;

    public int availableUpgradePoints;

    public List<string> activeAbilities;

    public List<string> passiveAbilities;

    public StatGrades statGrades;

    public UnitSimple()
    {
        statGrades = new StatGrades();
        activeAbilities = new List<string>();
        passiveAbilities = new List<string>();
    }

    public UnitSimple(string name, string id, int level, StatGrades statGrades)
    {
        this.name = name;
        this.id = id;
        this.level = level;
        this.statGrades = statGrades;
        activeAbilities = new List<string>();
        passiveAbilities = new List<string>();
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
