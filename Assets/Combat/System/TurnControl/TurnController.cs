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

    public UnityEvent<float, TimeWrapper> nextEventStarting = new UnityEvent<float, TimeWrapper>(); 

    private UnitBase _currentActor;

    private TimeQueue<TimeWrapper> turnQueue = new TimeQueue<TimeWrapper>();

    private int wrapperID = 1;

    public class TimeWrapper : IEquatable<TimeWrapper>
    {
        public UnitBase unit;

        public UnityAction action;

        public int actionID;

        public TimeWrapper(UnitBase initUnit)
        {
            unit = initUnit;
        }

        public TimeWrapper(UnityAction unityAction, int actionID)
        {
            action = unityAction;
            this.actionID = actionID;
        }

        public bool Equals(TimeWrapper other)
        {
            if (other == null) return false;
            if (actionID == 0)
            {
                if (unit == null) return false;
                return unit.Equals(other.unit);
            }
            return actionID.Equals(other.actionID);
        }
    }
    
    private void Awake()
    {
        controller = this;
    }

    public void RemoveFromQueue(UnitBase toRemove, bool repaint=true)
    {
        turnQueue.Remove(new TimeWrapper(toRemove));
        view.RemoveUnit(toRemove);
        if(repaint) TurnQueueRepaint();
    }

    public void RemoveFromQueue(int actionID)
    {
        turnQueue.Remove(new TimeWrapper(null, actionID));
    }

    public void ChangeInitiative(UnitBase unit, float add, bool repaint = true)
    {
        (TimeWrapper, float) val = turnQueue.AdvanceAndPop();
        float newTime = Mathf.Max(0, val.Item2 + add);
        turnQueue.Add(val.Item1, newTime);
    }
    
    public void AddToQueue(UnitBase add, bool repaint=true)
    {
        turnQueue.Add(new TimeWrapper(add), add.initiative);
        if(repaint) TurnQueueRepaint();
    }

    public int AddToQueue(UnityAction action, int time)
    {
        turnQueue.Add(new TimeWrapper(action, wrapperID), time);
        wrapperID++;
        return wrapperID - 1;
    }

    public void SetQueue(List<UnitBase> UnitsInBattle)
    {
        UnitsInBattle.Sort(UnitBase.CompareByInitiative);
        UnitsInBattle.Reverse();
        turnQueue = new TimeQueue<TimeWrapper>();
        foreach (UnitBase unit in UnitsInBattle)
        {
            AddToQueue(unit, false);
        }
        TurnQueueRepaint();
        NextEvent();
    }

    public void NextEvent()
    {
        if (_currentActor != null)
        {
            _currentActor.myEvents.onTurnEnded.Invoke(_currentActor);
        }
        (TimeWrapper, float) val = turnQueue.AdvanceAndPop();
        nextEventStarting.Invoke(val.Item2, val.Item1);
        if (val.Item1.unit != null)
        {
            UnitBase cur = val.Item1.unit;
            if(cur.myTeam != 0) endTurnButton.SetActive(false);
            else endTurnButton.SetActive(true);
            curUnitIndicator.sprite = cur.baseData.portrait;
            turnPassed.Invoke(cur, val.Item2);
            _currentActor = cur;
            cur.TurnStarted();
            AddToQueue(cur);
        }
        else
        {
            if(val.Item1.action != null)
                val.Item1.action.Invoke();
            _currentActor = null;
        }
    }

    public void TurnQueueRepaint()
    {
        List<TimeNode<TimeWrapper>> queue = turnQueue.queue;
        view.Repaint(queue);
    }

    
}
