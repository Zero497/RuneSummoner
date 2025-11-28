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
    
    public override string GetAbilityName()
    {
        return "Entrenched";
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
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Entrenched";
        ret.desc = 
            "This Unit has +"+(level*50)+"% (50% base) Physical Defense and +"+(level*25)+"% (25% base) Magical Defense. This Unit loses -"+(10*level)+"% (10% base) Physical Defense and -"+(5*level)+"% Magical Defense for each tile it moves until the start of its next turn.";
        ret.levelEffect = "+50% Physical Defense and +25% Magical Defense per Level and -10% Physical Defense and -5% Magical Defense per tile moved per Level.";
        return ret;
    }
}
