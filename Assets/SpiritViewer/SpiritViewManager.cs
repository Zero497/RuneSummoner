using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpiritViewManager : MonoBehaviour
{
    public static SpiritViewManager spiritViewManager;
    
    public Transform characterFrameContent;

    public GameObject characterButtonPrefab;

    public Transform abilityFrameContent;

    public GameObject abilityButtonPrefab;

    public GameObject levelFrame;

    public GameObject expFrame;

    public Image unitImage;

    public TextMeshProUGUI unitName;

    public TextMeshProUGUI unitGrade;

    public GameObject statsPanel;

    public List<StatFrame> statsFrames;

    public GameObject upgradeButton;

    public TextMeshProUGUI availablePointsText;
    
    private void Awake()
    {
        spiritViewManager = this;
        foreach (UnitSimple unit in UnitManager.playerUnits)
        {
            GameObject unitButton = Instantiate(characterButtonPrefab, characterFrameContent);
            unitButton.transform.GetChild(0).GetComponent<Image>().sprite = unit.GetMyUnitData().portrait;
            unitButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = unit.nickname;
            unitButton.GetComponent<DisplaySpiritInfoOnClick>().unit = unit;
        }
    }

    public void DisplayUnitInfo(UnitSimple unit)
    {
        SetStatsPanel(unit);
        SetAbilitiesPanel(unit);
    }

    private void SetAbilitiesPanel(UnitSimple unit)
    {
        foreach (Transform child in abilityFrameContent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (ActiveAbility.ActiveAbilityDes key in unit.activeAbilities.Keys.OrderBy(key => key))
        {
            GameObject abilityButton = Instantiate(abilityButtonPrefab, abilityFrameContent);
            abilityButton.GetComponent<AbilityInfoButton>().Init(key, unit.activeAbilities[key], unit);
        }
        foreach (PassiveAbility.PassiveAbilityDes key in unit.passiveAbilities.Keys.OrderBy(key => key))
        {
            GameObject abilityButton = Instantiate(abilityButtonPrefab, abilityFrameContent);
            abilityButton.GetComponent<AbilityInfoButton>().Init(key, unit.passiveAbilities[key], unit);
        }
    }

    private void SetStatsPanel(UnitSimple unit)
    {
        levelFrame.SetActive(true);
        levelFrame.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = unit.level.ToString();
        expFrame.SetActive(true);
        float expToNext = unit.ExpToNextLevel();
        expFrame.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Exp to Next Level\n"+ unit.currentExp + "/" + expToNext;
        expFrame.transform.GetChild(0).GetComponent<Image>().fillAmount = unit.currentExp / expToNext;
        statsPanel.SetActive(true);
        UnitData unitData = unit.GetMyUnitData();
        foreach (StatFrame frame in statsFrames)
        {
            frame.Init(unit, unitData);
        }
        unitImage.gameObject.SetActive(true);
        unitImage.sprite = unitData.UnitSprite;
        unitName.gameObject.SetActive(true);
        unitName.text = unit.name.FirstCharacterToUpper();
        
        if (unit.name.Equals("Player"))
        {
            availablePointsText.gameObject.SetActive(false);
            unitGrade.gameObject.SetActive(false);
            upgradeButton.SetActive(false);
        }
        else
        {
            unitGrade.gameObject.SetActive(true);
            unitGrade.text = UnitData.GradeToColorString(unitData.myGrade);
            upgradeButton.SetActive(true);
            availablePointsText.gameObject.SetActive(true);
            availablePointsText.text = "Available Upgrade Points: " + unit.availableUpgradePoints;
        }
    }
}
