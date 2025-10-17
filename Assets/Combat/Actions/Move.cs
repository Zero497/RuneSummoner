using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : UnitAction
{
    protected Coroutine moveRoutine;
    public override bool RunAction(SendData data)
    {
        if (inProgress) return false;
        if (source.usedAbilityThisTurn) return false;
        inProgress = true;
        return MoveController.mControl.Move(data.positionData[0], out moveRoutine, OnMoveStopped);
    }

    public override bool PrepAction()
    {
        if (inProgress) return false;
        if (source.usedAbilityThisTurn) return false;
        MoveController.mControl.InitMovement(TurnController.controller.currentActor, TurnController.controller.currentActor.isFriendly);
        return true;
    }

    public override bool RushCompletion()
    {
        if (!inProgress) return false;
        TurnController.controller.currentActor.forceMove = true;
        
        return true;
    }

    public override string GetDescription()
    {
        return "";
    }

    protected void OnMoveStopped(bool reason)
    {
        inProgress = false;
    }
}
