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
        MoveController.mControl.InitMovement(cur);
        turnQueue.Enqueue(cur);
    }

    public void AddToQueue(UnitBase add)
    {
        turnQueue.Enqueue(add);
    }
}
