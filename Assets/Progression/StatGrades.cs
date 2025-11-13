using UnityEngine;

public class StatGrades 
{
    public UnitData.Grade healthGrade = UnitData.Grade.normal;

    public UnitData.Grade magicalAttackGrade = UnitData.Grade.normal;
    
    public UnitData.Grade physicalAttackGrade = UnitData.Grade.normal;
    
    public UnitData.Grade magicalDefenseGrade = UnitData.Grade.normal;
    
    public UnitData.Grade physicalDefenseGrade = UnitData.Grade.normal;
    
    public UnitData.Grade abilityPowerGrade = UnitData.Grade.normal;
    
    public UnitData.Grade manaGrade = UnitData.Grade.normal;
    
    public UnitData.Grade manaRegenGrade = UnitData.Grade.normal;
    
    public UnitData.Grade stamainaGrade = UnitData.Grade.normal;
    
    public UnitData.Grade staminaRegenGrade = UnitData.Grade.normal;

    public UnitData.Grade GetGrade(UnitData.Stat stat)
    {
        switch (stat)
        {
            case UnitData.Stat.health:
                return healthGrade;
            case UnitData.Stat.magicalAttack:
                return magicalAttackGrade;
            case UnitData.Stat.physicalAttack:
                return physicalAttackGrade;
            case UnitData.Stat.magicalDefense:
                return magicalDefenseGrade;
            case UnitData.Stat.physicalDefense:
                return physicalDefenseGrade;
            case UnitData.Stat.abilityPower:
                return abilityPowerGrade;
            case UnitData.Stat.mana:
                return manaGrade;
            case UnitData.Stat.manaRegen:
                return manaRegenGrade;
            case UnitData.Stat.stamina:
                return stamainaGrade;
            case UnitData.Stat.staminaRegen:
                return staminaRegenGrade;
            default:
                return UnitData.Grade.normal;
        }
    }
}
