using UnityEngine;

public class Entrenched : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode> onMoveEnd;

    private ActionPriorityWrapper<UnitBase> onTurnStart;

    private int tilesMovedSinceLastTurn = 0;
    
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        source.myCombatStats.AddPhysicalDefense(source.myCombatStats.getPhysicalDefense(true)*0.5f*level);
        source.myCombatStats.AddMagicalDefense(source.myCombatStats.getMagicalDefense(true)*0.25f*level);
        onMoveEnd = new ActionPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode>();
        onMoveEnd.priority = 42;
        onMoveEnd.action = OnMoveEnd;
        source.myEvents.onMoveEnd.Subscribe(onMoveEnd);
        onTurnStart = new ActionPriorityWrapper<UnitBase>();
        onTurnStart.priority = 42;
        onTurnStart.action = OnTurnStart;
        source.myEvents.onTurnStarted.Subscribe(onTurnStart);
    }

    private void OnMoveEnd(UnitBase myUnit, HexTileUtility.DjikstrasNode node)
    {
        int tileCnt = node.CountNodesInPath();
        tilesMovedSinceLastTurn += tileCnt;
        ChangeValues(tileCnt);
    }

    private void OnTurnStart(UnitBase myUnit)
    {
        ChangeValues(-tilesMovedSinceLastTurn);
        tilesMovedSinceLastTurn = 0;
    }

    private void ChangeValues(int tilesMoved)
    {
        source.myCombatStats.AddPhysicalDefense(-source.myCombatStats.getPhysicalDefense(true)*0.1f*level*tilesMoved);
        source.myCombatStats.AddMagicalDefense(-source.myCombatStats.getMagicalDefense(true)*0.05f*level*tilesMoved);
    }
}
