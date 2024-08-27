using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Purpose: extend the tile class to store a reference to data about the tile.
[CreateAssetMenu(menuName = "2D/Tiles/DataTile")]
public class DataTile : Tile
{
    public TileData data;
}
