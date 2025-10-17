using System.Collections.Generic;
using UnityEngine;

public class Unstoppable : PassiveAbility
{
    private List<Vector3Int> mostRecentLine = new List<Vector3Int>();
    
    private ActionPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode> onMoved;

    private ActionPriorityWrapper<UnitBase> onTurnStarted;

    private ActionPriorityWrapper<UnitBase> onTurnEnd;
    
    /*
        Expects:
            Unit 0: unit to apply to
            Float 0: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onMoved = new ActionPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode>();
        onMoved.priority = 64;
        onMoved.action = OnMoved;
        source.myEvents.onMoveEnd.Subscribe(onMoved);
        onTurnStarted = new ActionPriorityWrapper<UnitBase>();
        onTurnStarted.priority = 64;
        onTurnStarted.action = OnTurnStated;
        source.myEvents.onTurnStarted.Subscribe(onTurnStarted);
        onTurnEnd = new ActionPriorityWrapper<UnitBase>();
        onTurnEnd.priority = 16;
        onTurnEnd.action = OnTurnEnd;
        source.myEvents.onTurnEnded.Subscribe(onTurnEnd);
    }

    private void OnTurnEnd(UnitBase myUnit)
    {
        SendData physDef = new SendData(myUnit);
        physDef.AddStr("physicaldefense");
        physDef.AddStr("physicaldefense");
        physDef.AddFloat(2*level);
        physDef.AddFloat(1);
        myUnit.AddEffect(physDef);
        SendData magDef = new SendData(myUnit);
        magDef.AddStr("magicaldefense");
        magDef.AddStr("magicaldefense");
        magDef.AddFloat(2*level);
        magDef.AddFloat(1);
        myUnit.AddEffect(magDef);
    }
    
    private void OnTurnStated(UnitBase myUnit)
    {
        mostRecentLine = new List<Vector3Int>();
    }

    private void OnMoved(UnitBase myUnit, HexTileUtility.DjikstrasNode moveTarget)
    {
        HexTileUtility.DjikstrasNode cur = moveTarget;
        List<Vector3Int> newLine = new List<Vector3Int>();
        while (cur != null)
        {
            if (HexTileUtility.isInLine(cur.location, newLine[0]) && !HexTileUtility.AreAdjacent(newLine[^2], cur.location))
            {
                newLine.Add(cur.location);
                cur = cur.parent;
            }
            else
            {
                break;
            }
        }

        if (mostRecentLine.Count > 0 && HexTileUtility.isInLine(newLine[0], mostRecentLine[^1]))
        {
            if (mostRecentLine.Count == 1 || !HexTileUtility.AreAdjacent(newLine[0], mostRecentLine[^1]))
            {
                for (int i = 0; i < mostRecentLine.Count; i++)
                {
                    newLine.Add(mostRecentLine[i]);
                }
            }
        }

        mostRecentLine = newLine;
    }
}
