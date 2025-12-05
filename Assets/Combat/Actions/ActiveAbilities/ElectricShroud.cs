using System;
using UnityEngine;

public class ElectricShroud : ActiveAbility
{
    private ActionPriorityWrapper<UnitBase> onTurnStarted;

    private Effect myEffect;

    private int spikesLastApplied;

    public override void Initialize(SendData sendData)
    {
        base.Initialize(sendData);
        onTurnStarted = new ActionPriorityWrapper<UnitBase>();
        onTurnStarted.priority = 42;
        onTurnStarted.action = OnTurnStarted;
        source.myEvents.onTurnStarted.Subscribe(onTurnStarted);
    }

    public override string GetID()
    {
        return "Electric Shroud";
    }

    public override bool RunAction(SendData actionData)
    {
        if (!source.PayCost(this, false) || usedThisTurn) return false;
        source.PayCost(this);
        usedThisTurn = true;
        int spikesApplied = GetSpikesToApply();
        spikesLastApplied = spikesApplied;
        SendData data = new SendData("spikes");
        data.AddFloat(spikesApplied);
        data.AddFloat((int) AttackData.DamageType.Magic);
        data.AddFloat((int) AttackData.Element.electro);
        myEffect = source.AddEffect(data);
        return true;
    }

    private int GetSpikesToApply()
    {
        int spikesApplied = 20;
        spikesApplied += Mathf.FloorToInt(spikesApplied * 0.05f * source.abilityPower);
        spikesApplied += Mathf.FloorToInt(spikesApplied * 0.5f * (level - 1));
        return spikesApplied;
    }

    private void OnTurnStarted(UnitBase myUnit)
    {
        if (myEffect != null && spikesLastApplied > 0)
        {
            myEffect.AddStacks(-spikesLastApplied);
            spikesLastApplied = 0;
            myEffect = null;
        }
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
        AbilityData abData = Resources.Load<AbilityData>("AbilityData/Electric Shroud");
        ret.name = "Electric Shroud";
        ret.desc = abData.description;
        ret.abilityType = "Support";
        ret.range = "Self";
        float temp = abData.manaCost*(1+0.05f*abilityPower);
        temp = MathF.Round(temp, 2);
        ret.cost = temp + " ("+abData.manaCost+" base) Mana";
        ret.targetType = "Self";
        temp = Mathf.FloorToInt(20 * (1 + 0.05f * abilityPower));
        temp = Mathf.FloorToInt(temp * (0.5f + 0.5f * level));
        ret.special =
            "Free Action. User gains Spikes Magical Electro "+temp+" (20 base) until the User's next turn.";
        ret.apEffect = "+5% (rounded down) Spikes applied and +5% Mana Cost";
        ret.levelEffect = "Spikes applied +50% after the increase from AP";
        ret.icon = Resources.Load<Sprite>("Icons/ElectricShroud");
        return ret;
    }
}
