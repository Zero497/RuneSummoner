using UnityEngine;

public class SkirmisherMove : Move
{
    public override bool RunAction(SendData data)
    {
        if (inProgress) return false;
        inProgress = true;
        return MoveController.mControl.Move(data.positionData[0], out moveRoutine, OnMoveStopped);
    }
    public override bool PrepAction()
    {
        if (inProgress) return false;
        MoveController.mControl.InitMovement(TurnController.controller.currentActor, TurnController.controller.currentActor.isFriendly);
        return true;
    }

    public override string GetID()
    {
        return "Skirmisher Move";
    }
}
