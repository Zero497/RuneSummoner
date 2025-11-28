using UnityEngine;

public class BristlingSpines : PassiveAbility
{
    private SpikesUndispellable myEffect;

    private ActionPriorityWrapper<UnitBase, string, float> onPhysicalDefenseChanged;
    
    /*
        Expects:
            Unit 0: unit to apply to
            Float 0: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        myEffect = new SpikesUndispellable();
        SendData d2 = new SendData(source);
        d2.AddStr("spikes");
        d2.AddFloat(Mathf.FloorToInt(source.physicalDefence*0.5f*level));
        d2.AddFloat((int) AttackData.DamageType.Physical);
        d2.AddFloat((int) source.baseData.DefaultDamageElement);
        myEffect.Initialize(d2);
        source.AddEffect(myEffect);
        onPhysicalDefenseChanged = new ActionPriorityWrapper<UnitBase, string, float>();
        onPhysicalDefenseChanged.priority = 80;
        onPhysicalDefenseChanged.action = OnPhysicalDefenseChanged;
    }
    
    public override string GetAbilityName()
    {
        return "Bristling Spines";
    }

    private void OnPhysicalDefenseChanged(UnitBase myUnit, string statChanged, float change)
    {
        if (statChanged.Equals("physicaldefense"))
        {
            myEffect.AddStacks(Mathf.FloorToInt(change*0.5f*level+((source.physicalDefence*0.5f*level)%1)));
        }
    }
    
    public static PassiveText GetFullText(int level, AttackData.Element defaultElement)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Bristling Spines";
        ret.desc = "This Unit perpetually has Spikes Physical "+defaultElement+" equal to "+(level*50)+"% (50% base) of its Physical Defense. This buff cannot be removed.";
        ret.levelEffect = "+50% of Physical Defense added as Spikes per Level.";
        return ret;
    }
}
