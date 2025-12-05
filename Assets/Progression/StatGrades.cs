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

    public static StatGrades RandomStatGrades()
    {
        StatGrades ret = new StatGrades();
        ret.healthGrade = GetRandomGrade();
        ret.magicalAttackGrade = GetRandomGrade();
        ret.physicalAttackGrade = GetRandomGrade();
        ret.magicalDefenseGrade = GetRandomGrade();
        ret.physicalDefenseGrade = GetRandomGrade();
        ret.abilityPowerGrade = GetRandomGrade();
        ret.manaGrade = GetRandomGrade();
        ret.manaRegenGrade = GetRandomGrade();
        ret.stamainaGrade = GetRandomGrade();
        ret.staminaRegenGrade = GetRandomGrade();
        return ret;
    }

    public static UnitData.Grade GetRandomGrade()
    {
        float rand = Random.Range(0, 100);
        UnitData.Grade ret = UnitData.Grade.normal;
        switch (rand)
        {
            case <2.28f:
                ret = UnitData.Grade.poor;
                break;
            case <15.87f:
                ret = UnitData.Grade.common;
                break;
            case <84.13f:
                ret = UnitData.Grade.normal;
                break;
            case <97.73f:
                ret = UnitData.Grade.rare;
                break;
            case <99.87f:
                ret = UnitData.Grade.epic;
                break;
            default:
                ret = UnitData.Grade.legendary;
                break;
        }
        return ret;
    }

    public void ChangeGrade(UnitData.Grade newGrade, UnitData.Stat stat)
    {
        switch (stat)
        {
            case UnitData.Stat.health:
                healthGrade = newGrade;
                break;
            case UnitData.Stat.magicalAttack:
                magicalAttackGrade = newGrade;
                break;
            case UnitData.Stat.physicalAttack:
                physicalAttackGrade = newGrade;
                break;
            case UnitData.Stat.magicalDefense:
                magicalDefenseGrade = newGrade;
                break;
            case UnitData.Stat.physicalDefense:
                physicalDefenseGrade = newGrade;
                break;
            case UnitData.Stat.abilityPower:
                abilityPowerGrade = newGrade;
                break;
            case UnitData.Stat.mana:
                manaGrade = newGrade;
                break;
            case UnitData.Stat.manaRegen:
                manaRegenGrade = newGrade;
                break;
            case UnitData.Stat.stamina:
                stamainaGrade = newGrade;
                break;
            case UnitData.Stat.staminaRegen:
                staminaRegenGrade = newGrade;
                break;
        }
    }

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
