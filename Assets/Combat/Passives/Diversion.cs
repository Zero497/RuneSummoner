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
    
    public override string GetAbilityName()
    {
        return "Diversion";
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
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Diversion\n";
        ret += 
            "When this Unit attacks an enemy Unit it deals 75% reduced damage and that enemy Unit gains Marked "+(5*level)+" (5 base).\n";
        ret += "Level Effect: +5 Marked applied per Level.\n";
        return ret;
    }
}
