using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class EnemyGenData : ScriptableObject, IComparable<EnemyGenData>
{
    public string eName;
    
    public int minLevel;

    public float frequency;

    public List<string> upgradeStrings;
    public int CompareTo(EnemyGenData other)
    {
        if (minLevel < other.minLevel) return -1;
        if (minLevel > other.minLevel) return 1;
        return eName.CompareTo(other.eName);
    }
}
