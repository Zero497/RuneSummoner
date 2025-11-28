using UnityEngine;

public class DisplaySpiritInfoOnClick : MonoBehaviour
{
    public UnitSimple unit;

    public void OnClick()
    {
        OpenUpgradeMenuOnClick.curUnit = unit;
        SpiritViewManager.spiritViewManager.DisplayUnitInfo(unit);
    }
}
