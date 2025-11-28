using System;
using UnityEngine;

public class Block : Reaction
{
    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttacked;
    
    public override void Initialize(SendData sendData)
    {
        base.Initialize(sendData);
        onAttacked = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        onAttacked.priority = 210;
        onAttacked.action = OnAttacked;
        source.myEvents.onAttacked.Subscribe(onAttacked);
    }

    public override string GetID()
    {
        return "Block";
    }

    private void OnAttacked(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        if (isActive)
        {
            if (!source.PayCost(GetCost(attack.damage), true)) return;
            attack.damage = 0;
        }
    }

    private float GetCost(float damage)
    {
        float cost = 2.5f * damage;
        cost *= 1 - 0.02f * source.abilityPower;
        cost *= 1 - 0.2f * (level - 1);
        return cost;
    }

    public override bool PrepAction()
    {
        throw new System.NotImplementedException();
    }

    public override bool RushCompletion()
    {
        throw new System.NotImplementedException();
    }

    public static AbilityText GetAbilityText(float level, float abilityPower)
    {
        AbilityText ret = new AbilityText();
        AbilityData abData = Resources.Load<AbilityData>("AbilityData/Block");
        ret.name = "Block";
        ret.desc = abData.description;
        ret.abilityType = "Reaction";
        ret.range = "Self";
        float temp = 2.5f * (1 - 0.02f * abilityPower) * (1.2f - level * 0.2f);
        temp = MathF.Round(temp, 2);
        ret.cost = temp + " (base 2.5) Stamina per point of incoming damage";
        ret.targetType = "Self";
        ret.special =
            "Negate the damage from an incoming attack entirely by paying Stamina. Fails if insufficient Stamina remains.";
        ret.apEffect = "Stamina cost reduced by 0.2% per AP";
        ret.levelEffect = "Stamina cost reduced by 20% per Level after AP";
        ret.icon = Resources.Load<Sprite>("Icons/Block");
        return ret;
    }
}
