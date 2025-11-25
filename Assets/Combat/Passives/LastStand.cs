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
    
    public override string GetAbilityName()
    {
        return "Last Stand";
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
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Last Stand\n";
        ret += 
            "When this Unit would die for the first "+level+" (1 base) times, it is instead reduced to 1 Health and gains Invulnerable. It dies at the end of its next turn.\n";
        ret += "Level Effect: +1 saves from death per Level\n";
        return ret;
    }
}
