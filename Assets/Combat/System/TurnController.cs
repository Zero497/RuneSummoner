using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    public static TurnController controller;

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

    public void AddToQueue(UnitBase add)
    {
        turnQueue.Enqueue(add);
    }
}
