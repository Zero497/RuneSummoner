using UnityEngine;

public class Lumbering : PassiveAbility
{
    private ActionPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode> onMoveEnd;

    private ActionPriorityWrapper<UnitBase> onTurnStart;

    private int tilesMovedSinceLastTurn = 0;
    
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        source.myCombatStats.AddPhysicalAttack(source.myCombatStats.getPhysicalAttack(true)*0.75f*level);
        source.myCombatStats.AddMagicalAttack(source.myCombatStats.getMagicalAttack(true)*0.5f*level);
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
        return "Lumbering";
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
        source.myCombatStats.AddPhysicalAttack(-source.myCombatStats.getPhysicalAttack(true)*0.15f*level*tilesMoved);
        source.myCombatStats.AddMagicalAttack(-source.myCombatStats.getMagicalAttack(true)*0.1f*level*tilesMoved);
    }
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Lumbering";
        ret.desc = 
            "This Unit has +"+(75*level)+"% (75% base) Physical Attack and +"+(50*level)+"% (50% base) Magical Attack. This Unit loses -"+(level*15)+"% (15% base) Physical Attack and -"+(level*10)+"% (10% base) Magical Attack for each tile it moves until the start of its next turn.";
        ret.levelEffect = "+75% Physical Defense and +50% Magical Defense per Level and -15% Physical Defense and -10% Magical Defense per tile moved per Level.";
        return ret;
    }
}
