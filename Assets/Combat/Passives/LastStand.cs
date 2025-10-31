using UnityEngine;

public class LastStand : PassiveAbility
{
    private int deathsRemaining;

    private bool dieNextTurn;

    private ActionPriorityWrapper<UnitBase> onTurnEnd;

    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        deathsRemaining = level;
        dieNextTurn = false;
        onTurnEnd = new ActionPriorityWrapper<UnitBase>();
        onTurnEnd.priority = 120;
        onTurnEnd.action = OnTurnEnd;
        source.myEvents.onTurnEnded.Subscribe(onTurnEnd);
    }

    public bool HasUsesRemaining()
    {
        return deathsRemaining > 0;
    }

    public void ReplaceDeath()
    {
        deathsRemaining--;
        source.myCombatStats.SetCurrentHealth(1);
        SendData invulnData = new SendData("invulnerable");
        invulnData.AddFloat(1);
        source.AddEffect(invulnData);
        dieNextTurn = true;
    }

    private void OnTurnEnd(UnitBase myUnit)
    {
        if (dieNextTurn)
        {
            myUnit.Die();
        }
    }
}
