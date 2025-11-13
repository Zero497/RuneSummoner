using UnityEngine;

public class DisplaySpiritInfoOnClick : MonoBehaviour
{
    public UnitSimple unit;

    public void OnClick()
    {
        SpiritViewManager.spiritViewManager.DisplayUnitInfo(unit);
    }
}
