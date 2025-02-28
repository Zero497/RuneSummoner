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
    public UnitBase currentActor => _currentActor;

    public UnityEvent<UnitBase, float> turnPassed = new UnityEvent<UnitBase, float>();

    public TurnView view;

    private UnitBase _currentActor;

    private TimeQueue<(UnitBase, GameObject)> turnQueue = new TimeQueue<(UnitBase, GameObject)>();
    private void Awake()
    {
        controller = this;
    }
    
    public void AddToQueue(UnitBase add, bool repaint=true)
    {
        turnQueue.Add((add, null), add.initiative);
        if(repaint) TurnQueueRepaint();
    }
    
    public void AddToQueue((UnitBase, GameObject) add, bool repaint=true)
    {
        turnQueue.Add(add, add.Item1.initiative);
        if(repaint) TurnQueueRepaint();
    }

    public void SetQueue(List<UnitBase> UnitsInBattle)
    {
        UnitsInBattle.Sort(UnitBase.CompareByInitiative);
        UnitsInBattle.Reverse();
        turnQueue = new TimeQueue<(UnitBase, GameObject)>();
        foreach (UnitBase unit in UnitsInBattle)
        {
            AddToQueue(unit, false);
        }
        TurnQueueRepaint();
        NextTurn();
    }

    public void NextTurn()
    {
        ((UnitBase, GameObject), float) val = turnQueue.AdvanceAndPop();
        (UnitBase, GameObject) cur = val.Item1;
        turnPassed.Invoke(cur.Item1, val.Item2);
        _currentActor = cur.Item1;
        cur.Item1.TurnStarted();
        AddToQueue(cur);
    }

    public void TurnQueueRepaint()
    {
        List<TimeNode<(UnitBase,GameObject)>> queue = turnQueue.queue;
        view.Repaint(queue);
    }

    
}
