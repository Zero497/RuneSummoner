using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    public static TurnController controller;
    
    public Tilemap mainMap;

    public GameObject endTurnButton; 
        
    public UnitBase currentActor => _currentActor;

    public UnityEvent<UnitBase, float> turnPassed = new UnityEvent<UnitBase, float>();

    public TurnView view;

    public Image curUnitIndicator;

    private UnitBase _currentActor;

    private TimeQueue<UnitBase> turnQueue = new TimeQueue<UnitBase>();
    private void Awake()
    {
        controller = this;
    }

    public void RemoveFromQueue(UnitBase toRemove, bool repaint=true)
    {
        turnQueue.Remove(toRemove);
        view.RemoveUnit(toRemove);
        if(repaint) TurnQueueRepaint();
    }
    
    public void AddToQueue(UnitBase add, bool repaint=true)
    {
        turnQueue.Add(add, add.initiative);
        if(repaint) TurnQueueRepaint();
    }

    public void SetQueue(List<UnitBase> UnitsInBattle)
    {
        UnitsInBattle.Sort(UnitBase.CompareByInitiative);
        UnitsInBattle.Reverse();
        turnQueue = new TimeQueue<UnitBase>();
        foreach (UnitBase unit in UnitsInBattle)
        {
            AddToQueue(unit, false);
        }
        TurnQueueRepaint();
        NextTurn();
    }

    public void NextTurn()
    {
        (UnitBase, float) val = turnQueue.AdvanceAndPop();
        UnitBase cur = val.Item1;
        if(cur.myTeam != 0) endTurnButton.SetActive(false);
        else endTurnButton.SetActive(true);
        curUnitIndicator.sprite = cur.baseData.portrait;
        turnPassed.Invoke(cur, val.Item2);
        _currentActor = cur;
        cur.TurnStarted();
        AddToQueue(cur);
    }

    public void TurnQueueRepaint()
    {
        List<TimeNode<UnitBase>> queue = turnQueue.queue;
        view.Repaint(queue);
    }

    
}
