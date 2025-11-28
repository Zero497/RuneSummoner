using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [NonSerialized]public string upgradeStr;

    public TextMeshProUGUI myText;

    public Button myButton;

    public void OnClick()
    {
        UpgradeTreeManager.unit.UnlockUpgrade(upgradeStr);
        UpgradeTreeManager.upgradeTreeManager.ResetTree();
        myButton.interactable = false;
        myText.text = "Unlocked";
    }
}
