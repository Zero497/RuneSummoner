using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BasicAggressive", menuName = "FSMNodes/BasicAggressive")]
public class BasicAggressiveNode : FSMNode
{
    private UnitBase curUnit;
    private List<Vector3Int> inSight;

    public override void OnEnter(UnitBase unit)
    {
        return;
    }

    public override void OnExit(UnitBase unit)
    {
        return;
    }

    public override void OnTurnStarted(UnitBase unit)
    {
        curUnit = unit;
        List<HexTileUtility.DjikstrasNode> myMoveLocations = MoveController.mControl.InitMovement(unit, false);
        inSight =
            VisionManager.visionManager.DjikstrasSightCheck(unit.currentPosition, unit.sightRadius);
        foreach (UnitBase enUnit in MainCombatManager.manager.allFriendly)
        {
            if (inSight.Contains(enUnit.currentPosition))
            {
                List<Vector3Int> adjacents = HexTileUtility.GetAdjacentTiles(enUnit.currentPosition);
                foreach (Vector3Int adj in adjacents)
                {
                    if (MoveController.mControl.IsValidMove(adj))
                    {
                        MoveController.mControl.Move(adj, out var routine, OnMoveStopped);
                        Debug.Log("Moving to: "+adj);
                        return;
                    }
                }
            }
        }

        int rand = UnityEngine.Random.Range(0, myMoveLocations.Count);
        MoveController.mControl.Move(myMoveLocations[rand].location, out var co, OnMoveStopped);
        Debug.Log("Moving to: "+myMoveLocations[rand].location);
    }

    private void OnMoveStopped(bool reason)
    {
        if (!reason)
        {
            OnTurnStarted(curUnit);
        }
        foreach (UnitBase enUnit in MainCombatManager.manager.allFriendly)
        {
            if (inSight.Contains(enUnit.currentPosition))
            {
                List<Vector3Int> adjacents = HexTileUtility.GetAdjacentTiles(enUnit.currentPosition);
                foreach (Vector3Int adj in adjacents)
                {
                    if (curUnit.currentPosition.Equals(adj))
                    {
                        //TODO
                        MainCombatManager.manager.EndTurn();
                    }
                }
            }
        }
        MainCombatManager.manager.EndTurn();
    }
}
