using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Purpose: store the properties of a specific tile
[CreateAssetMenu(menuName = "2D/Tiles/DataTiles/TileData")]
public class TileData : ScriptableObject
{
    //how many tiles of movement this tile costs to enter
    public float moveCost;
    //whether this is an impassable terrain tile
    public bool isImpassable;
    //whether this terrain blocks line of sight
    public bool lineOfSightBlocking;

    public float getCostByType(int index)
    {
        switch (index)
        {
            case 0:
                if (isImpassable) return float.PositiveInfinity;
                return moveCost;
            default:
                return 1;
        }
    }
}
