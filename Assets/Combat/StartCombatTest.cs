using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StartCombatTest : MonoBehaviour
{
    public GameObject main;

    public GameObject golem;

    public GameObject dummy;

    public GameObject wulf;

    public Tilemap mainMap;
    void Start()
    {
        MainCombatManager.manager.CreateUnit(main, new Vector3Int(0, 0), "main", false);
        MainCombatManager.manager.CreateUnit(golem, new Vector3Int(0, 1), "golem", false);
        MainCombatManager.manager.CreateUnit(wulf, new Vector3Int(0, 3), "wulf", false);
        MainCombatManager.manager.CreateUnit(dummy, new Vector3Int(5, 5), "dummy", false, team:1);
        MainCombatManager.manager.StartCombat();
    }
}
