using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : ActiveAbility
{
    public AttackData data;

    public UnitBase source;

    public override bool RunAction(SendData sentData)
    {
        AttackMessageToTarget outgoingAttack = PrepareMessage();
        switch (data.MyAbilityData.targetType)
        {
            case AbilityData.TargetType.self:
                source.ReceiveAttack(outgoingAttack);
                break;
            case AbilityData.TargetType.aoeNeutral:
                
                break;
        }
        return true;
    }

    private AttackMessageToTarget PrepareMessage()
    {
        AttackMessageToTarget retval = new AttackMessageToTarget(data);
        float mult = 1;
        if (retval.damageType == AttackData.DamageType.Magic)
        {
            mult += source.magicalAttack / 100;
        }
        else if (retval.damageType == AttackData.DamageType.Physical)
        {
            mult += source.physicalAttack / 100;
        }
        retval.damage *= mult;
        source.ModifyOutgoingAttack(retval);
        return retval;
    }

    public override bool PrepAction()
    {
        throw new System.NotImplementedException();
    }

    public override bool RushCompletion()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsFree()
    {
        throw new System.NotImplementedException();
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    public override void Initialize(SendData sendData)
    {
        source = sendData.unitData[0];
        base.Initialize(sendData);
    }

    public class AttackMessageToTarget
    {
        public AbilityData MyAbilityData;
    
        public AttackData.DamageType damageType;
    
        public UnitData.Element element;

        public float damage;
    
        public Effect effectToApplyTarget;

        public AttackMessageToTarget(AttackData defaultData)
        {
            MyAbilityData = defaultData.MyAbilityData;
            damageType = defaultData.damageType;
            element = defaultData.element;
            damage = defaultData.damage;
            effectToApplyTarget = Effect.GetEffect(defaultData.effectToApplyTarget);
        }
    }
}
