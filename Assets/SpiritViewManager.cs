using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpiritViewManager : MonoBehaviour
{
    public static SpiritViewManager spiritViewManager;
    
    public Transform characterFrameContent;

    public GameObject characterButtonPrefab;

    public GameObject levelFrame;

    public GameObject expFrame;

    public Image unitImage;

    public TextMeshProUGUI unitName;

    public TextMeshProUGUI unitGrade;

    public GameObject statsPanel;

    public List<StatFrame> statsFrames;
    
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
        levelFrame.SetActive(true);
        levelFrame.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = unit.level.ToString();
        expFrame.SetActive(true);
        float expToNext = unit.ExpToNextLevel();
        expFrame.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Exp to Next Level\n"+
            unit.currentExp + "/" + expToNext;
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
        unitGrade.gameObject.SetActive(true);
        unitGrade.text = UnitData.GradeToColorString(unitData.myGrade);
    }
}
