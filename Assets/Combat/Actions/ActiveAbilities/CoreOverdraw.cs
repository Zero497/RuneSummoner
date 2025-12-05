using UnityEngine;

public class CoreOverdraw : ActiveAbility
{
    public override void Initialize(SendData sendData)
    {
        base.Initialize(sendData);
        if(sendData.intData.Count > 2 && sendData.intData[2] > 0)
            abilityData = Resources.Load<AbilityData>("AbilityData/Core OverdrawM");
        else
        {
            abilityData = Resources.Load<AbilityData>("AbilityData/Core OverdrawS");
            SendData dCopy = new SendData(sendData.unitData[0]);
            dCopy.AddInt(sendData.intData[0]);
            dCopy.AddInt(sendData.intData[1]);
            dCopy.AddInt(1);
            sendData.unitData[0].activeAbilities.Add(ActiveAbility.GetActiveAbility(dCopy));
        }
    }

    public override string GetID()
    {
        return "Core Overdraw";
    }

    public override bool RunAction(SendData actionData)
    {
        if (!source.PayCost(this, false) || usedThisTurn) return false;
        source.PayCost(this);
        usedThisTurn = true;
        if (abilityData.staminaCost > 0)
        {
            source.myCombatStats.AddMana(GetStaminaCost().flt * (0.3f+0.2f*level));
            source.TakeDamage(AttackData.DamageType.True, AttackData.Element.neutral, Mathf.Max(0, (0.6f-0.1f*level)*GetStaminaCost().flt));
            source.updateBars();
        }
        else
        {
            source.myCombatStats.AddStamina(GetManaCost().flt * (0.3f+0.2f*level));
            source.TakeDamage(AttackData.DamageType.True, AttackData.Element.neutral, Mathf.Max(0, (0.6f-0.1f*level)*GetManaCost().flt));
        }
        return true;
    }

    public override bool PrepAction()
    {
        return RunAction(new SendData(""));
    }

    public override bool RushCompletion()
    {
        throw new System.NotImplementedException();
    }

    public static AbilityText GetAbilityText(int level, float abilityPower)
    {
        AbilityText ret = new AbilityText();
        AbilityData abData = Resources.Load<AbilityData>("AbilityData/Core OverdrawM");
        ret.name = "Core Overdraw";
        ret.desc = abData.description;
        ret.abilityType = "Support";
        ret.range = "Self";
        float temp = abData.manaCost*(1+0.05f*abilityPower);
        ret.cost = temp + " ("+abData.manaCost+" base) Mana or "+temp+" ("+abData.manaCost+" base) Stamina";
        ret.targetType = "Self";
        ret.special =
            "The user gains Mana or Stamina equal to "+(30+20*level)+"% (50% base) the cost paid in the other and takes "+Mathf.Max(0,60-level*10)+"% (50% base) that much damage.";
        ret.apEffect = "+5% cost per AP";
        ret.levelEffect = "Increases the Mana or Stamina gain by 20% (additive) and reduces the damage percentage by 10% (subtractive)";
        ret.icon = Resources.Load<Sprite>("Icons/CoreOverdraw");
        return ret;
    }
}
