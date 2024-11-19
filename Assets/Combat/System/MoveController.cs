using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class MoveController : MonoBehaviour
{
    public static MoveController mControl;

    public UnitBase curUnit;

    public List<HexTileUtility.DjikstrasNode> allInRange;
    
    public Tile movementOverlayTile;

    public Tilemap mainMap;

    public Tilemap overlayMap;

    private void Awake()
    {
        mControl = this;
    }

    //call this before accessing Mcontrol methods. Creates movement overlay tiles and initializes tiles in movement range
    public void InitMovement(UnitBase unit, bool useOverlay = true)
    {
        OverlayManager.instance.ClearOverlays();
        allInRange = HexTileUtility.DjikstrasGetTilesInRange(mainMap, unit.currentPosition, unit.moveRemaining, 0, true);
        if (useOverlay)
        {
            OverlayManager.instance.CreateOverlay(allInRange, movementOverlayTile);
        }
        curUnit = unit;
    }
    

    public bool Move(Vector3Int target, out Coroutine routine, UnityAction<bool> onMoveStopped = null)
    {   
        OverlayManager.instance.ClearOverlays();
        routine = null;
        
        if (curUnit != TurnController.controller.currentActor) return false;
        if (allInRange == null || !allInRange.Contains(new HexTileUtility.DjikstrasNode(curUnit.currentPosition))) return false;
        
        HexTileUtility.DjikstrasNode targetNode = allInRange.Find(node => node.location == target);
        
        routine = StartCoroutine(curUnit.MoveUnit(targetNode, mainMap, onMoveStopped));
        
        return true;
    }

    public bool IsValidMove(Vector3Int target)
    {
        return allInRange.Contains(new HexTileUtility.DjikstrasNode(target));
    }
}
