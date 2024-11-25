using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VisionManager : MonoBehaviour
{
    public static VisionManager visionManager;

    public Tilemap hiddenMap;

    public Tilemap mainMap;

    public Tile hiddenTile;

    private Dictionary<Vector3Int, HashSet<String>> revealedPositions = new Dictionary<Vector3Int, HashSet<String>>();

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
        hiddenMap.SetTile(position, null);
    }

    //conceal the specified position
    public void ConcealPosition(Vector3Int position)
    {
        revealedPositions.Remove(position);
        hiddenMap.SetTile(position, hiddenTile);
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
    public void RevealInRadius(String viewerID, float sightRadius, Vector3Int center)
    {
        List<Vector3Int> revealTiles = DjikstrasSightCheck(center, sightRadius);
        foreach (Vector3Int tile in revealTiles)
        {
            if(!revealedPositions.ContainsKey(tile))
                revealedPositions.Add(tile, new HashSet<string>());
            revealedPositions[tile].Add(viewerID);
            RevealPosition(tile);
        }
    }

    public void UpdateVision(UnitBase unit)
    {
        List<Vector3Int> rmvList = new List<Vector3Int>();
        foreach (KeyValuePair<Vector3Int, HashSet<String>> kvp in revealedPositions)
        {
            if (kvp.Value.Contains(unit.myId))
            {
                rmvList.Add(kvp.Key);
            }
        }
        foreach (Vector3Int tile in rmvList)
        {
            revealedPositions[tile].Remove(unit.myId);
            if (revealedPositions[tile].Count == 0)
            {
                ConcealPosition(tile);
            }
        }
        RevealInRadius(unit.myId, unit.baseData.sightRadius, unit.currentPosition);
    }
    
    private List<Vector3Int> DjikstrasSightCheck(Vector3Int start, float sightRadius, bool ignoreSightBlocking = false)
    {
        List<Vector3Int> viewableList = new List<Vector3Int>();
        List<HexTileUtility.DjikstrasNode> allInRange =
            HexTileUtility.DjikstrasGetTilesInRange(mainMap, start, sightRadius, -1);
        List<Vector3Int> viewBlockerList = new List<Vector3Int>();
        foreach (HexTileUtility.DjikstrasNode val in allInRange)
        {
            if (mainMap.GetTile<DataTile>(val.location).data.lineOfSightBlocking)
            {
                viewBlockerList.Add(val.location);
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

    //checks if a target tile is in view of the viewer
    private bool IsInView(Vector3Int target, Vector3Int viewer, List<Vector3Int> viewBlockers)
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
