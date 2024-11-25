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
        foreach (UnitBase unit in sentData.unitData)
        {
            unit.ReceiveAttack(outgoingAttack);
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
        OverlayManager.instance.ClearOverlays();
        OverlayManager.instance.CreateOverlay(HexTileUtility.DjikstrasGetTilesInRange(TurnController.controller.mainMap, source.currentPosition, data.MyAbilityData.range, 1),"AttackOverlay");
        return true;
    }

    public override bool RushCompletion()
    {
        return true;
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    private string ParseDescription(string description)
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
