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
}
