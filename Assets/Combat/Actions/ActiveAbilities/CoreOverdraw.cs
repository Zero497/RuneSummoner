using UnityEngine;

public class CoreOverdraw : ActiveAbility
{
    public override void Initialize(SendData sendData)
    {
        base.Initialize(sendData);
        abilityData = Resources.Load<AbilityData>("AbilityData/Core OverdrawM");
    }

    public override string GetID()
    {
        return "Core Overdraw";
    }

    public override bool RunAction(SendData actionData)
    {
        if (!source.PayCost(this)) return false;
        if (abilityData.staminaCost > 0)
        {
            source.myCombatStats.AddMana(GetStaminaCost().flt * (0.3f+0.2f*level));
            source.TakeDamage(AttackData.DamageType.True, AttackData.Element.neutral, Mathf.Max(0, (0.6f-0.1f*level)*GetStaminaCost().flt));
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
        throw new System.NotImplementedException();
    }

    public override bool RushCompletion()
    {
        throw new System.NotImplementedException();
    }
}
