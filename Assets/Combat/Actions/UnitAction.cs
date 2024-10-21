using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnitAction
{
    public UnityEvent OnActionComplete = new UnityEvent();

    public bool inProgress = false;
    
    //attempt to run the action at the target position. return false on failure
    public abstract bool RunAction(Vector3Int target);

    public abstract bool PrepAction();

    public abstract bool RushCompletion();

}
