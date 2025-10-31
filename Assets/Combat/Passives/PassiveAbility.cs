using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbility : IEquatable<PassiveAbility>
{
   protected string _abilityName;
   
   protected UnitBase source;

   protected int level;

   public string abilityName => _abilityName;

   /*
        Expects:
            Unit 0: unit to apply to
            Float 0: level of ability
     */
   public virtual void Initialize(SendData data)
   { 
       _abilityName = data.strData[0];
       source = data.unitData[0];
       source.passiveAbilities.Add(this);
       level = (int) data.floatData[0];
   }

   public int GetLevel()
   {
       return level;
   }

   /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
     */
   public static PassiveAbility GetPassive(SendData data)
   {
       if (data.strData[0] == null) return null;
       string abilityName = data.strData[0];
       PassiveAbility ability;
       abilityName = abilityName.ToLower();
       switch (abilityName)
       {
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "charge":
               ability = new Charge();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "adaptable":
               ability = new Adaptable();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "bristlingspines":
               ability = new BristlingSpines();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "rage":
               ability = new Rage();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "unstoppable":
               ability = new Unstoppable();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "stealth":
               ability = new Stealth();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "ambush":
               ability = new Ambush();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "openingassault":
               ability = new OpeningAssault();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "skirmisher":
               ability = new Skirmisher();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "observer":
               ability = new Observer();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "farseer":
               ability = new Farseer();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "diversion":
               ability = new Diversion();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "debilitatingmark":
               ability = new DebilitatingMark();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "defensiveattacks":
               ability = new DefensiveAttacks();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "lastingmark":
               ability = new LastingMark();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "evasive":
               ability = new Evasive();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "exposingevade":
               ability = new ExposingEvade();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "mechanical":
               ability = new Mechanical();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "clockworkstrikes":
               ability = new ClockworkStrikes();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "clockworkdefenses":
               ability = new ClockworkDefenses();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "laststand":
               ability = new LastStand();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "guardian":
               ability = new Guardian();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "tauntextension":
               ability = new TauntExtension();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "hyperadapted":
               ability = new HyperAdapted();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "rageboost":
               ability = new RageBoost();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "berserker":
               ability = new Berserker();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "entrenched":
               ability = new Entrenched();
               break;
           /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
           case "lumbering":
               ability = new Lumbering();
               break;
           default:
               return null;
       }
       ability.Initialize(data);
       return ability;
   }

   /*
        Expects:
            String 0: name of passive ability
     */
   public virtual bool Equals(SendData data)
   {
       if (data == null) return false;
       return data.strData[0].Equals(abilityName);
   }

   public virtual bool Equals(PassiveAbility other)
   {
       if (other == null) return false;
       return abilityName.Equals(other.abilityName);
   }
}
