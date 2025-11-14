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
}
