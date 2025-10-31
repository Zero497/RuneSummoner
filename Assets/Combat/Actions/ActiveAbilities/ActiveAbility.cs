using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : UnitAction
{
    public int level;

    public override void Initialize(SendData sendData)
    {
        base.Initialize(sendData);
        level = (int)sendData.floatData[0];
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

    public static ActiveAbility GetActiveAbility(SendData data)
    {
        if (data.strData[0] == null) return null;
        string abilityName = data.strData[0];
        ActiveAbility ability;
        AbilityData abilityData = Resources.Load<AbilityData>(data.strData[0]);
        abilityName = abilityName.ToLower();
        switch (abilityName)
        {
            /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
            case "block":
                ability = new Block();
                break;
            /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
            case "coreoverdraw":
                ability = new CoreOverdraw();
                break;
            /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
            case "diver":
                ability = new Divert();
                break;
            /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
            case "dodge":
                ability = new Dodge();
                break;
            /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
            case "electricshroud":
                ability = new ElectricShroud();
                break;
            /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
            case "mark":
                ability = new Mark();
                break;
            /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
            case "sprint":
                ability = new Sprint();
                break;
            /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
            case "taunt":
                ability = new Taunt();
                break;
            /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
            case "coreoverload":
                ability = new CoreOverload();
                break;
            /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
            case "exposecore":
                ability = new ExposeCore();
                break;
            /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
            case "fury":
                ability = new Fury();
                break;
            /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
            case "physicalmelee":
            case "physicalranged":
            case "magicalmelee":
            case "magicalranged":
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
