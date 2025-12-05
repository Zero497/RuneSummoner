using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : UnitAction
{
    public int level;

    public ActiveAbilityDes myDes;

    public bool usedThisTurn;
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

    public class AbilityText
    {
        public string damage = "";
        public string aoeRange = "";
        public string name = "";
        public string desc = "";
        public string abilityType = "";
        public string range = "";
        public string cost = "";
        public string targetType = "";
        public string special = "";
        public string apEffect = "";
        public string levelEffect = "";
        public bool isAttack = false;
        public bool isAOE = false;
        public Sprite icon;
    }

    public static string AbilityTextToFullDesc(AbilityText text, int level)
    {
        string ret = "<style=\"H1\">" + text.name+"</style>\n";
        ret += "\t<style=\"H2\">Level: " + level+"</style>\n";
        ret += "\tAbility Type: "+text.abilityType+"\n";
        ret += "\tRange: "+text.range+"\n";
        if (text.isAttack)
            ret += "\tDamage: " + text.damage + "\n";
        ret += "\tCost: " + text.cost+"\n";
        ret += "\tTarget Type: " + text.targetType+"\n";
        if (text.isAOE)
            ret += "\tAOE Range: " + text.aoeRange + "\n";
        ret += "\tAdditional Details: " + text.special + "\n";
        ret += "\tAbility Power Effect: " + text.apEffect+"\n";
        ret += "\tLevel Effect: " + text.levelEffect+"\n";
        return ret;
    }
    public static AbilityText GetAbilityText(ActiveAbilityDes des, UnitSimple unit, int level)
    {
        UnitData unitData = unit.GetMyUnitData();
        float abilityPower =
            UnitCombatStats.GetActualBase(unitData.abilityPower, unit.statGrades.abilityPowerGrade, unit.level);
        float physicalAttack =
            UnitCombatStats.GetActualBase(unitData.physicalAttack, unit.statGrades.physicalAttackGrade, unit.level);
        float magicalAttack =
            UnitCombatStats.GetActualBase(unitData.magicalAttack, unit.statGrades.magicalAttackGrade, unit.level);
        AbilityText text = new AbilityText();
        switch (des)
        {
            case ActiveAbilityDes.block:
                text = Block.GetAbilityText(level, abilityPower);
                break;
            case ActiveAbilityDes.coreOverdraw:
                text = CoreOverdraw.GetAbilityText(level, abilityPower);
                break;
            case ActiveAbilityDes.coreOverload:
                text = CoreOverload.GetAbilityText(level, abilityPower, magicalAttack);
                break;
            case ActiveAbilityDes.divert:
                text = Divert.GetAbilityText(level, abilityPower);
                break;
            case ActiveAbilityDes.dodge:
                text = Dodge.GetAbilityText(level, abilityPower);
                break;
            case ActiveAbilityDes.electricShroud:
                text = ElectricShroud.GetAbilityText(level, abilityPower);
                break;
            case ActiveAbilityDes.exposeCore:
                text = ExposeCore.GetAbilityText(level, abilityPower, magicalAttack);
                break;
            case ActiveAbilityDes.fury:
                AbilityText basicText = new AbilityText();
                foreach (ActiveAbilityDes abilityDes in unitData.baseActiveAbilities)
                {
                    if (abilityDes == ActiveAbilityDes.physicalMelee || abilityDes == ActiveAbilityDes.physicalRanged || abilityDes == ActiveAbilityDes.magicalMelee || abilityDes == ActiveAbilityDes.magicalRanged)
                        basicText = Attack.GetAbilityText(level, abilityPower, physicalAttack, magicalAttack, abilityDes,
                            unitData.DefaultDamageElement);
                }
                text = Fury.GetAbilityText(level, abilityPower, physicalAttack, magicalAttack, basicText);
                break;
            case ActiveAbilityDes.physicalMelee:
            case ActiveAbilityDes.physicalRanged:
            case ActiveAbilityDes.magicalMelee:
            case ActiveAbilityDes.magicalRanged:
                text = Attack.GetAbilityText(level, abilityPower, physicalAttack, magicalAttack, des,
                    unitData.DefaultDamageElement);
                break;
            case ActiveAbilityDes.mark:
                text = Mark.GetAbilityText(level, abilityPower);
                break;
            case ActiveAbilityDes.sprint:
                text = Sprint.GetAbilityText(level, abilityPower);
                break;
            case ActiveAbilityDes.taunt:
                text = Taunt.GetAbilityText(level, abilityPower);
                break;
        }
        return text;
    }


    public static ActiveAbility GetActiveAbility(SendData data)
    {
        ActiveAbilityDes des = (ActiveAbilityDes)data.intData[0];
        ActiveAbility ability;
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
        ability.Initialize(data);
        return ability;
    }
}
