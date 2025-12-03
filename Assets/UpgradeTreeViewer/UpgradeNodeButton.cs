using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeNodeButton : MonoBehaviour
{
    [NonSerialized]public UpgradeTreeNode myUpgrade;

    [NonSerialized]public string myUpgradeString;

    public Image myIcon;

    public TextMeshProUGUI myName;

    public List<UpgradeNodeButton> myChildren;

    [NonSerialized]public bool isEnabled;

    [NonSerialized]public bool isUnlocked;

    public Image buttonImage;

    public Button button;

    public Sprite disabledButton;

    public Sprite enabledButton;

    public Sprite unlockedButton;

    public UpgradeButton upgradeButton;

    public LineRenderer lineRenderer;

    [NonSerialized]public bool isOpen = false;

    public void OnClick()
    {
        if (isOpen)
        {
            UpgradeTreeManager.upgradeTreeManager.panel1.gameObject.SetActive(false);
            UpgradeTreeManager.upgradeTreeManager.panel2.gameObject.SetActive(false);
            isOpen = false;
            return;
        }
        isOpen = true;
        UpgradeTreeTextPanel panel1 = UpgradeTreeManager.upgradeTreeManager.panel1;
        UpgradeTreeTextPanel panel2 = UpgradeTreeManager.upgradeTreeManager.panel2;
        string p1Text = "";
        string p2Text = "";
        UnitSimple unit = UpgradeTreeManager.unit;
        if (myUpgrade.activeGrant.Count > 0)
        {
            ActiveAbility.ActiveAbilityDes active = myUpgrade.activeGrant[0];
            if (UpgradeTreeManager.unit.activeAbilities.ContainsKey(active) && !isUnlocked)
            {
                p1Text = ActiveAbility.AbilityTextToFullDesc(
                    ActiveAbility.GetAbilityText(active, unit, unit.activeAbilities[active] + 1),
                    unit.activeAbilities[active] + 1);
                p2Text = ActiveAbility.AbilityTextToFullDesc(
                    ActiveAbility.GetAbilityText(active, unit, unit.activeAbilities[active]),
                    unit.activeAbilities[active]);
            }
            else if(isUnlocked)
            {
                p1Text = ActiveAbility.AbilityTextToFullDesc(
                    ActiveAbility.GetAbilityText(active, unit, unit.activeAbilities[active]),
                    unit.activeAbilities[active]);
            }
            else
            {
                p1Text = ActiveAbility.AbilityTextToFullDesc(
                    ActiveAbility.GetAbilityText(active, unit, 1),
                    1);
            }
        }
        else if (myUpgrade.passiveGrant.Count > 0)
        {
            PassiveAbility.PassiveAbilityDes passive = myUpgrade.passiveGrant[0];
            if (UpgradeTreeManager.unit.passiveAbilities.ContainsKey(passive) && !isUnlocked)
            {
                p1Text = PassiveAbility.GetPassiveFullText(passive, unit, unit.passiveAbilities[passive] + 1);
                p2Text = PassiveAbility.GetPassiveFullText(passive, unit, unit.passiveAbilities[passive]);
            }
            else if(isUnlocked)
            {
                p1Text = PassiveAbility.GetPassiveFullText(passive, unit, unit.passiveAbilities[passive]);
            }
            else
            {
                p1Text = PassiveAbility.GetPassiveFullText(passive, unit, 1);
            }
        }
        else
        {
            return;
        }
        panel1.gameObject.SetActive(true);
        panel1.descriptionText.text = p1Text;
        if (p2Text.Equals(""))
        {
            panel2.gameObject.SetActive(false);
        }
        else
        {
            panel2.gameObject.SetActive(true);
            panel2.descriptionText.text = p2Text;
        }

        if (isUnlocked)
        {
            upgradeButton.myButton.interactable = false;
            upgradeButton.myText.text = "Unlocked";
        }
        else if (unit.availableUpgradePoints < 0)
        {
            upgradeButton.myButton.interactable = false;
            upgradeButton.myText.text = "Insufficient Upgrade Points";
        }
        else if (!isEnabled)
        {
            upgradeButton.myButton.interactable = false;
            upgradeButton.myText.text = "Unlock Previous Nodes First";
        }
        else
        {
            upgradeButton.myButton.interactable = true;
            upgradeButton.myText.text = "Upgrade";
            upgradeButton.upgradeStr = myUpgradeString;
        }
        
    }

    public void Init(bool unlocked, string upgradeString, UpgradeTreeNode upgradeTreeNode)
    {
        isUnlocked = unlocked;
        if (unlocked)
        {
            foreach (UpgradeNodeButton child in myChildren)
            {
                child.isEnabled = true;
            }
            isEnabled = false;
        }
        else
        {
            foreach (UpgradeNodeButton child in myChildren)
            {
                child.isEnabled = false;
            }
        }
        if (unlocked)
        {
            buttonImage.sprite = unlockedButton;
            SpriteState buttonState = button.spriteState;
            buttonState.pressedSprite = unlockedButton;
            button.spriteState = buttonState;
        }
        else if (isEnabled)
        {
            buttonImage.sprite = enabledButton;
            SpriteState buttonState = button.spriteState;
            buttonState.pressedSprite = enabledButton;
            button.spriteState = buttonState;
        }
        else
        {
            buttonImage.sprite = disabledButton;
            SpriteState buttonState = button.spriteState;
            buttonState.pressedSprite = disabledButton;
            button.spriteState = buttonState;
        }

        if (upgradeTreeNode.activeGrant.Count > 0)
        {
            ActiveAbility.AbilityText txt =
                ActiveAbility.GetAbilityText(upgradeTreeNode.activeGrant[0], UpgradeTreeManager.unit, 1);
            myName.text = txt.name;
            myIcon.sprite = txt.icon;
        }
        else if (upgradeTreeNode.passiveGrant.Count > 0)
        {
            PassiveData pd = PassiveAbility.GetPassiveData(upgradeTreeNode.passiveGrant[0]);
            myName.text = pd.name;
            myIcon.sprite = pd.icon;
        }
        else
        {
            myName.text = UpgradeTreeManager.unit.name;
            myIcon.sprite = UpgradeTreeManager.unit.GetMyUnitData().portrait;
        }
        myUpgradeString = upgradeString;
        myUpgrade = upgradeTreeNode;
        SetupLineRenderer();
    }

    private void SetupLineRenderer()
    {
        Color lineColor = Color.green;

        if (!isUnlocked)
        {
            if (isEnabled)
                lineColor = Color.yellow;
            else
            {
                lineColor = Color.gray;
            }
        }
        
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }
}
