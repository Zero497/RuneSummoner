using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonButton : MonoBehaviour
{
    [NonSerialized]public string myUnitName;

    public Button myButton;

    public Image unitIcon;

    public TextMeshProUGUI text;
    
    public void Init()
    {
        unitIcon.sprite = Resources.Load<UnitData>("UnitData/"+myUnitName).portrait;
        text.text = "Summon: " + myUnitName+"\nSummon Shards: "+InventoryManager.GetSummonShards(myUnitName);
        if (InventoryManager.GetSummonShards(myUnitName) < 100)
            myButton.interactable = false;
    }

    public void OnClick()
    {
        UnitManager.AddUnit(new UnitSimple(myUnitName, UnitManager.GetValidUnitID(myUnitName), 1, StatGrades.RandomStatGrades()));
        InventoryManager.ChangeSummonShards(myUnitName, -100);
        if(InventoryManager.GetSummonShards(myUnitName) < 100)
            myButton.interactable = false;
    }
}
