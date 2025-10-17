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
           default:
               return null;
       }
       foreach (PassiveAbility passive in data.unitData[0].passiveAbilities)
       {
           if (passive.Equals(ability))
           {
               passive.IncrementLevel();
               return null;
           }
       }
       ability.Initialize(data);
       return ability;
   }

   public virtual void IncrementLevel()
   {
       level++;
   }

   public virtual bool Equals(PassiveAbility other)
   {
       if (other == null) return false;
       return abilityName.Equals(abilityName);
   }
}
