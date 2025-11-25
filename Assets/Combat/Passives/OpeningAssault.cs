using UnityEngine;

public class OpeningAssault : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase> onTurnStarted;

    private ActionPriorityWrapper<UnitBase> onTurnEnd;

    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttack;

    private float addedMAValue = 0;

    private float addedPAValue = 0;
    
    private int turnsSinceAttacked = 3;

    private bool hasBuff = false;

    /*
        Expects:
            Unit 0: unit to apply to
            Int 1: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onTurnStarted = new ActionPriorityWrapper<UnitBase>();
        onTurnStarted.priority = 32;
        onTurnStarted.action = OnTurnStarted;
        onAttack = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        onAttack.priority = 80;
        onAttack.action = OnAttack;
        source.myEvents.applyToOutgoingAttack.Subscribe(onAttack);
        source.myEvents.onTurnStarted.Subscribe(onTurnStarted);
        onTurnEnd = new ActionPriorityWrapper<UnitBase>();
        onTurnEnd.priority = 16;
        onTurnEnd.action = OnTurnEnd;
        source.myEvents.onTurnEnded.Subscribe(onTurnEnd);
    }
    
    public override string GetAbilityName()
    {
        return "Opening Assault";
    }

    private void OnAttack(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        turnsSinceAttacked = 0;
        addedMAValue = myUnit.magicalAttack * 0.05f * level;
        addedPAValue = myUnit.physicalAttack * 0.05f * level;
        myUnit.myCombatStats.AddMagicalAttack(-addedMAValue);
        myUnit.myCombatStats.AddPhysicalAttack(-addedPAValue);
        hasBuff = false;
    }

    private void OnTurnEnd(UnitBase myUnit)
    {
        turnsSinceAttacked++;
    }

    private void OnTurnStarted(UnitBase myUnit)
    {
        if (turnsSinceAttacked >= 3 && !hasBuff)
        {
            addedMAValue = myUnit.magicalAttack * 0.05f * level;
            addedPAValue = myUnit.physicalAttack * 0.05f * level;
            myUnit.myCombatStats.AddMagicalAttack(addedMAValue);
            myUnit.myCombatStats.AddPhysicalAttack(addedPAValue);
            hasBuff = true;
        }
    }
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Opening Assault\n";
        ret += 
            "Until this Unit makes its first attack in a combat, its Physical Attack and Magical Attack are increased by "+(50*level)+"% (50% base). This bonus resets after 3 turns where the Unit did not attack.\n";
        ret += "Level Effect: +50% Physical and Magical Attack increase per Level.\n";
        return ret;
    }
}
