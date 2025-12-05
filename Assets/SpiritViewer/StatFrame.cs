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

    private int gradeUpCost;

    private UnitSimple myUnit;

    public void Init(UnitSimple unit, UnitData baseData)
    {
        myUnit = unit;
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

    public void GradeUp()
    {
        myUnit.statGrades.ChangeGrade(myUnit.statGrades.GetGrade(myStat)+1, myStat);
        Init(myUnit, myUnit.GetMyUnitData());
    }

    private bool GradeUpShouldBeEnabled(UnitSimple unit)
    {
        UnitData.Grade sGrade = unit.statGrades.GetGrade(myStat);
        switch (sGrade)
        {
            case UnitData.Grade.poor:
                gradeUpCost = 30;
                break;
            case UnitData.Grade.common:
                gradeUpCost = 50;
                break;
            case UnitData.Grade.normal:
                gradeUpCost = 80;
                break;
            case UnitData.Grade.rare:
                gradeUpCost = 120;
                break;
            case UnitData.Grade.epic:
                gradeUpCost = 170;
                break;
            case UnitData.Grade.legendary:
                gradeUpCost = Int32.MaxValue;
                break;
        }

        if (InventoryManager.GetSummonShards(unit.name) >= gradeUpCost) return true;
        return false;
    }
}
