using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitBase : MonoBehaviour
{
    public UnitData baseData;

    public float moveRemaining;

    public Vector3Int currentPosition;
    
    public string myId;

    public void TurnStarted()
    {
        moveRemaining = baseData.movementSpeed;
    }
    
    public static int CompareByInitiative(UnitBase item1, UnitBase item2)
    {
        return UnitData.CompareByInitiative(item1.baseData, item2.baseData);
    }
}
