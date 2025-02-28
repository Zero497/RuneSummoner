using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PortraitMover : MonoBehaviour
{
    private float targetTime;

    private float yieldTime = 0.05f;

    private float curTime;

    private float oldMaxTime;

    public void init(float initTime, float initMaxTime)
    {
        curTime = initTime;
        oldMaxTime = initMaxTime;
        StartCoroutine(Move());
    }
    
    public void MovePortrait(float targetTime, float newMaxTime)
    {
        this.targetTime = targetTime;
        curTime = curTime * newMaxTime / oldMaxTime;
        oldMaxTime = newMaxTime;
    }

    private IEnumerator Move()
    {   
        RectTransform rect = TurnView.getRect(transform);
        while (true)
        {
            float dist = TurnView.timeDiff(curTime, targetTime);
            float moveAmount = dist * yieldTime * 2;
            curTime -= moveAmount;
            if (curTime <= 1 && targetTime > 1)
            {
                curTime = TurnView.maxTime+curTime;
            }
            TurnView.timeToPosition(rect, curTime);
            yield return new WaitForSeconds(yieldTime);
        }
    }

    

    

    

    
}
