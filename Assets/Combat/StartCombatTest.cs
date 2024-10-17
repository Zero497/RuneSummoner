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
        actGolem.transform.position = mainMap.GetCellCenterWorld(new Vector3Int(2, 4));
        golemBase.currentPosition = new Vector3Int(2, 4);
        actMain.transform.position = mainMap.GetCellCenterWorld(new Vector3Int(2, 2));
        mainBase.currentPosition = new Vector3Int(2, 2);
        golemBase.myId = "Golem";
        mainBase.myId = "MC";
        VisionManager.visionManager.RevealInRadius(golemBase.myId, golemBase.baseData.sightRadius, golemBase.currentPosition);
        VisionManager.visionManager.RevealInRadius(mainBase.myId, golemBase.baseData.sightRadius, golemBase.currentPosition);
        TurnController.controller.SetQueue(new List<UnitBase>(){actGolem.GetComponent<UnitBase>(), actMain.GetComponent<UnitBase>()});
    }
}
