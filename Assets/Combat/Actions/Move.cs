using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : UnitAction
{
    Coroutine moveRoutine;
    public override bool RunAction(Vector3Int target)
    {
        if (inProgress) return false;
        inProgress = true;
        return MoveController.mControl.Move(target, out moveRoutine, OnMoveStopped);
    }

    public override bool PrepAction()
    {
        if (inProgress) return false;
        MoveController.mControl.InitMovement(TurnController.controller.currentActor, TurnController.controller.currentActor.isFriendly);
        return true;
    }

    public override bool RushCompletion()
    {
        if (!inProgress) return false;
        TurnController.controller.currentActor.forceMove = true;
        
        return true;
    }

    private void OnMoveStopped(bool reason)
    {
        inProgress = false;
    }
}
