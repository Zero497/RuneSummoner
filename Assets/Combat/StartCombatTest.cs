using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StartCombatTest : MonoBehaviour
{
    public Tilemap mainMap;
    void Start()
    {
        UnitSimple direFerret = new UnitSimple("DireFerret", "DF1", 1, new StatGrades());
        MainCombatManager.manager.CreateUnit(direFerret, new Vector3Int(Random.Range(10,20), Random.Range(10,20)), "liz1", false, team:1, isFriendly: false);
        MainCombatManager.manager.CreateUnit(direFerret, new Vector3Int(Random.Range(10,20), Random.Range(10,20)), "liz2", false, team:1, isFriendly: false);
        //MainCombatManager.manager.CreateUnit(evilLizard, new Vector3Int(Random.Range(10,20), Random.Range(10,20)), "liz3", false, team:1, isFriendly: false);
        UnitSimple main = new UnitSimple("Player", "player", 1, new StatGrades());
        MainCombatManager.manager.CreateUnit(main, new Vector3Int(0, 0), "main", false);
        UnitSimple golem = new UnitSimple("Golem", "g1", 1, new StatGrades());
        MainCombatManager.manager.CreateUnit(golem, new Vector3Int(0, 1), "golem", false);
        MainCombatManager.manager.CreateUnit(direFerret, new Vector3Int(0, 3), "fer", false);
        //MainCombatManager.manager.CreateUnit(dummy, new Vector3Int(5, 5), "dummy", false, team:2);
        
        MainCombatManager.manager.StartCombat();
    }
}
