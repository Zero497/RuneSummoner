using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveController : MonoBehaviour
{
    public static MoveController mControl;

    public UnitBase curUnit;
    
    public Tile movementOverlayTile;

    public Tilemap mainMap;

    public Tilemap overlayMap;

    private void Awake()
    {
        mControl = this;
    }

    public void InitMovement(UnitBase unit)
    {
        List<HexTileUtility.DjikstrasNode> allInRange = HexTileUtility.DjikstrasGetTilesInRange(mainMap, unit.currentPosition, unit.moveRemaining, 0);
        foreach (HexTileUtility.DjikstrasNode pos in allInRange)
        {
            overlayMap.SetTile(pos.location, movementOverlayTile);
        }
    }
}
