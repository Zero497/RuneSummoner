using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class HexTileUtility
{
    public static Vector3Int GetNearestTile(Vector3 worldPosition, Tilemap map)
    {
        var cellBounds = map.cellBounds;
        Vector3 closestTilePosition = worldPosition - map.GetCellCenterWorld(new Vector3Int(cellBounds.min.x,cellBounds.min.y,0));
        Vector3Int closestTile = new Vector3Int(cellBounds.min.x,cellBounds.min.y,0);
        for (int x = cellBounds.min.x; x < map.size.x; x++)
        {
            for (int y = cellBounds.min.y; y < map.size.y; y++)
            {
                Vector3 tempTilePosition = worldPosition - map.GetCellCenterWorld(new Vector3Int(x,y,0));
                if (tempTilePosition.magnitude < closestTilePosition.magnitude)
                {
                    closestTilePosition = tempTilePosition;
                    closestTile = new Vector3Int(x, y, 0);
                }
            }
        }
        return closestTile;
    }
    
    public static List<Vector3Int> GetAdjacentTiles(Vector3Int tile, Tilemap map)
    {
        int x = tile.x;
        int y = tile.y;
        int z = tile.z;
        List<Vector3Int> retval = new List<Vector3Int>();
        retval.Add(new Vector3Int(x, y+1, z));
        retval.Add(new Vector3Int(x-1, y+1, z));
        retval.Add(new Vector3Int(x-1, y, z));
        retval.Add(new Vector3Int(x-1, y-1, z));
        retval.Add(new Vector3Int(x, y-1, z));
        retval.Add(new Vector3Int(x+1, y, z));
        int i = 0;
        while (i < retval.Count)
        {
            if (!map.HasTile(retval[i]))
                retval.RemoveAt(i);
            else
                i++;
        }
        return retval;
    }

    //get distance between two tiles
    public static int GetTileDistance(Vector3Int start, Vector3Int end)
    {
        return GetTileDistance(start.x, start.y, end.x, end.y);
    }
    
    //get distance between two tiles
    //Thanks to Bunny83 on Unity forums
    public static int GetTileDistance(int aX1, int aY1, int aX2, int aY2)
    {
        int dx = aX2 - aX1;     // signed deltas
        int dy = aY2 - aY1;
        int x = Mathf.Abs(dx);  // absolute deltas
        int y = Mathf.Abs(dy);
        // special case if we start on an odd row or if we move into negative x direction
        if ((dx < 0)^((aY1&1)==1))
            x = Mathf.Max(0, x - (y + 1) / 2);
        else
            x = Mathf.Max(0, x - (y) / 2);
        return x + y;
    }
    
    /*
    gets the quadrant relative to the starting tile that the target falls in
    0 is from the upper left diagonal to the upper right, 1 from upper right to middle right,
    2 from middle right to lower right, 3 from lower right to lower left, 
    4 from lower left to middle left, and 5 from middle left to upper left
    this is inclusive on the left side only, ex. a tile exactly on the upper left
    diagonal from start will return 0, one on the upper right will return 1
    */
    public static int GetQuadrant(Vector3Int start, Vector3Int target)
    {
        List<Vector3Int> lineVals = FindValsInLine(start, GetTileDistance(start, target));
        for (int i = 0; i < lineVals.Count; i++)
        {
            if (lineVals[i] == target)
            {
                switch (i)
                {
                    case 0: return 0;
                    case 1: return 1;
                    case 2: return 4;
                    case 3: return 3;
                    case 4: return 5;
                    case 5: return 2;
                }
            }
        }
        if (isInLine(lineVals[0], target))
        {
            if (isInLine(target, lineVals[1])) return 0;
            if (isInLine(target, lineVals[4])) return 5;
        }
        if (isInLine(lineVals[3], target))
        {
            if (isInLine(target, lineVals[5])) return 2;
            if (isInLine(target, lineVals[2])) return 3;
        }
        if (isInLine(target, lineVals[1]) && isInLine(target, lineVals[5])) return 1;
        if (isInLine(target, lineVals[2]) && isInLine(target, lineVals[4])) return 4;
        return -1;
    }

    //checks if the two tiles are directly in line with each other diagonally
    public static bool isInLine(Vector3Int start, Vector3Int end)
    {
        if (start.y == end.y) return true;
        int dist = GetTileDistance(start, end);
        int amt = 0;
        if ((start.y % 2 == 0 && end.x < start.x) || (start.y % 2 != 0 && end.x > start.x))
            amt = Mathf.CeilToInt(dist / 2.0f);
        else
            amt = Mathf.FloorToInt(dist / 2.0f);
        if ((end.x == start.x - amt || end.x == start.x + amt) && (end.y == start.y+dist || end.y == start.y-dist))
        {
            return true;
        }
        return false;
    }

    //finds tiles in a direct lines with the start tile at the specified distance away
    //returned in order, upper left, upper right, lower left, lower right, middle left, middle right
    public static List<Vector3Int> FindValsInLine(Vector3Int start, int dist)
    {
        int x = start.x;
        int y = start.y;
        int z = start.z;
        List<Vector3Int> retval = new List<Vector3Int>();
        int amtCeil = Mathf.CeilToInt(dist / 2.0f);
        int amtFlr = Mathf.FloorToInt(dist / 2.0f);
        if (y % 2 == 0)
        {
            retval.Add(new Vector3Int(x-amtCeil, y+dist, z));
            retval.Add(new Vector3Int(x+amtFlr, y+dist, z));
            retval.Add(new Vector3Int(x-amtCeil, y-dist, z));
            retval.Add(new Vector3Int(x+amtFlr, y-dist, z));
        }
        else
        {
            retval.Add(new Vector3Int(x-amtFlr, y+dist, z));
            retval.Add(new Vector3Int(x+amtCeil, y+dist, z));
            retval.Add(new Vector3Int(x-amtFlr, y-dist, z));
            retval.Add(new Vector3Int(x+amtCeil, y-dist, z));
        }
        retval.Add(new Vector3Int(x-dist, y, z));
        retval.Add(new Vector3Int(x+dist, y, z));
        return retval;
    }
}
