using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatFrame : MonoBehaviour
{
    public UnitData.Stat myStat;

    public TextMeshProUGUI value;

    public TextMeshProUGUI grade;

    public Button gradeUpButton;

    public void Init(UnitSimple unit, UnitData baseData)
    {
        float statVal = baseData.GetStatValue(myStat);
        if (grade != null && !unit.name.Equals("Player"))
        {
            statVal = UnitCombatStats.GetActualBase(statVal, unit.statGrades.GetGrade(myStat), unit.level);
            grade.text = UnitData.GradeToColorString(unit.statGrades.GetGrade(myStat));
            gradeUpButton.gameObject.SetActive(true);
            gradeUpButton.interactable = GradeUpShouldBeEnabled(unit);
        }
        else 
        {
            gradeUpButton.gameObject.SetActive(false);
        }
        if (unit.name.Equals("Player") && grade != null)
        {
            grade.gameObject.SetActive(false);
        }
        else if(grade != null)
        {
            grade.gameObject.SetActive(true);
        }
        value.text = Math.Round(statVal,1).ToString();
    }

    private bool GradeUpShouldBeEnabled(UnitSimple unit)
    {
        return false;
    }
}
