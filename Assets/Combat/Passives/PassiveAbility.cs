using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbility : IEquatable<PassiveAbility>
{
   protected UnitBase source;

   protected int level;

   protected PassiveAbilityDes _myDes;

   public string abilityName => GetAbilityName();

   public PassiveAbilityDes myDes => _myDes;

   public abstract string GetAbilityName();

   public enum PassiveAbilityDes
   {
       adaptable,
       ambush,
       berserker,
       bristlingSpines,
       charge,
       clockworkDefenses,
       clockworkStrikes,
       debilitatingMark,
       defensiveAttacks,
       diversion,
       entrenched,
       evasive,
       exposingEvade,
       farseer,
       guardian,
       hyperAdapted,
       lastingMark,
       lastStand,
       lumbering,
       mechanical,
       observer,
       openingAssault,
       skirmisher,
       stealth,
       rage,
       rageBoost,
       resilient,
       tauntExtension,
       unstoppable
   }

   /*
        Expects:
            Unit 0: unit to apply to
            Int 0: ability des
            Int 1: level of ability
     */
   public virtual void Initialize(SendData data)
   {
       _myDes = (PassiveAbilityDes) data.intData[0];
       source = data.unitData[0];
       source.passiveAbilities.Add(this);
       level = data.intData[1];
   }

   public int GetLevel()
   {
       return level;
   }

   public static string GetPassiveFullText(PassiveAbilityDes des, UnitSimple unit, int level)
   {
       UnitData unitData = Resources.Load<UnitData>(unit.name);
       float abilityPower =
           UnitCombatStats.GetActualBase(unitData.abilityPower, unit.statGrades.abilityPowerGrade, unit.level);
       string ret = "";
       switch (des)
       {
           case PassiveAbilityDes.adaptable:
               ret = Adaptable.GetFullText(level);
               break;
           case PassiveAbilityDes.ambush:
               ret = Ambush.GetFullText(level);
               break;
           case PassiveAbilityDes.berserker:
               ret = Berserker.GetFullText(level);
               break;
           case PassiveAbilityDes.bristlingSpines:
               ret = BristlingSpines.GetFullText(level, unitData.DefaultDamageElement);
               break;
           case PassiveAbilityDes.charge:
               ret = Charge.GetFullText(level);
               break;
           case PassiveAbilityDes.clockworkDefenses:
               ret = ClockworkDefenses.GetFullText(level);
               break;
           case PassiveAbilityDes.clockworkStrikes:
               ret = ClockworkStrikes.GetFullText(level);
               break;
           case PassiveAbilityDes.debilitatingMark:
               ret = DebilitatingMark.GetFullText(level);
               break;
           case PassiveAbilityDes.defensiveAttacks:
               ret = DefensiveAttacks.GetFullText(level);
               break;
           case PassiveAbilityDes.diversion:
               ret = Diversion.GetFullText(level);
               break;
           case PassiveAbilityDes.entrenched:
               ret = Entrenched.GetFullText(level);
               break;
           case PassiveAbilityDes.evasive:
               ret = Evasive.GetFullText(level);
               break;
           case PassiveAbilityDes.exposingEvade:
               ret = ExposingEvade.GetFullText(level);
               break;
           case PassiveAbilityDes.farseer:
               ret = Farseer.GetFullText(level);
               break;
           case PassiveAbilityDes.guardian:
               ret = Guardian.GetFullText(level);
               break;
           case PassiveAbilityDes.hyperAdapted:
               ret = HyperAdapted.GetFullText(level);
               break;
           case PassiveAbilityDes.lastingMark:
               ret = LastingMark.GetFullText(level);
               break;
           case PassiveAbilityDes.lastStand:
               ret = LastStand.GetFullText(level);
               break;
           case PassiveAbilityDes.lumbering:
               ret = Lumbering.GetFullText(level);
               break;
           case PassiveAbilityDes.mechanical:
               ret = Mechanical.GetFullText(level);
               break;
           case PassiveAbilityDes.observer:
               ret = Observer.GetFullText(level);
               break;
           case PassiveAbilityDes.openingAssault:
               ret = OpeningAssault.GetFullText(level);
               break;
           case PassiveAbilityDes.rage:
               ret = Rage.GetFullText(level);
               break;
           case PassiveAbilityDes.rageBoost:
               ret = RageBoost.GetFullText(level);
               break;
           case PassiveAbilityDes.resilient:
               ret = Resilient.GetFullText(level);
               break;
           case PassiveAbilityDes.skirmisher:
               ret = Skirmisher.GetFullText(level);
               break;
           case PassiveAbilityDes.tauntExtension:
               ret = TauntExtension.GetFullText(level);
               break;
           case PassiveAbilityDes.unstoppable:
               ret = Unstoppable.GetFullText(level);
               break;
       }
       return ret;
   }

   /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability  
     */
   public static PassiveAbility GetPassive(SendData data)
   {
       if (data.intData[0] < 0) return null;
       PassiveAbilityDes des = (PassiveAbilityDes)data.intData[0];
       PassiveAbility ability;
       switch (des)
       {
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.charge:
               ability = new Charge();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.adaptable:
               ability = new Adaptable();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.bristlingSpines:
               ability = new BristlingSpines();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.rage:
               ability = new Rage();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.unstoppable:
               ability = new Unstoppable();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.stealth:
               ability = new Stealth();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.ambush:
               ability = new Ambush();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.openingAssault:
               ability = new OpeningAssault();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.skirmisher:
               ability = new Skirmisher();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.observer:
               ability = new Observer();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.farseer:
               ability = new Farseer();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.diversion:
               ability = new Diversion();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.debilitatingMark:
               ability = new DebilitatingMark();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.defensiveAttacks:
               ability = new DefensiveAttacks();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.lastingMark:
               ability = new LastingMark();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.evasive:
               ability = new Evasive();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.exposingEvade:
               ability = new ExposingEvade();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.mechanical:
               ability = new Mechanical();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.clockworkStrikes:
               ability = new ClockworkStrikes();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.clockworkDefenses:
               ability = new ClockworkDefenses();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.lastStand:
               ability = new LastStand();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.guardian:
               ability = new Guardian();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.tauntExtension:
               ability = new TauntExtension();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.hyperAdapted:
               ability = new HyperAdapted();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.rageBoost:
               ability = new RageBoost();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.berserker:
               ability = new Berserker();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.entrenched:
               ability = new Entrenched();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.lumbering:
               ability = new Lumbering();
               break;
           /*
        Expects:
            Unit 0: unit to apply to
            Int 0: des of passive ability
            Int 1: level of ability
     */
           case PassiveAbilityDes.resilient:
               ability = new Resilient();
               break;
           default:
               return null;
       }
       ability.Initialize(data);
       return ability;
   }

   /*
        Expects:
            Int 0: des of passive ability
     */
   public virtual bool Equals(SendData data)
   {
       if (data == null) return false;
       return data.intData[0] == (int)myDes;
   }

   public virtual bool Equals(PassiveAbility other)
   {
       if (other == null) return false;
       return abilityName.Equals(other.abilityName);
   }
}
