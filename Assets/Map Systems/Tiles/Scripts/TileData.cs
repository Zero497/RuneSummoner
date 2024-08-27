using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Purpose: store the properties of a specific tile
[CreateAssetMenu(menuName = "2D/Tiles/TileData")]
public class TileData : ScriptableObject
{
    //how many tiles of movement this tile costs to enter
    public float moveCost;
    //whether this is an impassable terrain tile
    public bool isImpassable;
    
    //if this is a hidden tile, this will store a reference to the actual tile at this location
    private DataTile _hiddenTilePointer;
    public DataTile hiddenTilePointer
    {
        get => _hiddenTilePointer;
        set => _hiddenTilePointer = value;
    }
}
