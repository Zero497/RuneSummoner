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
        actGolem.transform.position = mainMap.GetCellCenterWorld(new Vector3Int(2, 4));
        actMain.transform.position = mainMap.GetCellCenterWorld(new Vector3Int(2, 2));
        TurnController.controller.SetQueue(new List<UnitBase>(){actGolem.GetComponent<UnitBase>(), actMain.GetComponent<UnitBase>()});
    }
}
