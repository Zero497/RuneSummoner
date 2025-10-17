using UnityEngine;

public class Diversion : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttack;

    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onAttack = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        onAttack.priority = 40;
        onAttack.action = OnAttack;
        source.myEvents.applyToOutgoingAttack.Subscribe(onAttack);
    }

    private void OnAttack(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        attack.damage -= attack.baseDamage * 0.75f;
        SendData markData = new SendData(attack.target);
        markData.AddUnit(myUnit);
        markData.AddStr("marked");
        markData.AddFloat(5*level);
        attack.target.AddEffect(markData);
    }
}
