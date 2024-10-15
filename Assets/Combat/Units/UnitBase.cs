using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public UnitData baseData;

    public void TurnStarted()
    {
        
    }
    
    public static int CompareByInitiative(UnitBase item1, UnitBase item2)
    {
        return UnitData.CompareByInitiative(item1.baseData, item2.baseData);
    }
}
