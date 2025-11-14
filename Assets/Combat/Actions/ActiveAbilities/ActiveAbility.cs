using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : UnitAction
{
    public int level;

    public ActiveAbilityDes myDes;
    public override void Initialize(SendData sendData)
    {
        base.Initialize(sendData);
        level = sendData.intData[1];
        myDes = (ActiveAbilityDes)sendData.intData[0];
    }
    
    public override Float GetStaminaCost(bool getBase = false)
    {
        if (getBase) return base.GetStaminaCost();
        return new Float(abilityData.staminaCost * (1 + 0.05f * source.abilityPower));
    }
    
    public override Float GetManaCost(bool getBase = false)
    {
        if (getBase) return base.GetStaminaCost();
        return new Float(abilityData.manaCost * (1 + 0.05f * source.abilityPower));
    }
    
    public Func<int, bool> getValidTargets()
    {
        switch (abilityData.targetType)
        {
            case AbilityData.TargetType.singleTargetEnemy:
            case AbilityData.TargetType.multiTargetEnemy:
            case AbilityData.TargetType.aoeEnemyOnly:
                return i => i != source.myTeam;
            case AbilityData.TargetType.singleTargetFriendly:
            case AbilityData.TargetType.multiTargetFriendly:
            case AbilityData.TargetType.aoeFriendlyOnly:
                return i => i == source.myTeam;
            case AbilityData.TargetType.singleTargetNeutral:
            case AbilityData.TargetType.multiTargetNeutral:
            case AbilityData.TargetType.aoeNeutral:
                return i => true; 
        }
        return null;
    }

    public enum ActiveAbilityDes
    {
        block,
        coreOverdraw,
        coreOverload,
        divert,
        dodge,
        electricShroud,
        exposeCore,
        fury,
        magicalMelee,
        magicalRanged,
        mark,
        physicalMelee,
        physicalRanged,
        sprint,
        taunt
    }
    
    public static string GetFullDescription(ActiveAbilityDes des, UnitSimple unit, int level)
    {
        UnitData unitData = Resources.Load<UnitData>(unit.name);
        float abilityPower =
            UnitCombatStats.GetActualBase(unitData.abilityPower, unit.statGrades.abilityPowerGrade, unit.level);
        string name = "";
        string desc = "";
        string abilityType = "";
        string range = "";
        string cost = "";
        string targetType = "";
        string special = "";
        string apEffect = "";
        string levelEffect = "";
        bool isAttack = false;
        bool isAOE = false;
        AbilityData abData;
        AttackData attackData;
        float temp;
        string damage = "";
        string aoeRange = "";
        switch (des)
        {
            case ActiveAbilityDes.block:
                abData = Resources.Load<AbilityData>("AbilityData/Block");
                name = "Block";
                desc = abData.description;
                abilityType = "Reaction";
                range = "Self";
                temp = 2.5f * (1 - 0.02f * abilityPower) * (1.2f - level * 0.2f);
                cost = temp + " Stamina per point of incoming damage";
                targetType = "Self";
                special =
                    "Negate the damage from an incoming attack entirely by paying Stamina. Fails if insufficient Stamina remains.";
                apEffect = "Stamina cost reduced by 0.2% per AP";
                levelEffect = "Stamina cost reduced by 20% per Level after AP";
                break;
            case ActiveAbilityDes.coreOverdraw:
                abData = Resources.Load<AbilityData>("AbilityData/Core OverdrawM");
                name = "Core Overdraw";
                desc = abData.description;
                abilityType = "Support";
                range = "Self";
                temp = abData.manaCost*(1+0.05f*abilityPower);
                cost = temp + " Mana or "+temp+" Stamina";
                targetType = "Self";
                special =
                    "The user gains Mana or Stamina equal to "+(30+20*level)+"% the cost paid in the other and takes "+Mathf.Max(0,60-level*10)+"% that much damage.";
                apEffect = "+5% cost per AP";
                levelEffect = "Increases the Mana or Stamina gain by 20% (additive) and reduces the damage percentage by 10% (subtractive)";
                break;
            case ActiveAbilityDes.coreOverload:
                abData = Resources.Load<AbilityData>("AttackData/Core Overload");
                isAttack = true;
                isAOE = true;
                name = "Core Overload";
                desc = abData.description;
                abilityType = "Attack";
                range = "Self";
                temp = 0.5f + 0.25f * level;
                damage = temp + "Magical Electro per Mana Spent";
                cost = "100% current Mana";
                targetType = "AOE All";
                aoeRange = (abData.aoeRange + Mathf.FloorToInt(abilityPower / 20)).ToString();
                special = "This Unit Dies";
                apEffect = "+1 AOE range per AP";
                levelEffect = "+0.25 Magical Electro damage per Mana per Level";
                break;
            case ActiveAbilityDes.divert:
                abData = Resources.Load<AbilityData>("AbilityData/Divert");
                name = "Divert";
                desc = abData.description;
                abilityType = "Reaction";
                range = "1";
                temp = abData.staminaCost;
                cost = temp + " Stamina";
                targetType = "Single Enemy";
                special =
                    "When an adjacent friendly Unit makes an attack on an adjacent Unit, apply "+(1+2*level)+" stacks of Marked (after the attack).";
                apEffect = "None";
                levelEffect = "+2 Marked stacks applied";
                break;
            case ActiveAbilityDes.dodge:
                abData = Resources.Load<AbilityData>("AbilityData/Dodge");
                name = "Dodge";
                desc = abData.description;
                abilityType = "Support";
                range = "Self";
                temp = abData.staminaCost*(1-0.01f*abilityPower);
                cost = temp + " Stamina";
                targetType = "Self";
                special =
                    "Grants "+(6+4*level)+" stacks of Evade.";
                apEffect = "-1% Stamina Cost per AP";
                levelEffect = "+4 Stacks of Evade per Level";
                break;
            case ActiveAbilityDes.electricShroud:
                abData = Resources.Load<AbilityData>("AbilityData/Electric Shroud");
                name = "Electric Shroud";
                desc = abData.description;
                abilityType = "Support";
                range = "Self";
                temp = abData.manaCost*(1+0.05f*abilityPower);
                cost = temp + " Mana";
                targetType = "Self";
                temp = Mathf.FloorToInt(20 * (1 + 0.05f * abilityPower));
                temp = Mathf.FloorToInt(temp * (0.5f + 0.5f * level));
                special =
                    "Free Action. User gains Spikes Magical Electro "+temp+" until the User's next turn.";
                apEffect = "+5% (rounded down) Spikes applied and +5% Mana Cost";
                levelEffect = "Spikes applied +50% after the increase from AP";
                break;
            case ActiveAbilityDes.exposeCore:
                attackData = Resources.Load<AttackData>("AttackData/Expose Core");
                isAttack = true;
                name = "Expose Core";
                desc = attackData.description;
                abilityType = "Attack";
                range = (attackData.range+(level-1)).ToString();
                temp = attackData.damage*(1+0.05f*abilityPower);
                damage = temp + " Magical Electro";
                cost = attackData.manaCost+" Mana";
                targetType = "Single Enemy";
                special = "The target gains Shocked "+(5*level)+". This Unit gains Vulnerable 3.";
                apEffect = "+5% damage and cost per AP";
                levelEffect = "+1 Range per Level +5 Shocked applied per Level";
                break;
            case ActiveAbilityDes.fury:
                attackData = Resources.Load<AttackData>("AttackData/Fury");
                AttackData basicData;
                
                isAttack = true;
                name = "Fury";
                desc = attackData.description;
                abilityType = "Attack";
                range = (attackData.range+(level-1)).ToString();
                temp = attackData.damage*(1+0.05f*abilityPower);
                damage = temp + " Magical Electro";
                cost = attackData.manaCost+" Mana";
                targetType = "Single Enemy";
                special = "The target gains Shocked "+(5*level)+". This Unit gains Vulnerable 3.";
                apEffect = "+5% damage and cost per AP";
                levelEffect = "+1 Range per Level +5 Shocked applied per Level";
                break;
        }
        string ret = "<style=\"H1\">"+name+"</style>\n";
        ret += "\t<style=\"H2\">Level: " + level+"\n";
        ret += desc+"</style>\n";
        ret += "\tAbility Type: "+abilityType+"\n";
        ret += "\tRange: "+range+"\n";
        if (isAttack)
            ret += "\tDamage: " + damage + "\n";
        ret += "\tCost: " + cost+"\n";
        ret += "\tTarget Type: " + targetType+"\n";
        if (isAOE)
            ret += "\tAOE Range: " + aoeRange + "\n";
        ret += "\tAdditional Details: " + special + "\n";
        ret += "\tAbility Power Effect: " + apEffect+"\n";
        ret += "\tLevel Effect: " + levelEffect+"\n";
        return ret;
    }


    public static ActiveAbility GetActiveAbility(SendData data)
    {
        if (data.strData[0] == null) return null;
        ActiveAbilityDes des = (ActiveAbilityDes)data.intData[0];
        ActiveAbility ability;
        AbilityData abilityData = Resources.Load<AbilityData>(data.strData[0]);
        switch (des)
        {
            /*
        Expects:
            Unit 0: unit to apply to
            Int 0: enum ability des
            Int 1: level of ability
     */
            case ActiveAbilityDes.block:
                ability = new Block();
                break;
            /*
        Expects:
            Unit 0: unit to apply to
            Int 0: enum ability des
            Int 1: level of ability
     */
            case ActiveAbilityDes.coreOverdraw:
                ability = new CoreOverdraw();
                break;
            /*
        Expects:
            Unit 0: unit to apply to
            Int 0: enum ability des
            Int 1: level of ability
     */
            case ActiveAbilityDes.divert:
                ability = new Divert();
                break;
            /*
        Expects:
            Unit 0: unit to apply to
            Int 0: enum ability des
            Int 1: level of ability
     */
            case ActiveAbilityDes.dodge:
                ability = new Dodge();
                break;
            /*
        Expects:
            Unit 0: unit to apply to
            Int 0: enum ability des
            Int 1: level of ability
     */
            case ActiveAbilityDes.electricShroud:
                ability = new ElectricShroud();
                break;
            /*
        Expects:
            Unit 0: unit to apply to
            Int 0: enum ability des
            Int 1: level of ability
     */
            case ActiveAbilityDes.mark:
                ability = new Mark();
                break;
            /*
        Expects:
            Unit 0: unit to apply to
            Int 0: enum ability des
            Int 1: level of ability
     */
            case ActiveAbilityDes.sprint:
                ability = new Sprint();
                break;
            /*
        Expects:
            Unit 0: unit to apply to
            Int 0: enum ability des
            Int 1: level of ability
     */
            case ActiveAbilityDes.taunt:
                ability = new Taunt();
                break;
            /*
        Expects:
            Unit 0: unit to apply to
            Int 0: enum ability des
            Int 1: level of ability
     */
            case ActiveAbilityDes.coreOverload:
                ability = new CoreOverload();
                break;
            /*
        Expects:
            Unit 0: unit to apply to
            Int 0: enum ability des
            Int 1: level of ability
     */
            case ActiveAbilityDes.exposeCore:
                ability = new ExposeCore();
                break;
            /*
        Expects:
            Unit 0: unit to apply to
            Int 0: enum ability des
            Int 1: level of ability
     */
            case ActiveAbilityDes.fury:
                ability = new Fury();
                break;
            /*
        Expects:
            Unit 0: unit to apply to
            Int 0: enum ability des
            Int 1: level of ability
     */
            case ActiveAbilityDes.physicalMelee:
            case ActiveAbilityDes.physicalRanged:
            case ActiveAbilityDes.magicalMelee:
            case ActiveAbilityDes.magicalRanged:
                ability = new Attack();
                break;
            default:
                return null;
        }
        ability.abilityData = abilityData;
        ability.Initialize(data);
        return ability;
    }
}
