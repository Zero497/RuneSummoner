using System;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    [NonSerialized]public UnitBase myUnit;

    public Image unitImage;

    public Image teamImage;

    public void Init(UnitBase unit)
    {
        myUnit = unit;
        unitImage.sprite = unit.baseData.portrait;
        Color teamColor = Color.red;
        if(unit.isFriendly)
            teamColor = Color.green;
        teamImage.color = teamColor;
    }
}
