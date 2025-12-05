using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class UnitCombatStats
{
    private UnitData baseStats;
    private UnitEvents myEvents;
    private UnitBase myUnit;
    private StatGrades grades;
    private int level;
    
    private float health;
    private float initiative;
    private float abilityPower;
    private float magicalAttack;
    private float physicalAttack;
    private float magicalDefense;
    private float physicalDefense;
    private float mana;
    private float manaRegen;
    private float stamina;
    private float staminaRegen;
    private float sightRadius;
    private float speed;
    
    
    private float currentHealth;
    private float currentMana;
    private float currentStamina;

    public EventPriorityWrapper<UnitBase, string, float> onStatChanged =
        new EventPriorityWrapper<UnitBase, string, float>(); 

    public UnitCombatStats(UnitData inBaseData, UnitSimple unit, UnitEvents events, UnitBase myUnit)
    {
        myEvents = events;
        baseStats = inBaseData;
        level = unit.level;
        this.myUnit = myUnit;
        grades = unit.statGrades;
        
        SetStatsByGradeAndLevel();
        
        currentHealth = health;
        currentMana = mana;
        currentStamina = stamina;
    }

    public float GetActualBase(float dataBase, UnitData.Grade grade)
    {
        float gradePercent = GradeToStatPercentage(grade);
        return dataBase * gradePercent + dataBase * gradePercent * 0.1f * (level - 1);
    }
    
    public static float GetActualBase(float dataBase, UnitData.Grade grade, int level)
    {
        float gradePercent = GradeToStatPercentage(grade);
        return dataBase * gradePercent + dataBase * gradePercent * 0.1f * (level - 1);
    }
    
    public void SetStatsByGradeAndLevel()
    {
        health = GetActualBase(baseStats.health, grades.healthGrade);
        initiative = baseStats.initiative;
        abilityPower = GetActualBase(baseStats.abilityPower, grades.abilityPowerGrade);
        magicalAttack = GetActualBase(baseStats.magicalAttack, grades.magicalAttackGrade);
        physicalAttack = GetActualBase(baseStats.physicalAttack, grades.physicalAttackGrade);
        magicalDefense = GetActualBase(baseStats.magicalDefence, grades.magicalDefenseGrade);
        mana = GetActualBase(baseStats.mana, grades.manaGrade);
        manaRegen = GetActualBase(baseStats.manaRegen, grades.manaRegenGrade);
        stamina = GetActualBase(baseStats.stamina, grades.stamainaGrade);
        staminaRegen = GetActualBase(baseStats.staminaRegen, grades.staminaRegenGrade);
        sightRadius = baseStats.sightRadius;
        speed = baseStats.speed;
    }

    public static float GradeToStatPercentage(UnitData.Grade grade)
    {
        switch (grade)
        {
            case UnitData.Grade.poor:
                return 0.9f;
            case UnitData.Grade.common:
                return 0.95f;
            case UnitData.Grade.normal:
                return 1;
            case UnitData.Grade.rare:
                return 1.05f;
            case UnitData.Grade.epic:
                return 1.1f;
            case UnitData.Grade.legendary:
                return 1.15f;
            default:
                return 1;
        }
    }

    public float GetStat(string statName, bool getBase = false)
    {
        statName = statName.ToLower();
        switch (statName)
        {
            case "health":
                return getHealth(getBase);
            case "abilitypower":
                return getAbilityPower(getBase);
            case "magicalattack":
                return getMagicalAttack(getBase);
            case "physicalattack":
                return getPhysicalAttack(getBase);
            case "magicaldefense":
            case "magicaldefence":
                return getMagicalDefense(getBase);
            case "physicaldefense":
            case "physicaldefence":
                return getPhysicalDefense(getBase);
            case "mana":
                return getMana(getBase);
            case "manaregen":
                return getManaRegen(getBase);
            case "stamina":
                return getStamina(getBase);
            case "staminaregen":
                return getStaminaRegen(getBase);
            case "initiative":
                return getInitiative(getBase);
            case "speed":
                return getSpeed(getBase);
            case "sightradius":
                return getSightRadius(getBase);
            case "currenthealth":
                return getCurrentHealth();
            case "currentmana":
                return getCurrentMana();
            case "currentstamina":
                return getCurrentStamina();
            default:
                return -1;
        }
    }
    
    public void ChangeStat(string statName, float change, bool changeCurrWhenChangeMax = true)
    {
        statName = statName.ToLower();
        switch (statName)
        {
            case "health":
                AddHealth(change, changeCurrWhenChangeMax);
                break;
            case "abilitypower":
                AddAbilityPower(change);
                break;
            case "magicalattack":
                AddMagicalAttack(change);
                break;
            case "physicalattack":
                AddPhysicalAttack(change);
                break;
            case "magicaldefense":
            case "magicaldefence":
                AddMagicalDefense(change);
                break;
            case "physicaldefense":
            case "physicaldefence":
                AddPhysicalDefense(change);
                break;
            case "mana":
                AddMana(change, changeCurrWhenChangeMax);
                break;
            case "manaregen":
                AddManaRegen(change);
                break;
            case "stamina":
                AddStamina(change, changeCurrWhenChangeMax);
                break;
            case "staminaregen":
                AddStaminaRegen(change);
                break;
            case "initiative":
                AddInitiative(change);
                break;
            case "speed":
                AddSpeed(change);
                break;
            case "sightradius":
                AddSightRadius(change);
                break;
            case "currenthealth":
                AddCurrentHealth(change);
                break;
            case "currentmana":
                AddCurrentMana(change);
                break;
            case "currentstamina":
                AddCurrentStamina(change);
                break;
        }
    }

    public float getHealth(bool getBase = false)
    {
        if(getBase) return GetActualBase(baseStats.health, grades.healthGrade);
        return health;
    }
    
    public float getAbilityPower(bool getBase = false)
    {
        if(getBase) return GetActualBase(baseStats.abilityPower, grades.abilityPowerGrade);
        return abilityPower;
    }

    public float getMagicalAttack(bool getBase = false)
    {
        if(getBase) return GetActualBase(baseStats.magicalAttack, grades.magicalAttackGrade);
        return magicalAttack;
    }
    
    public float getPhysicalAttack(bool getBase = false)
    {
        if(getBase) return GetActualBase(baseStats.physicalAttack, grades.physicalAttackGrade);
        return physicalAttack;
    }
    
    public float getMagicalDefense(bool getBase = false)
    {
        if(getBase) return GetActualBase(baseStats.magicalDefence, grades.magicalDefenseGrade);;
        return magicalDefense;
    }
    
    public float getPhysicalDefense(bool getBase = false)
    {
        if(getBase) return GetActualBase(baseStats.physicalDefence, grades.physicalDefenseGrade);
        return physicalDefense;
    }
    
    public float getMana(bool getBase = false)
    {
        if(getBase) return GetActualBase(baseStats.mana, grades.manaGrade);;
        return mana;
    }
    
    public float getManaRegen(bool getBase = false)
    {
        if(getBase) return GetActualBase(baseStats.manaRegen, grades.manaRegenGrade);
        return manaRegen;
    }
    
    public float getStamina(bool getBase = false)
    {
        if(getBase) return GetActualBase(baseStats.stamina, grades.stamainaGrade);
        return stamina;
    }
    
    public float getStaminaRegen(bool getBase = false)
    {
        if(getBase) return GetActualBase(baseStats.staminaRegen, grades.staminaRegenGrade);
        return staminaRegen;
    }
    
    public float getInitiative(bool getBase = false)
    {
        if(getBase) return baseStats.initiative;
        return initiative;
    }
    
    public float getSpeed(bool getBase = false)
    {
        if(getBase) return baseStats.speed;
        return speed;
    }
    
    public float getSightRadius(bool getBase = false)
    {
        if(getBase) return baseStats.sightRadius;
        return sightRadius;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public float getCurrentMana()
    {
        return currentMana;
    }

    public float getCurrentStamina()
    {
        return currentStamina;
    }

    public void RegenStamina(float timePassed)
    {
        Float stamToRegen = new Float(staminaRegen*timePassed);
        myEvents.modStamRegen.Invoke(myUnit, stamToRegen);
        currentStamina = Mathf.Min(stamina, currentStamina + stamToRegen.flt);
        myEvents.onStamRegen.Invoke(myUnit, stamToRegen.flt);
    }

    public void RegenMana(float timePassed)
    {
        Float manaToRegen = new Float(manaRegen*timePassed);
        myEvents.modManaRegen.Invoke(myUnit, manaToRegen);
        currentMana = Mathf.Min(mana, currentMana + manaToRegen.flt);
        myEvents.onManaRegen.Invoke(myUnit, manaToRegen.flt);
    }

    public void AddCurrentStamina(float add)
    {
        currentStamina += add;
        currentStamina = Mathf.Min(stamina, currentStamina);
    }

    public void AddCurrentMana(float add)
    {
        currentMana += add;
        currentMana = Mathf.Min(mana, currentMana);
    }

    public void AddCurrentHealth(float add)
    {
        currentHealth += add;
        currentHealth = Mathf.Min(health, currentHealth);
        if(add < 0)
            myEvents.onTakeDamage.Invoke(myUnit, add);
        else
        {
            myEvents.onRegainHealth.Invoke(myUnit, add);
        }
    }

    //Special bypass, use with caution
    public void SetCurrentHealth(float set)
    {
        currentHealth = set;
    }

    public void AddHealth(float add, bool addCurr = true)
    {
        health += add;
        if(addCurr) AddCurrentHealth(add);
        onStatChanged.Invoke(myUnit, "health", add);
    }
    
    public void AddMana(float add, bool addCurr = true)
    {
        mana += add;
        if(addCurr) AddCurrentMana(add);
        onStatChanged.Invoke(myUnit, "mana", add);
    }
    
    public void AddStamina(float add, bool addCurr = true)
    {
        stamina += add;
        if(addCurr) AddCurrentHealth(add);
        onStatChanged.Invoke(myUnit, "stamina", add);
    }

    public void AddInitiative(float add)
    {
        initiative += add;
        TurnController.controller.ChangeInitiative(myUnit, add);
        onStatChanged.Invoke(myUnit, "initiative", add);
    }

    public void AddAbilityPower(float add)
    {
        abilityPower += add;
        onStatChanged.Invoke(myUnit, "abilitypower", add);
    }

    public void AddMagicalAttack(float add)
    {
        magicalAttack += add;
        onStatChanged.Invoke(myUnit, "magicalattack", add);
    }

    public void AddPhysicalAttack(float add)
    {
        physicalAttack += add;
        onStatChanged.Invoke(myUnit, "physicalattack", add);
    }

    public void AddMagicalDefense(float add)
    {
        magicalDefense += add;
        onStatChanged.Invoke(myUnit, "magicaldefense", add);
    }

    public void AddPhysicalDefense(float add)
    {
        physicalDefense += add;
        onStatChanged.Invoke(myUnit, "physicaldefense", add);
    }

    public void AddManaRegen(float add)
    {
        manaRegen += add;
        onStatChanged.Invoke(myUnit, "manaregen", add);
    }

    public void AddStaminaRegen(float add)
    {
        staminaRegen += add;
        onStatChanged.Invoke(myUnit, "staminaregen", add);
    }

    public void AddSpeed(float add)
    {
        speed += add;
        myUnit.moveRemaining += add;
        onStatChanged.Invoke(myUnit, "speed", add);
    }

    public void AddSightRadius(float add)
    {
        sightRadius += add;
        if(myUnit.isFriendly)
            VisionManager.visionManager.UpdateFriendlyVision(myUnit);
        else
        {
            VisionManager.visionManager.UpdateEnemyVision(myUnit);
        }
        onStatChanged.Invoke(myUnit, "sightradius", add);
    }
}
