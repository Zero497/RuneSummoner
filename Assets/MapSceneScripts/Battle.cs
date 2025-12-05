using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Battle : IComparable<Battle>
{
    public string zoneID;

    public Difficulty diff;

    public int level;

    public Vector2Int mapSize;

    public List<UnitSimple> enemies;
    
    [XmlIgnore]public List<DataTile> tiles;

    public List<MapGenerator.GenerationType> genTypes;

    public List<float> spawnFrequency;
    
    public enum Difficulty
    {
        novice,
        standard,
        heroic,
        mythic
    }

    public float GetMult(UnitSimple unit)
    {
        float mult = 1;
        switch (diff)
        {
            case Difficulty.novice:
                break;
            case Difficulty.standard:
                mult += 0.1f;
                break;
            case Difficulty.heroic:
                mult += 0.2f;
                break;
            case Difficulty.mythic:
                mult += 0.4f;
                break;
        }
        mult += StatGradeToMultAmt(unit.statGrades.magicalAttackGrade);
        mult += StatGradeToMultAmt(unit.statGrades.abilityPowerGrade);
        mult += StatGradeToMultAmt(unit.statGrades.physicalAttackGrade);
        mult += StatGradeToMultAmt(unit.statGrades.healthGrade);
        mult += StatGradeToMultAmt(unit.statGrades.manaGrade);
        mult += StatGradeToMultAmt(unit.statGrades.stamainaGrade);
        mult += StatGradeToMultAmt(unit.statGrades.magicalDefenseGrade);
        mult += StatGradeToMultAmt(unit.statGrades.physicalDefenseGrade);
        mult += StatGradeToMultAmt(unit.statGrades.manaRegenGrade);
        mult += StatGradeToMultAmt(unit.statGrades.staminaRegenGrade);
        return mult;
    }

    private float StatGradeToMultAmt(UnitData.Grade grade)
    {
        switch (grade)
        {
            case UnitData.Grade.poor:
                return -0.025f;
            case UnitData.Grade.common:
                return -0.0125f;
            case UnitData.Grade.normal:
                return 0;
            case UnitData.Grade.rare:
                return 0.0125f;
            case UnitData.Grade.epic:
                return 0.025f;
            case UnitData.Grade.legendary:
                return 0.05f;
        }
        return 0;
    }

    public int CompareTo(Battle other)
    {
        if (zoneID.CompareTo(other.zoneID) != 0)
            return zoneID.CompareTo(other.zoneID);
        if (level < other.level) return -1;
        if (level > other.level) return 1;
        if (diff < other.diff) return -1;
        if (diff > other.diff) return 1;
        return 0;
    }
}
