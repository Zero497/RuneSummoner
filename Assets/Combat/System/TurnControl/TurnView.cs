using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnView : MonoBehaviour
{
    public GameObject unitPortraitPrefab;

    private static float width;
    [SerializeField] private static float MaxTime;

    public static float maxTime => MaxTime;

    private List<PortraitMover> movers = new List<PortraitMover>();

    private void Awake()
    {
        width = getRect(transform).rect.width;
    }
    
    public static float getWidth()
    {
        return width;
    }

    public static float getTimePercent(float time)
    {
        return time / maxTime;
    }
    
    public static float percentToPosition(float percent)
    {
        return width * percent;
    }
    
    public static RectTransform getRect(Transform trans)
    {
        return trans as RectTransform;
    }
    
    public static float timeDiff(float t1, float t2)
    {
        if (t1 > t2)
        {
            return t1 - t2;
        }
        return t1 + (maxTime - t2);
    }

    public void Repaint(List<TimeNode<UnitBase>> queue)
    {
        MaxTime = queue[^1].time;
        UpdateBenchies(maxTime/4);
        //int curTime;
        foreach (TimeNode<UnitBase> node in queue)
        {
            UnitBase tup = node.value;
            PortraitMover portActual = null;
            foreach (PortraitMover port in movers)
            {
                if (port.myUnit.Equals(tup))
                {
                    port.MovePortrait(node.time, MaxTime);
                    portActual = port;
                    break;
                }
            }

            if (portActual != null) continue;
            GameObject portObj = Instantiate(unitPortraitPrefab, transform);
            portObj.GetComponent<Image>().sprite = tup.baseData.portrait;
            timeToPosition(getRect(portObj.transform), node.time);
            portActual = portObj.GetComponent<PortraitMover>();
            portActual.myUnit = tup;
            movers.Add(portActual);
            portActual.init(node.time, MaxTime);
        }
    }

    public static void timeToPosition(RectTransform targ, float time)
    {
        Vector2 val = new Vector2(percentToPosition(getTimePercent(time)), targ.anchoredPosition.y);
        targ.anchoredPosition = val;
    }
    
    private void UpdateBenchies(float div)
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = (div*(i+1)).ToString();
        }
    }
}
