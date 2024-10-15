using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    public float Initiative;

    public Sprite portrait;

    public float movementSpeed;

    public float sightRadius;

    public static int CompareByInitiative(UnitData item1, UnitData item2)
    {
        if (item1 == null && item2 == null) return 0;
        if (item1 == null) return -1;
        if (item2 == null) return 1;
        if (item1.Initiative < item2.Initiative) return -1;
        if (item1.Initiative > item2.Initiative) return 1;
        return 0;
    }
}
