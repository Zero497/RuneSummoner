using UnityEngine;

public class Resistant : Effect
{
    public int dtype = -1;

    public int dElement = -1;

    public ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> myAction;
    
    /*
     Expects:
        Unit 0: unit to apply to
        String 0: name of effect (resistant)
        Float 0: stacks to apply
        Float 1: damage type to resist (physical/magical, -1 for none)
        Float 2: element to resist (-1 for none)
    */
    public override void Initialize(SendData data)
    {
        stackDecayAmount = 1;
        stackDecayTime = 50;
        base.Initialize(data);
        dtype = (int) data.floatData[1];
        dElement = (int)data.floatData[2];
        myAction =
            new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        myAction.priority = 32;
        myAction.action = ModifyAttack;
        source.myEvents.onAttacked.Subscribe(myAction);
        
    }

    public override void RemoveEffect()
    {
        source.myEvents.onAttacked.Unsubscribe(myAction);
        base.RemoveEffect();
    }

    /*Expects:
        String 0: name of effect (resistant)
        Float 1: damage type to resist (physical/magical, -1 for none)
        Float 2: element to resist (-1 for none)
    */
    public override bool MatchForMerge(SendData data)
    {
        if (data.strData[0].Equals(effectName))
        {
            if ((int)data.floatData[1] == dtype && (int)data.floatData[2] == dElement)
            {
                return true;
            }
        }
        return false;
    }

    public override bool Equals(Effect effect)
    {
        if (effect == null) return false;
        if (effectName.Equals(effect.effectName))
        {
            Resistant res = (Resistant)effect;
            if (res.dtype == dtype && res.dElement == dElement)
            {
                return true;
            }
        }
        return false;
    }

    /*Expects:
        String 0: name of effect (resistant)
        Float 1: damage type to resist (physical/magical, -1 for none)
        Float 2: element to resist (-1 for none)
    */
    public override bool Equals(SendData data)
    {
        if (base.Equals(data))
        {
            if ((int)data.floatData[1] == dtype && (int)data.floatData[2] == dElement)
                return true;
        }
        return false;
    }

    private void ModifyAttack(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        if ((dtype == -1 || dtype == (int)attack.damageType) && (dElement == -1 || dElement == (int)attack.damageElement))
        {
            float def;
            if (attack.damageType == AttackData.DamageType.Physical)
                def = myUnit.physicalDefence;
            else
            {
                def = myUnit.magicalDefence;
            }
            def *= 0.1f * stacks;
            attack.damage -= def;
        }
    }
    
}
