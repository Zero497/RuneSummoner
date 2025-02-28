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

    public void Repaint(List<TimeNode<(UnitBase, GameObject)>> queue)
    {
        MaxTime = queue[^1].time;
        UpdateBenchies(maxTime/4);
        //int curTime;
        foreach (TimeNode<(UnitBase, GameObject)> node in queue)
        {
            (UnitBase, GameObject) tup = node.value;
            if (tup.Item2 == null)
            {
                tup.Item2 = Instantiate(unitPortraitPrefab, transform);
                tup.Item2.GetComponent<Image>().sprite = tup.Item1.baseData.portrait;
                timeToPosition(getRect(tup.Item2.transform), node.time);
                tup.Item2.GetComponent<PortraitMover>().init(node.time, MaxTime);
                node.value.Item2 = tup.Item2;
            }
            else
            {
                tup.Item2.GetComponent<PortraitMover>().MovePortrait(node.time, MaxTime);
            }
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
