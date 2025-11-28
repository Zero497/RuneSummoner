using UnityEngine;

public class Adaptable : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> myAction;
    
    /*
        Expects:
            Unit 0: unit to apply to
            Float 0: level of ability
     */
    public override void Initialize(SendData data)
    {
        myAction = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        myAction.priority = 96;
        myAction.action = AddResistance;
        base.Initialize(data);
    }
    
    public override string GetAbilityName()
    {
        return "Adaptable";
    }

    private void AddResistance(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        SendData resData = new SendData(myUnit);
        resData.AddStr("resistant");
        resData.AddFloat(4*level);
        resData.AddFloat(-1);
        resData.AddFloat((int) attack.damageElement);
        myUnit.AddEffect(resData);
    }

    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Adaptable";
        ret.desc =  "When damaged, gain Resistant " + (level * 4) + " (base 4) against that damage's Element.";
        ret.levelEffect = "+4 Resistant per Level.";
        return ret;
    }
}
