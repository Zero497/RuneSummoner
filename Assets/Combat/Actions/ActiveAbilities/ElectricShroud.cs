using UnityEngine;

public class ElectricShroud : ActiveAbility
{
    private ActionPriorityWrapper<UnitBase> onTurnStarted;

    private Effect myEffect;

    private int spikesLastApplied;

    public override void Initialize(SendData sendData)
    {
        base.Initialize(sendData);
        onTurnStarted = new ActionPriorityWrapper<UnitBase>();
        onTurnStarted.priority = 42;
        onTurnStarted.action = OnTurnStarted;
        source.myEvents.onTurnStarted.Subscribe(onTurnStarted);
    }
    
    public override bool RunAction(SendData actionData)
    {
        if (!source.PayCost(this)) return false;
        int spikesApplied = GetSpikesToApply();
        spikesLastApplied = spikesApplied;
        SendData data = new SendData("spikes");
        data.AddFloat(spikesApplied);
        data.AddFloat((int) AttackData.DamageType.Magic);
        data.AddFloat((int) AttackData.Element.electro);
        myEffect = source.AddEffect(data);
        return true;
    }

    private int GetSpikesToApply()
    {
        int spikesApplied = 20;
        spikesApplied += Mathf.FloorToInt(spikesApplied * 0.05f * source.abilityPower);
        spikesApplied += Mathf.FloorToInt(spikesApplied * 0.5f * (level - 1));
        return spikesApplied;
    }

    private void OnTurnStarted(UnitBase myUnit)
    {
        if (myEffect != null && spikesLastApplied > 0)
        {
            myEffect.AddStacks(-spikesLastApplied);
            spikesLastApplied = 0;
            myEffect = null;
        }
    }

    public override bool PrepAction()
    {
        throw new System.NotImplementedException();
    }

    public override bool RushCompletion()
    {
        throw new System.NotImplementedException();
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }
}
