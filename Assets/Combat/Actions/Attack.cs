using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : ActiveAbility
{
    public AttackData data;

    public override bool RunAction(Vector3Int target)
    {
        throw new System.NotImplementedException();
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

    public override void Initialize(string dataname)
    {
        data = Resources.Load<AttackData>("AttackData/" + dataname);
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
