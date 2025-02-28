using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendData
{
    public List<string> strData = new List<string>();

    public List<UnitBase> unitData = new List<UnitBase>();

    public List<Vector3Int> positionData = new List<Vector3Int>();

    public SendData(string str)
    {
        strData.Add(str);
    }
        
    public SendData(UnitBase unit)
    {
        unitData.Add(unit);
    }
        
    public SendData(Vector3Int pos)
    {
        positionData.Add(pos);
    }

    public void AddStr(string str)
    {
        strData.Add(str);
    }
}
