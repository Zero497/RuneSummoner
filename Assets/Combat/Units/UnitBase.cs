using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class UnitBase : MonoBehaviour
{
    public UnitData baseData;

    public float moveRemaining;

    public Vector3Int currentPosition;
    
    public string myId;

    public bool isFriendly;

    [NonSerialized]public bool forceMove;

    //number of seconds to reach destination (from 1 tile to another)
    private static float moveSpeed = 0.2f;

    public void TurnStarted()
    {
        moveRemaining = baseData.movementSpeed;
    }
    
    public static int CompareByInitiative(UnitBase item1, UnitBase item2)
    {
        return UnitData.CompareByInitiative(item1.baseData, item2.baseData);
    }

    public IEnumerator MoveUnit(HexTileUtility.DjikstrasNode target, Tilemap mainMap, UnityAction<bool> returnToCaller = null)
    {
        forceMove = false;
        List<HexTileUtility.DjikstrasNode> allInRange = HexTileUtility.DjikstrasGetTilesInRange(mainMap, currentPosition, baseData.sightRadius, -1);
        while (true)
        {
            HexTileUtility.DjikstrasNode next = getNext(target);
            if (next == null)
            {
                if (returnToCaller != null) returnToCaller(true);
                break;
            }
            Vector3 nextPosition = mainMap.GetCellCenterWorld(next.location);
            Vector3 moveRate = (nextPosition - transform.position)/moveSpeed;
            Vector3 lastPos = transform.position;
            while ((transform.position - nextPosition).magnitude > 0.01f)
            {
                transform.Translate(moveRate * Time.deltaTime, Space.World);
                if(forceMove) transform.Translate(moveRate * (Time.deltaTime * 3), Space.World);
                if ((lastPos - nextPosition).magnitude < (transform.position - nextPosition).magnitude)
                {
                    transform.position = nextPosition;
                    break;
                }
                lastPos = transform.position;
                yield return new WaitForSeconds(0);
            }
            moveRemaining -= mainMap.GetTile<DataTile>(next.location).data.moveCost;
            currentPosition = next.location;
            VisionManager.visionManager.UpdateVision(this);
            List<HexTileUtility.DjikstrasNode> newInRange = HexTileUtility.DjikstrasGetTilesInRange(mainMap, currentPosition, baseData.sightRadius, -1);
            List<UnitBase> compList;
            if (isFriendly) compList = TurnController.controller.allEnemy;
            else compList = TurnController.controller.allFriendly;
            if (UnitInDiff(newInRange.diff(allInRange), compList))
            {
                if (returnToCaller != null) returnToCaller(false);
                break;
            }
            allInRange = newInRange;
        }
    }

    private bool UnitInDiff(List<HexTileUtility.DjikstrasNode> tiles, List<UnitBase> units)
    {
        foreach (UnitBase unit in units)
        {
            if (tiles.Contains(new HexTileUtility.DjikstrasNode(unit.currentPosition)))
            {
                return true;
            }
        }
        return false;
    }

    private HexTileUtility.DjikstrasNode getNext(HexTileUtility.DjikstrasNode target)
    {
        HexTileUtility.DjikstrasNode ret = target;
        if (ret == null || ret.parent == null) return null;
        while (ret.parent.location != currentPosition)
        {
            ret = ret.parent;
            if (ret.parent == null) return null;
        }
        return ret;
    }
}
