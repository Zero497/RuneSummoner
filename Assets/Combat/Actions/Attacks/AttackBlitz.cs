using System;
using System.Collections.Generic;
using UnityEngine;

//VALID IDS: BlitzMagic, BlitzPhysical
public class AttackBlitz : Attack
{
    private int numAttacks;

    private bool isPhysical;
    
    public override bool RunAction(SendData sentData)
    {
        Attack basic = getBasicAttack();
        if (basic == null) return false;
        numAttacks = (int) (source.currentStamina / basic.abilityData.staminaCost);
        for (int i = 0; i < numAttacks; i++)
        {
            source.PayCost(basic);
        }
        return base.RunAction(sentData);
    }

    public override bool RunSingleTarget(Func<int, bool> validTarget, Vector3Int position, float mod = 1)
    {
        UnitBase unitAtPosition = MainCombatManager.manager.getUnitAtPosition(position);
        Attack myAttack = getBasicAttack();
        if (unitAtPosition != null && validTarget(unitAtPosition.myTeam))
        {
            for(int i = 0; i<numAttacks; i++)
                myAttack.RunAttack(new List<UnitBase>{unitAtPosition}, mod*0.5f);
            return true;
        }
        return false;
    }

    public override void Initialize(SendData sendData)
    {
        if (sendData.strData[1].Equals("physical"))
            isPhysical = true;
        else
            isPhysical = false;
        base.Initialize(sendData);
    }

    private Attack getBasicAttack()
    {
        foreach (ActiveAbility ability in source.activeAbilities)
        {
            if ((ability.id.Equals("PhysicalMelee") && isPhysical) || (ability.id.Equals("MagicalMelee") && !isPhysical))
            {
                return ability as Attack;
            }
        }
        Debug.Log("Failed to find appropriate ability type!");
        return null;
    }
}
