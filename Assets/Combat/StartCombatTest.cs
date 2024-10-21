using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StartCombatTest : MonoBehaviour
{
    public GameObject main;

    public GameObject golem;

    public Tilemap mainMap;
    void Start()
    {
        GameObject actGolem = Instantiate(golem);
        GameObject actMain = Instantiate(main);
        UnitBase golemBase = actGolem.GetComponent<UnitBase>();
        UnitBase mainBase = actMain.GetComponent<UnitBase>();
        golemBase.isFriendly = true;
        mainBase.isFriendly = true;
        actGolem.transform.position = mainMap.GetCellCenterWorld(new Vector3Int(30, 30));
        golemBase.currentPosition = new Vector3Int(30, 30);
        actMain.transform.position = mainMap.GetCellCenterWorld(new Vector3Int(2, 2));
        mainBase.currentPosition = new Vector3Int(2, 2);
        golemBase.myId = "Golem";
        mainBase.myId = "MC";
        VisionManager.visionManager.UpdateVision(golemBase);
        VisionManager.visionManager.UpdateVision(mainBase);
        TurnController.controller.SetQueue(new List<UnitBase>(){actGolem.GetComponent<UnitBase>(), actMain.GetComponent<UnitBase>()});
    }
}
