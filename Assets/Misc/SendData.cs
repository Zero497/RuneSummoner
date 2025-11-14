using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendData
{
    public List<string> strData = new List<string>();

    public List<UnitBase> unitData = new List<UnitBase>();

    public List<Vector3Int> positionData = new List<Vector3Int>();

    public List<int> intData = new List<int>();

    //some things still use unsafe casts to int from this list
    public List<float> floatData = new List<float>();

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

    public SendData(float flt)
    {
        floatData.Add(flt);
    }

    public SendData(int integer)
    {
        intData.Add(integer);
    }

    public void AddStr(string str)
    {
        strData.Add(str);
    }

    public void AddFloat(float add)
    {
        floatData.Add(add);        
    }

    public void AddV3I(Vector3Int add)
    {
        positionData.Add(add);
    }

    public void AddInt(int add)
    {
        intData.Add(add);
    }

    public void AddUnit(UnitBase unit)
    {
        unitData.Add(unit);
    }
}
