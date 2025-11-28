using UnityEngine;

public class Stealth : PassiveAbility
{
    public ActionPriorityWrapper<UnitBase, UnitBase> onRevealed;

    /*
        Expects:
            Unit 0: unit to apply to
            Int 1: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onRevealed = new ActionPriorityWrapper<UnitBase, UnitBase>();
        onRevealed.priority = 80;
        onRevealed.action = OnRevealed;
        source.myEvents.onPositionRevealed.Subscribe(onRevealed);
    }
    
    public override string GetAbilityName()
    {
        return "stealth";
    }
    
    private void OnRevealed(UnitBase myUnit, UnitBase revealer)
    {
        if (HexTileUtility.GetTileDistance(myUnit.currentPosition, revealer.currentPosition) >
            (Mathf.Max(1, 8 - 2 * level)))
        {
            if (myUnit.isFriendly)
            {
                VisionManager.visionManager.visibleFriendlyUnits.Remove(myUnit);
            }
            else
            {
                myUnit.ConcealMe(myUnit);
                VisionManager.visionManager.visibleEnemyUnits.Remove(myUnit);
            }
        }
    }
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Stealth";
        ret.desc = 
            "This Unit is invisible to enemy Units that are further than "+(8-2*level)+" (6 base) tiles away from this Unit.";
        ret.levelEffect = "Enemies must be +2 tiles closer to see Unit per Level (minimum of 1).";
        return ret;
    }
}
