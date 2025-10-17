using UnityEngine;

public class Evasive : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttack;

    private ActionPriorityWrapper<UnitBase> onTurnEnded;

    private bool attackedThisTurn = false;

    /*
        Expects:
            String 0: name of passive ability
            Unit 0: unit to apply to
            Float 0: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onAttack = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>
        {
            priority = 80,
            action = OnAttack
        };
        source.myEvents.applyToOutgoingAttack.Subscribe(onAttack);
        onTurnEnded = new ActionPriorityWrapper<UnitBase>
        {
            priority = 64,
            action = OnTurnEnded
        };
        source.myEvents.onTurnEnded.Subscribe(onTurnEnded);
    }

    private void OnAttack(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        attackedThisTurn = true;
    }

    private void OnTurnEnded(UnitBase myUnit)
    {
        if (!attackedThisTurn)
        {
            SendData evadeData = new SendData(source);
            evadeData.AddStr("evade");
            evadeData.AddFloat(5*level);
            source.AddEffect(evadeData);
        }
        attackedThisTurn = false;
    }
}
