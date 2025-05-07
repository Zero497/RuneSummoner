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
        HexTileUtility.DjikstrasNode best = myMoveLocations[0];
        int bestDist = 1000;
        foreach (UnitBase enUnit in MainCombatManager.manager.allFriendly)
        {
            if (inSight.Contains(enUnit.currentPosition))
            {
                foreach (HexTileUtility.DjikstrasNode moveloc in myMoveLocations)
                {
                    int nb = HexTileUtility.GetTileDistance(moveloc.location, enUnit.currentPosition);
                    if (nb < bestDist)
                    {
                        bestDist = nb; 
                        best = moveloc; 
                    }
                    if (nb == bestDist)
                    {
                        best = moveloc;
                    }
                    
                }
            }
        }

        if (bestDist < 1000)
        {
            MoveController.mControl.Move(best.location, out var co, OnMoveStopped);
        }
        else
        {
            int rand = UnityEngine.Random.Range(0, myMoveLocations.Count);
            MoveController.mControl.Move(myMoveLocations[rand].location, out var co, OnMoveStopped);
        }
        
    }

    private void OnMoveStopped(bool reason)
    {
        if (!reason)
        {
            OnTurnStarted(curUnit);
            return;
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
                        foreach (ActiveAbility ability in curUnit.activeAbilities)
                        {
                            bool ran = false;
                            switch (ability.abilityData.targetType)
                            {
                                case AbilityData.TargetType.singleTargetEnemy:
                                    SendData data = new SendData(enUnit.currentPosition);
                                    ran = ability.RunAction(data);
                                    break;
                                default:
                                    //TODO other types
                                    break;
                            }

                            if (ran)
                            {
                                break;
                            }
                        }
                        MainCombatManager.manager.EndTurn();
                        return;
                    }
                }
            }
        }
        MainCombatManager.manager.EndTurn();
    }
}
