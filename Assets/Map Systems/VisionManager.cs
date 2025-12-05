using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class VisionManager : MonoBehaviour
{
    public static VisionManager visionManager;

    public Tilemap hiddenMap;

    public Tilemap mainMap;

    public Tile hiddenTile;

    public UnityEvent<Vector3Int> positionRevealed = new UnityEvent<Vector3Int>();

    public UnityEvent<Vector3Int> positionConcealed = new UnityEvent<Vector3Int>();

    private Dictionary<Vector3Int, HashSet<String>> revealedPositions = new Dictionary<Vector3Int, HashSet<String>>();

    private Dictionary<Vector3Int, HashSet<String>> positionsKnownToEnemy =
        new Dictionary<Vector3Int, HashSet<string>>();

    public Dictionary<UnitBase, HashSet<String>> visibleEnemyUnits = new Dictionary<UnitBase, HashSet<String>>();

    public Dictionary<UnitBase, HashSet<String>> visibleFriendlyUnits = new Dictionary<UnitBase, HashSet<String>>();

    private void Awake()
    {
        if(visionManager != null) Destroy(visionManager.gameObject);
        visionManager = this;
    }

    public bool isRevealed(Vector3Int position)
    {
        return revealedPositions.ContainsKey(position);
    }

    //reveal the specified position
    public void RevealPosition(Vector3Int position)
    {
        positionRevealed.Invoke(position);
        hiddenMap.SetTile(position, null);
    }

    //conceal the specified position
    public void ConcealPosition(Vector3Int position)
    {
        positionConcealed.Invoke(position);
        revealedPositions.Remove(position);
        hiddenMap.SetTile(position, hiddenTile);
    }

    public HashSet<String> GetViewers(Vector3Int position)
    {
        if (!revealedPositions.ContainsKey(position)) return new HashSet<string>();
        return revealedPositions[position];
    }

    public HashSet<String> GetViewersE(Vector3Int position)
    {
        if (!positionsKnownToEnemy.ContainsKey(position)) return new HashSet<string>();
        return positionsKnownToEnemy[position];
    }

    /*
     * conceal in a specified radius around a target
     */
    public void ConcealInRadius (String viewerID, int sightRadius, Vector3Int center)
    {
        List<Vector3Int> oldSight = DjikstrasSightCheck(center, sightRadius, true);
        foreach (Vector3Int tile in oldSight)
        {
            if (revealedPositions.ContainsKey(tile))
            {
                revealedPositions[tile].Remove(viewerID);
                if (revealedPositions[tile].Count == 0)
                {
                    revealedPositions.Remove(tile);
                    ConcealPosition(tile);
                }
            }
        }
    }

    //reveal all tiles in a specified radius around a target
    public void RevealInRadius(UnitBase unit, float sightRadius, Vector3Int center)
    {
        List<Vector3Int> revealTiles = DjikstrasSightCheck(center, sightRadius);
        foreach (Vector3Int tile in revealTiles)
        {
            if(!revealedPositions.ContainsKey(tile))
                revealedPositions.Add(tile, new HashSet<string>());
            revealedPositions[tile].Add(unit.myId);
            RevealPosition(tile);
            foreach (UnitBase eUnit in MainCombatManager.manager.allEnemy)
            {
                if (tile == eUnit.currentPosition)
                {
                    if (!visibleEnemyUnits.ContainsKey(eUnit))
                    {
                        visibleEnemyUnits.Add(eUnit, new HashSet<string>{unit.myId});
                    }
                    else
                    {
                        visibleEnemyUnits[eUnit].Add(unit.myId);
                    }
                    eUnit.myEvents.onPositionRevealed.Invoke(eUnit, unit);
                }
            }
        }
    }

    public void UpdateEnemyVision(UnitBase unit)
    {
        List<Vector3Int> posTemp = new List<Vector3Int>();
        foreach (KeyValuePair<Vector3Int, HashSet<String>> kvp in positionsKnownToEnemy)
        {
            if (kvp.Value.Contains(unit.myId))
            {
                posTemp.Add(kvp.Key);
            }
        }

        foreach (Vector3Int pos in posTemp)
        {
            positionsKnownToEnemy[pos].Remove(unit.myId);
            foreach (UnitBase fUnit in MainCombatManager.manager.allFriendly)
            {
                if (visibleFriendlyUnits.ContainsKey(fUnit))
                {
                    if (pos == fUnit.currentPosition && visibleFriendlyUnits[fUnit].Contains(unit.myId))
                    {
                        if (visibleFriendlyUnits[fUnit].Count == 1)
                        {
                            visibleFriendlyUnits.Remove(fUnit);
                            fUnit.myEvents.onPositionConcealed.Invoke(fUnit);
                        }
                        else
                        {
                            visibleFriendlyUnits[fUnit].Remove(unit.myId);
                        }
                            
                    }
                }
            }
        }
        List<Vector3Int> revealTiles = DjikstrasSightCheck(unit.currentPosition,unit.baseData.sightRadius);
        foreach (Vector3Int tile in revealTiles)
        {
            if(!positionsKnownToEnemy.ContainsKey(tile))
                positionsKnownToEnemy.Add(tile, new HashSet<string>());
            positionsKnownToEnemy[tile].Add(unit.myId);
            foreach (UnitBase fUnit in MainCombatManager.manager.allFriendly)
            {
                if (tile == fUnit.currentPosition)
                {
                    if (!visibleFriendlyUnits.ContainsKey(fUnit))
                    {
                        visibleFriendlyUnits.Add(fUnit, new HashSet<string>{unit.myId});
                    }
                    else
                    {
                        visibleFriendlyUnits[fUnit].Add(unit.myId);
                    }
                    fUnit.myEvents.onPositionRevealed.Invoke(fUnit, unit);
                }
            }
        }
    }

    public void UpdateFriendlyVision(UnitBase unit)
    {
        List<Vector3Int> posTemp = new List<Vector3Int>();
        foreach (KeyValuePair<Vector3Int, HashSet<String>> kvp in revealedPositions)
        {
            if (kvp.Value.Contains(unit.myId))
            {
                posTemp.Add(kvp.Key);
            }
        }

        foreach (Vector3Int pos in posTemp)
        {
            revealedPositions[pos].Remove(unit.myId);
            if (revealedPositions[pos].Count == 0)
            {
                ConcealPosition(pos);
                revealedPositions.Remove(pos);
            }
            foreach (UnitBase eUnit in MainCombatManager.manager.allEnemy)
            {
                if (visibleEnemyUnits.ContainsKey(eUnit))
                {
                    if (pos == eUnit.currentPosition && visibleEnemyUnits[eUnit].Contains(unit.myId))
                    {
                        if (visibleEnemyUnits[eUnit].Count == 1)
                        {
                            visibleEnemyUnits.Remove(eUnit);
                            eUnit.myEvents.onPositionConcealed.Invoke(eUnit);
                        }
                        else
                        {
                            visibleEnemyUnits[eUnit].Remove(unit.myId);
                        }
                    }
                }
            }
        }
        RevealInRadius(unit, unit.sightRadius, unit.currentPosition);
    }
    
    public List<Vector3Int> DjikstrasSightCheck(Vector3Int start, float sightRadius, bool ignoreSightBlocking = false)
    {
        List<Vector3Int> viewableList = new List<Vector3Int>();
        List<HexTileUtility.DjikstrasNode> allInRange =
            HexTileUtility.DjikstrasGetTilesInRange(mainMap, start, sightRadius, -1);
        List<Vector3Int> viewBlockerList = new List<Vector3Int>();
        List<Vector3Int> addBlockers = GetAddBlockers(start);
        foreach (HexTileUtility.DjikstrasNode val in allInRange)
        {
            if (mainMap.GetTile<DataTile>(val.location).data.lineOfSightBlocking)
            {
                viewBlockerList.Add(val.location);
            }
            if (addBlockers.Contains(val.location))
            {
                viewBlockerList.Add(val.location);
                continue;
            }
            viewableList.Add(val.location);
        }
        if (ignoreSightBlocking) return viewableList;
        int i = 0;
        while (i < viewableList.Count)
        {
            Vector3Int location = viewableList[i];
            if (!IsInView(location, start, viewBlockerList))
            {
                viewableList.RemoveAt(i);
            }
            else i++;
        }
        return viewableList;
    }

    private List<Vector3Int> GetAddBlockers(Vector3Int start)
    {
        List<Vector3Int> ret = new List<Vector3Int>();
        List<Vector3Int> adj = HexTileUtility.GetAdjacentTiles(start);
        bool mod2 = start.y % 2 == 0;
        DataTile t1 = mainMap.GetTile<DataTile>(adj[0]);
        DataTile t2 = mainMap.GetTile<DataTile>(adj[1]);
        DataTile t3 = mainMap.GetTile<DataTile>(adj[2]);
        DataTile t4 = mainMap.GetTile<DataTile>(adj[3]);
        DataTile t5 = mainMap.GetTile<DataTile>(adj[4]);
        DataTile t6 = mainMap.GetTile<DataTile>(adj[5]);
        Vector3Int toAdd;
        if (t1 != null && t1.data.lineOfSightBlocking)
        {
            if (t2 != null && t2.data.lineOfSightBlocking)
            {
                if (mod2) toAdd = new Vector3Int(adj[0].x + 1, adj[0].y + 1);
                else toAdd = new Vector3Int(adj[0].x, adj[0].y + 1);
                if(mainMap.HasTile(toAdd)) ret.Add(toAdd);
            }
            if (t6 != null && t6.data.lineOfSightBlocking)
            {
                toAdd = new Vector3Int(adj[0].x-1, adj[0].y);
                if(mainMap.HasTile(toAdd)) ret.Add(toAdd);
            }
        }
        if (t3 != null && t3.data.lineOfSightBlocking)
        {
            if (t2 != null && t2.data.lineOfSightBlocking)
            {
                if (mod2) toAdd = new Vector3Int(adj[2].x, adj[2].y + 1);
                else toAdd = new Vector3Int(adj[2].x+1, adj[2].y + 1);
                if(mainMap.HasTile(toAdd)) ret.Add(toAdd);
            }
            if (t4 != null && t4.data.lineOfSightBlocking)
            {
                if (mod2) toAdd = new Vector3Int(adj[2].x, adj[2].y - 1);
                else toAdd = new Vector3Int(adj[2].x+1, adj[2].y - 1);
                if(mainMap.HasTile(toAdd)) ret.Add(toAdd);
            }
        }
        if (t5 != null && t5.data.lineOfSightBlocking)
        {
            if (t4 != null && t4.data.lineOfSightBlocking)
            {
                if (mod2) toAdd = new Vector3Int(adj[4].x + 1, adj[4].y - 1);
                else toAdd = new Vector3Int(adj[4].x, adj[4].y - 1);
                if(mainMap.HasTile(toAdd)) ret.Add(toAdd);
            }
            if (t6 != null && t6.data.lineOfSightBlocking)
            {
                toAdd = new Vector3Int(adj[0].x-1, adj[0].y);
                if(mainMap.HasTile(toAdd)) ret.Add(toAdd);
            }
        }
        return ret;
    }
    
    

    //checks if a target tile is in view of the viewer
    public bool IsInView(Vector3Int target, Vector3Int viewer, List<Vector3Int> viewBlockers)
    {
        foreach (Vector3Int blocker in viewBlockers)
        {
            if(target == blocker) continue;
            if (blocker.y == viewer.y && target.y == blocker.y)
            {
                if (blocker.x < viewer.x && target.x < blocker.x)
                    return false;
                if (blocker.x > viewer.x && target.x > blocker.x)
                    return false;
            }
            else if (!HexTileUtility.isInLine(viewer, blocker))
            {
                if (HexTileUtility.isInLine(viewer, target))
                    continue;
                int q1 = HexTileUtility.GetQuadrant(blocker, target);
                if (HexTileUtility.GetQuadrant(viewer, blocker) == q1)
                    return false;
                q1 -= 1;
                if (q1 < 0) q1 = 5;
                if (HexTileUtility.isInLine(blocker, target) && HexTileUtility.GetQuadrant(viewer, blocker) ==
                    q1)
                    return false;

            }
            else if (HexTileUtility.isInLine(blocker, target) && HexTileUtility.isInLine(viewer, target))
            {
                int q1 = HexTileUtility.GetQuadrant(viewer, target);
                if (q1 == HexTileUtility.GetQuadrant(viewer, blocker))
                {
                    switch (q1)
                    {
                        case 0:
                            if (blocker.y % 2 == 0 && target.x < blocker.x) return false;
                            if (blocker.y % 2 != 0 && target.x <= blocker.x) return false;
                            break;
                        case 1:
                            if (blocker.y % 2 == 0 && target.x >= blocker.x) return false;
                            if (blocker.y % 2 != 0 && target.x > blocker.x) return false;
                            break;
                        case 3:
                            if (blocker.y % 2 == 0 && target.x >= blocker.x) return false;
                            if (blocker.y % 2 != 0 && target.x > blocker.x) return false;
                            break;
                        case 4:
                            if (blocker.y % 2 == 0 && target.x < blocker.x) return false;
                            if (blocker.y % 2 != 0 && target.x <= blocker.x) return false;
                            break;
                    }
                }
            }

        }

        return true;
    }

    

    
}
