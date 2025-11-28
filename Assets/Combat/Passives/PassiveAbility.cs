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

   public class PassiveText
   {
       public string pName;
       public string desc;
       public string levelEffect;
   }

   public static string GetPassiveFullText(PassiveAbilityDes des, UnitSimple unit, int level)
   {
       UnitData unitData = unit.GetMyUnitData();
       string ret = "";
       PassiveText text = new PassiveText();
       switch (des)
       {
           case PassiveAbilityDes.adaptable:
               text = Adaptable.GetFullText(level);
               break;
           case PassiveAbilityDes.ambush:
               text = Ambush.GetFullText(level);
               break;
           case PassiveAbilityDes.berserker:
               text = Berserker.GetFullText(level);
               break;
           case PassiveAbilityDes.bristlingSpines:
               text = BristlingSpines.GetFullText(level, unitData.DefaultDamageElement);
               break;
           case PassiveAbilityDes.charge:
               text = Charge.GetFullText(level);
               break;
           case PassiveAbilityDes.clockworkDefenses:
               text = ClockworkDefenses.GetFullText(level);
               break;
           case PassiveAbilityDes.clockworkStrikes:
               text = ClockworkStrikes.GetFullText(level);
               break;
           case PassiveAbilityDes.debilitatingMark:
               text = DebilitatingMark.GetFullText(level);
               break;
           case PassiveAbilityDes.defensiveAttacks:
               text = DefensiveAttacks.GetFullText(level);
               break;
           case PassiveAbilityDes.diversion:
               text = Diversion.GetFullText(level);
               break;
           case PassiveAbilityDes.entrenched:
               text = Entrenched.GetFullText(level);
               break;
           case PassiveAbilityDes.evasive:
               text = Evasive.GetFullText(level);
               break;
           case PassiveAbilityDes.exposingEvade:
               text = ExposingEvade.GetFullText(level);
               break;
           case PassiveAbilityDes.farseer:
               text = Farseer.GetFullText(level);
               break;
           case PassiveAbilityDes.guardian:
               text = Guardian.GetFullText(level);
               break;
           case PassiveAbilityDes.hyperAdapted:
               text = HyperAdapted.GetFullText(level);
               break;
           case PassiveAbilityDes.lastingMark:
               text = LastingMark.GetFullText(level);
               break;
           case PassiveAbilityDes.lastStand:
               text = LastStand.GetFullText(level);
               break;
           case PassiveAbilityDes.lumbering:
               text = Lumbering.GetFullText(level);
               break;
           case PassiveAbilityDes.mechanical:
               text = Mechanical.GetFullText(level);
               break;
           case PassiveAbilityDes.observer:
               text = Observer.GetFullText(level);
               break;
           case PassiveAbilityDes.openingAssault:
               text = OpeningAssault.GetFullText(level);
               break;
           case PassiveAbilityDes.rage:
               text = Rage.GetFullText(level);
               break;
           case PassiveAbilityDes.rageBoost:
               text = RageBoost.GetFullText(level);
               break;
           case PassiveAbilityDes.resilient:
               text = Resilient.GetFullText(level);
               break;
           case PassiveAbilityDes.skirmisher:
               text = Skirmisher.GetFullText(level);
               break;
           case PassiveAbilityDes.stealth:
               text = Stealth.GetFullText(level);
               break;
           case PassiveAbilityDes.tauntExtension:
               text = TauntExtension.GetFullText(level);
               break;
           case PassiveAbilityDes.unstoppable:
               text = Unstoppable.GetFullText(level);
               break;
       }
       ret += "<style=\"H1\">" + text.pName + "</style>\n";
       ret += "<style=\"H2\">Level: " + level + "</style>\n";
       ret += "Desc: " + text.desc + "\n";
       ret += "Level Effect: " + text.levelEffect + "\n";
       return ret;
   }
   
   public static PassiveData GetPassiveData(PassiveAbilityDes des)
   {
       PassiveData ret = null;
       switch (des)
       {
           case PassiveAbilityDes.adaptable:
               ret = Resources.Load<PassiveData>("Adaptable");
               break;
           case PassiveAbilityDes.ambush:
               ret = Resources.Load<PassiveData>("Ambush");
               break;
           case PassiveAbilityDes.berserker:
               ret = Resources.Load<PassiveData>("Berserker");
               break;
           case PassiveAbilityDes.bristlingSpines:
               ret = Resources.Load<PassiveData>("Bristling Spines");
               break;
           case PassiveAbilityDes.charge:
               ret = Resources.Load<PassiveData>("Charge");
               break;
           case PassiveAbilityDes.clockworkDefenses:
               ret = Resources.Load<PassiveData>("Clockwork Defenses");
               break;
           case PassiveAbilityDes.clockworkStrikes:
               ret = Resources.Load<PassiveData>("Clockwork Strikes");
               break;
           case PassiveAbilityDes.debilitatingMark:
               ret = Resources.Load<PassiveData>("Debilitating Mark");
               break;
           case PassiveAbilityDes.defensiveAttacks:
               ret = Resources.Load<PassiveData>("Defensive Attacks");
               break;
           case PassiveAbilityDes.diversion:
               ret = Resources.Load<PassiveData>("Diversion");
               break;
           case PassiveAbilityDes.entrenched:
               ret = Resources.Load<PassiveData>("Entrenched");
               break;
           case PassiveAbilityDes.evasive:
               ret = Resources.Load<PassiveData>("Evasive");
               break;
           case PassiveAbilityDes.exposingEvade:
               ret = Resources.Load<PassiveData>("Exposing Evade");
               break;
           case PassiveAbilityDes.farseer:
               ret = Resources.Load<PassiveData>("Farseer");
               break;
           case PassiveAbilityDes.guardian:
               ret = Resources.Load<PassiveData>("Guardian");
               break;
           case PassiveAbilityDes.hyperAdapted:
               ret = Resources.Load<PassiveData>("Hyper-Adapted");
               break;
           case PassiveAbilityDes.lastingMark:
               ret = Resources.Load<PassiveData>("Lasting Mark");
               break;
           case PassiveAbilityDes.lastStand:
               ret = Resources.Load<PassiveData>("Last Stand");
               break;
           case PassiveAbilityDes.lumbering:
               ret = Resources.Load<PassiveData>("Lumbering");
               break;
           case PassiveAbilityDes.mechanical:
               ret = Resources.Load<PassiveData>("Mechanical");
               break;
           case PassiveAbilityDes.observer:
               ret = Resources.Load<PassiveData>("Observer");
               break;
           case PassiveAbilityDes.openingAssault:
               ret = Resources.Load<PassiveData>("OpeningAssault");
               break;
           case PassiveAbilityDes.rage:
               ret = Resources.Load<PassiveData>("Rage");
               break;
           case PassiveAbilityDes.rageBoost:
               ret = Resources.Load<PassiveData>("Rage Boost");
               break;
           case PassiveAbilityDes.resilient:
               ret = Resources.Load<PassiveData>("Resilient");
               break;
           case PassiveAbilityDes.skirmisher:
               ret = Resources.Load<PassiveData>("Skirmisher");
               break;
           case PassiveAbilityDes.stealth:
               ret = Resources.Load<PassiveData>("Stealth");
               break;
           case PassiveAbilityDes.tauntExtension:
               ret = Resources.Load<PassiveData>("Taunt Extension");
               break;
           case PassiveAbilityDes.unstoppable:
               ret = Resources.Load<PassiveData>("Unstoppable");
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
