using UnityEngine;

public class OpeningAssault : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttack;

    private ActionPriorityWrapper<UnitBase> onTurnEnd;

    private float addedMAValue = 0;

    private float addedPAValue = 0;
    
    private int turnsSinceAttacked = 3;

    /*
        Expects:
            Unit 0: unit to apply to
            Int 1: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onAttack = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        onAttack.priority = 3;
        onAttack.action = OnAttack;
        source.myEvents.applyToOutgoingAttack.Subscribe(onAttack);
        onTurnEnd = new ActionPriorityWrapper<UnitBase>();
        onTurnEnd.priority = 16;
        onTurnEnd.action = OnTurnEnd;
        source.myEvents.onTurnEnded.Subscribe(onTurnEnd);
    }
    
    public override string GetAbilityName()
    {
        return "Opening Assault";
    }

    private void OnTurnEnd(UnitBase myUnit)
    {
        turnsSinceAttacked++;
        myUnit.myCombatStats.AddMagicalAttack(-addedMAValue);
        addedMAValue = 0;
        myUnit.myCombatStats.AddPhysicalAttack(-addedPAValue);
        addedPAValue = 0;
    }

    private void OnAttack(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        if (turnsSinceAttacked >= 3)
        {
            addedMAValue = myUnit.magicalAttack * 0.05f * level;
            addedPAValue = myUnit.physicalAttack * 0.05f * level;
            myUnit.myCombatStats.AddMagicalAttack(addedMAValue);
            myUnit.myCombatStats.AddPhysicalAttack(addedPAValue);
        }

        turnsSinceAttacked = -1;
    }
}
