using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    public static TurnController controller;
    
    public Tilemap mainMap;

    public GameObject unitPortraitPrefab;
    public UnitBase currentActor => _currentActor;

    private UnitBase _currentActor;

    private Queue<UnitBase> turnQueue;

    public List<UnitBase> allFriendly;
    
    public List<UnitBase> allEnemy;

    private void Awake()
    {
        controller = this;
    }

    public void SetQueue(List<UnitBase> UnitsInBattle)
    {
        UnitsInBattle.Sort(UnitBase.CompareByInitiative);
        UnitsInBattle.Reverse();
        foreach (UnitBase unit in UnitsInBattle)
        {
            GameObject temp = Instantiate(unitPortraitPrefab, transform);
            temp.GetComponent<Image>().sprite = unit.baseData.portrait;
            if(unit.isFriendly) allFriendly.Add(unit);
            else allEnemy.Add(unit);
        }
        turnQueue = new Queue<UnitBase>(UnitsInBattle);
        NextTurn();
    }

    public void NextTurn()
    {
        UnitBase cur = turnQueue.Dequeue();
        Destroy(transform.GetChild(0).gameObject);
        GameObject temp = Instantiate(unitPortraitPrefab, transform);
        temp.GetComponent<Image>().sprite = cur.baseData.portrait;
        _currentActor = cur;
        cur.TurnStarted();
        turnQueue.Enqueue(cur);
    }

    public bool isTileOccupied(Vector3Int tile)
    {
        foreach (UnitBase unit in allFriendly)
        {
            if(unit.currentPosition == tile) return true;
        }
        foreach (UnitBase unit in allEnemy)
        {
            if(unit.currentPosition == tile) return true;
        }
        return false;
    }

    public UnitBase getUnitAtPosition(Vector3Int tile)
    {
        foreach (UnitBase unit in allFriendly)
        {
            if(unit.currentPosition == tile) return unit;
        }
        foreach (UnitBase unit in allEnemy)
        {
            if(unit.currentPosition == tile) return unit;
        }
        return null;
    }

    public List<UnitBase> getUnitsInRange(Vector3Int source, int range)
    {
        List<UnitBase> result = new List<UnitBase>();
        foreach (UnitBase unit in allFriendly)
        {
            if (HexTileUtility.GetTileDistance(source, unit.currentPosition) <= range)
            {
                result.Add(unit);
            }
        }

        foreach (UnitBase unit in allEnemy)
        {
            if (HexTileUtility.GetTileDistance(source, unit.currentPosition) <= range)
            {
                result.Add(unit);
            }
        }
        return result;
    }
    
    public List<UnitBase> getEnemiesInRange(Vector3Int source, int range, int friendly)
    {
        List<UnitBase> result = new List<UnitBase>();
        List<UnitBase> checkList;
        if (friendly == 0)
        {
            checkList = allEnemy;
        }
        else
        {
            checkList = allFriendly;
        }
        foreach (UnitBase unit in checkList)
        {
            if (HexTileUtility.GetTileDistance(source, unit.currentPosition) <= range && unit.myTeam != friendly)
            {
                result.Add(unit);
            }
        }
        return result;
    }
    
    public List<UnitBase> getFriendliesInRange(Vector3Int source, int range, int friendly)
    {
        List<UnitBase> result = new List<UnitBase>();
        List<UnitBase> checkList;
        if (friendly == 0)
        {
            checkList = allFriendly;
        }
        else
        {
            checkList = allEnemy;
        }
        foreach (UnitBase unit in checkList)
        {
            if (HexTileUtility.GetTileDistance(source, unit.currentPosition) <= range && unit.myTeam == friendly)
            {
                result.Add(unit);
            }
        }
        return result;
    }

    public void AddToQueue(UnitBase add)
    {
        turnQueue.Enqueue(add);
    }
}
