using UnityEngine;

public class ClockworkDefenses : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase> onTurnStart;

    private ActionPriorityWrapper<UnitBase, float> onTakeDamage;

    private bool damagedSinceLastTurn;

    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onTurnStart = new ActionPriorityWrapper<UnitBase>();
        onTurnStart.priority = 16;
        onTurnStart.action = OnTurnStart;
        source.myEvents.onTurnStarted.Subscribe(onTurnStart);
        onTakeDamage = new ActionPriorityWrapper<UnitBase, float>();
        onTakeDamage.priority = 72;
        onTakeDamage.action = OnTakeDamage;
        source.myEvents.onTakeDamage.Subscribe(onTakeDamage);
    }
    
    public override string GetAbilityName()
    {
        return "Clockwork Defenses";
    }

    private void OnTakeDamage(UnitBase myUnit, float damage)
    {
        if (damage > 0)
        {
            damagedSinceLastTurn = true;
        }
    }

    private void OnTurnStart(UnitBase myUnit)
    {
        if (damagedSinceLastTurn)
        {
            SendData data = new SendData("physmagstatchange");
            data.AddUnit(myUnit);
            data.AddStr("physicaldefense");
            data.AddFloat(5*level);
            data.AddFloat(1);
            myUnit.AddEffect(data);
            data.strData[1] = "magicaldefense";
            myUnit.AddEffect(data);
            damagedSinceLastTurn = false;
        }
        else
        {
            SendData data = new SendData("physmagstatchange");
            data.AddStr("physicaldefense");
            data.AddFloat(1);
            data.AddFloat(1);
            myUnit.RemoveEffect(data);
            data.strData[1] = "magicaldefense";
            myUnit.RemoveEffect(data);
        }
    }
}
