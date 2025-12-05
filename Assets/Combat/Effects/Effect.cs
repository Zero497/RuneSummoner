using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Effect : IEquatable<Effect>
{
    protected UnitBase source;
    
    protected int stackDecayTime = 0;

    protected int stackDecayAmount;

    protected int stacks;

    protected int myQueueNumber = -1;

    private string _effectName;

    public string effectName => _effectName;

    public EventPriorityWrapper<Effect> onStackDecayed = new EventPriorityWrapper<Effect>();

    public EventPriorityWrapper<Effect, int> onStackAddedOrRemoved = new EventPriorityWrapper<Effect, int>();
    
    public EventPriorityWrapper<Effect> onEffectRemoved = new EventPriorityWrapper<Effect>();

    public bool isBuff;

    /*
        Expects:
            Unit 0: unit to apply to
            String 0: effect name
            Float 0: stacks to apply
     */
    public virtual void Initialize(SendData data)
    {
        source = data.unitData[0];
        stacks = (int) data.floatData[0];
        _effectName = data.strData[0];
        if (stackDecayTime > 0)
        {
            myQueueNumber = TurnController.controller.AddToQueue(Decay, stackDecayTime);
        }
    }

    public virtual void RemoveEffect()
    {
        source.RemoveEffect(this);
        if(myQueueNumber > 0)
            TurnController.controller.RemoveFromQueue(myQueueNumber);
        onEffectRemoved.Invoke(this);
    }

    public virtual void Decay()
    {
        AddStacks(-stackDecayAmount);
        myQueueNumber = TurnController.controller.AddToQueue(Decay, stackDecayTime);
        onStackDecayed.Invoke(this);
        if (this.stacks <= 0)
        {
            RemoveEffect();
        }
        TurnController.controller.NextEvent();
    }

    public int getStacks()
    {
        return stacks;
    }

    /*
        Expects:
            String 0: effect name
     */
    public virtual bool MatchForMerge(SendData data)
    {
        return Equals(data);
    }
    
    public virtual bool MatchForMerge(Effect effect)
    {
        return this.Equals(effect);
    }

    public virtual bool MergeEffects(Effect toMerge)
    {
        if (MatchForMerge(toMerge))
        {
            stacks += toMerge.getStacks();
            return true;
        }
        return false;
    }

    public virtual bool MergeEffects(SendData data)
    {
        if (MatchForMerge(data))
        {
            stacks += (int) data.floatData[0];
            if(stacks <= 0)
                RemoveEffect();
            return true;
        }
        return false;
    }

    public virtual void AddStacks(int addStacks)
    {
        stacks += addStacks;
        onStackAddedOrRemoved.Invoke(this, addStacks);
        if (stacks <= 0)
        {
            RemoveEffect();
        }
    }

    public virtual bool Equals(Effect other)
    {
        if (other == null) return false;
        return other.effectName.Equals(effectName);
    }

    /*
        Expects:
            String 0: effect name
     */
    public virtual bool Equals(SendData data)
    {
        if (data == null) return false;
        return data.strData[0].Equals(effectName);
    }

    public UnitBase GetSource()
    {
        return source;
    }
}
