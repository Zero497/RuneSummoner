using System.Collections.Generic;
using UnityEngine;

public class ResetAbilities : MonoBehaviour
{
    public void OnClick()
    {
        UpgradeTreeManager.unit.acquiredUpgrades = new List<string> { "0", "1", "2" };
        UpgradeTreeManager.unit.availableUpgradePoints = UpgradeTreeManager.unit.level;
        UpgradeTreeManager.unit.InitAbilities();
        UnitManager.ModUnit(UpgradeTreeManager.unit);
        UpgradeTreeManager.upgradeTreeManager.ResetTree();
    }
}
