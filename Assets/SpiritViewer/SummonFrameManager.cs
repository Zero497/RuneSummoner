using System;
using UnityEngine;

public class SummonFrameManager : MonoBehaviour
{
    public Transform summonFrameContent;

    public GameObject summonButtonPrefab;

    public void Awake()
    {
        foreach ((string,int) summon in InventoryManager.GetAllShards())
        {
            GameObject sb = Instantiate(summonButtonPrefab, summonFrameContent);
            SummonButton sbs = sb.GetComponent<SummonButton>();
            sbs.myUnitName = summon.Item1;
            sbs.Init();
        }
    }
}
