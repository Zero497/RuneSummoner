using System;
using System.Collections.Generic;
using UnityEngine;

public class Battle : IComparable<Battle>
{
    public string zoneID;

    public Difficulty diff;

    public int level;

    public Vector2 mapSize;

    public List<UnitSimple> enemies;
    
    public enum Difficulty
    {
        novice,
        standard,
        heroic,
        mythic
    }

    public int CompareTo(Battle other)
    {
        if (zoneID.CompareTo(other.zoneID) != 0)
            return zoneID.CompareTo(other.zoneID);
        if (level < other.level) return -1;
        if (level > other.level) return 1;
        if (diff < other.diff) return -1;
        if (diff > other.diff) return 1;
        return 0;
    }
}
