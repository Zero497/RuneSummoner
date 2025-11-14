using UnityEngine;

public class Rage : PassiveAbility
{
    private bool takenDamageSinceLastTurn = false;

    private ActionPriorityWrapper<UnitBase, float> onTakeDamage;

    private ActionPriorityWrapper<UnitBase> onTurnStarted;
    
    /*
        Expects:
            Unit 0: unit to apply to
            Int 1: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onTakeDamage = new ActionPriorityWrapper<UnitBase, float>();
        onTakeDamage.priority = 96;
        onTakeDamage.action = OnTakeDamage;
        source.myEvents.onTakeDamage.Subscribe(onTakeDamage);
        onTurnStarted = new ActionPriorityWrapper<UnitBase>();
        onTurnStarted.priority = 32;
        onTurnStarted.action = OnTurnStarted;
    }
    
    public override string GetAbilityName()
    {
        return "Rage";
    }

    private void OnTurnStarted(UnitBase myUnit)
    {
        if (!takenDamageSinceLastTurn)
        {
            SendData physAUp = new SendData("physmagstatchange");
            physAUp.AddStr("physicalattack");
            physAUp.AddFloat(1);
            myUnit.RemoveEffect(physAUp);
            SendData magAUp = new SendData("physmagstatchange");
            magAUp.AddStr("magicalattack");
            magAUp.AddFloat(1);
            myUnit.RemoveEffect(magAUp);
            if (source.GetPassive(new SendData((int)PassiveAbility.PassiveAbilityDes.rageBoost)) != null)
            {
                SendData apUp = new SendData("apup");
                apUp.AddFloat(1);
                myUnit.RemoveEffect(apUp);
            }
        }
        else
        {
            takenDamageSinceLastTurn = false;
        }
    }

    private void OnTakeDamage(UnitBase myUnit, float damage)
    {
        takenDamageSinceLastTurn = true;
        SendData physAUp = new SendData("physmagstatchange");
        physAUp.AddStr("physicalattack");
        physAUp.AddUnit(source);
        physAUp.AddFloat(5*level);
        physAUp.AddFloat(1);
        myUnit.AddEffect(physAUp);
        SendData magAUp = new SendData("physmagstatchange");
        magAUp.AddStr("magicalattack");
        magAUp.AddUnit(source);
        magAUp.AddFloat(5*level);
        magAUp.AddFloat(1);
        myUnit.AddEffect(magAUp);
        PassiveAbility rb = source.GetPassive(new SendData((int)PassiveAbility.PassiveAbilityDes.rageBoost));
        if (rb != null)
        {
            SendData apUp = new SendData("apup");
            apUp.AddFloat(2*rb.GetLevel());
            apUp.AddFloat(1);
            myUnit.AddEffect(apUp);
        }
    }
}
