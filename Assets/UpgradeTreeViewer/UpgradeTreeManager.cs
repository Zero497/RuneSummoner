using System;
using TMPro;
using UnityEngine;

public class UpgradeTreeManager : MonoBehaviour
{
    public static UpgradeTreeManager upgradeTreeManager;

    public static UnitSimple unit;

    public UpgradeTreeTextPanel panel1;

    public UpgradeTreeTextPanel panel2;

    public UpgradeNodeButton primeNode;

    public TextMeshProUGUI availablePointsText;

    public TextMeshProUGUI nicknameText;

    public TextMeshProUGUI typeText;

    public TextMeshProUGUI unitText;

    public TextMeshProUGUI specialtyText;
    private void Awake()
    {
        upgradeTreeManager = this;
        ResetTree();
        nicknameText.text = unit.nickname;
        UnitData data = unit.GetMyUnitData();
        typeText.text = "Type: " + data.myUnitType.ToString().ToUpperInvariant();
        unitText.text = "Unit: " + unit.name;
        specialtyText.text = "Specialty: " + data.myCombatType.ToString().ToUpperInvariant();
    }

    public void ResetTree()
    {
        UnitData data = unit.GetMyUnitData();
        Initialize(primeNode, data.superRoot, "");
        SetAvailablePointsText();
    }

    public void SetAvailablePointsText()
    {
        availablePointsText.text = "Available Upgrade Points: " + unit.availableUpgradePoints;
    }

    private void Initialize(UpgradeNodeButton nodeButton, UpgradeTreeNode node, string upgradeStr)
    {
        nodeButton.Init(unit.acquiredUpgrades.Contains(upgradeStr), upgradeStr, node);
        for (int i = 0; i < nodeButton.myChildren.Count; i++)
        {
            Initialize(nodeButton.myChildren[i], node.branches[i], upgradeStr+i);
        }
    }
}
