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

    public GameObject evilLizard;

    public Tilemap mainMap;
    void Start()
    {
        MainCombatManager.manager.CreateUnit(evilLizard, new Vector3Int(Random.Range(10,20), Random.Range(10,20)), "liz1", false, team:1, isFriendly: false);
        MainCombatManager.manager.CreateUnit(evilLizard, new Vector3Int(Random.Range(10,20), Random.Range(10,20)), "liz2", false, team:1, isFriendly: false);
        //MainCombatManager.manager.CreateUnit(evilLizard, new Vector3Int(Random.Range(10,20), Random.Range(10,20)), "liz3", false, team:1, isFriendly: false);
        MainCombatManager.manager.CreateUnit(main, new Vector3Int(0, 0), "main", false);
        MainCombatManager.manager.CreateUnit(golem, new Vector3Int(0, 1), "golem", false);
        MainCombatManager.manager.CreateUnit(wulf, new Vector3Int(0, 3), "wulf", false);
        //MainCombatManager.manager.CreateUnit(dummy, new Vector3Int(5, 5), "dummy", false, team:2);
        
        MainCombatManager.manager.StartCombat();
    }
}
