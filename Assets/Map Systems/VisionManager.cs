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
    public void RevealInRadius(String viewerID, int sightRadius, Vector3Int center)
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
    
    private List<Vector3Int> DjikstrasSightCheck(Vector3Int start, int sightRadius, bool ignoreSightBlocking = false)
    {
        HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();
        List<Vector3Int> viewBlockerList = new List<Vector3Int>();
        List<Vector3Int> viewableList = new List<Vector3Int>();
        Queue<Vector3Int> openList = new Queue<Vector3Int>();
        openList.Enqueue(start);
        while (openList.Count != 0)
        {
            Vector3Int cur = openList.Dequeue();
            closedList.Add(cur);
            if(!mainMap.HasTile(cur)) continue;
            if (mainMap.GetTile<DataTile>(cur).data.lineOfSightBlocking)
                viewBlockerList.Add(cur);
            List<Vector3Int> adjacents = HexTileUtility.GetAdjacentTiles(cur, mainMap);
            foreach (Vector3Int adjacent in adjacents)
            {
                if (!closedList.Contains(adjacent) && !openList.Contains(adjacent))
                {
                    if (HexTileUtility.GetTileDistance(adjacent, start) >= sightRadius)
                        closedList.Add(adjacent);
                    else
                        openList.Enqueue(adjacent);
                }
            }
        }
        openList = new Queue<Vector3Int>();
        closedList = new HashSet<Vector3Int>();
        openList.Enqueue(start);
        while (openList.Count != 0)
        {
            Vector3Int cur = openList.Dequeue();
            closedList.Add(cur);
            if(!mainMap.HasTile(cur)) continue;
            viewableList.Add(cur);
            List<Vector3Int> adjacents = HexTileUtility.GetAdjacentTiles(cur, mainMap);
            foreach (Vector3Int adjacent in adjacents)
            {
                if (!closedList.Contains(adjacent) && !openList.Contains(adjacent))
                {
                    if (HexTileUtility.GetTileDistance(adjacent, start) >= sightRadius ||
                        (!IsInView(adjacent, start, viewBlockerList) && !ignoreSightBlocking))
                        closedList.Add(adjacent);
                    else
                        openList.Enqueue(adjacent);
                }
            }
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
