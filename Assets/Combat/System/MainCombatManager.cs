using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MainCombatManager : MonoBehaviour
{
    public static MainCombatManager manager;

    public Tilemap mainMap;

    public Transform abilityButtonParent;

    public GameObject abilityButtonPrefab;

    public GameObject winCanv;

    public GameObject loseCanv;

    public GameObject blankUnitPrefab;
    
    private List<AbilityButton> AbilityButtons = new List<AbilityButton>();
    
    [NonSerialized]public List<UnitBase> allFriendly = new List<UnitBase>();
    
    [NonSerialized]public List<UnitBase> allEnemy = new List<UnitBase>();

    public EventPriorityWrapper<UnitBase> onUnitAddedToCombat = new EventPriorityWrapper<UnitBase>();

    private void Awake()
    {
        if (manager != null)
        {
            Destroy(gameObject);
            return;
        }
        manager = this;
        hideAbilities();
    }

    public void SendAbilities(List<ActiveAbility> abilities)
    {
        while (AbilityButtons.Count < abilities.Count)
        {
            AddAbilityButton();
        }
        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i] != null)
            {
                AbilityButtons[i].gameObject.SetActive(true);
                AbilityButtons[i].SetAbility(abilities[i]);
            }
        }
    }

    private void AddAbilityButton()
    {
        GameObject button = Instantiate(abilityButtonPrefab, abilityButtonParent);
        AbilityButtons.Add(button.GetComponent<AbilityButton>());
    }

    public void StartCombat()
    {
        TurnController.controller.TurnQueueRepaint();
        TurnController.controller.NextEvent();
    }

    public void EndTurn()
    {
        hideAbilities();
        TurnController.controller.NextEvent();
    }

    private void hideAbilities()
    {
        foreach (AbilityButton abilityButton in AbilityButtons)
        {
            abilityButton.gameObject.SetActive(false);
        }
    }
    
    public Vector3Int CreateUnit(UnitSimple simple, Vector3Int pos, string id, bool repaint = true, bool isFriendly = true, int team = 0)
    {
        UnitData data = UnitData.GetUnitData(simple.name);
        GameObject newUnit = Instantiate(blankUnitPrefab);
        UnitBase newBase = newUnit.GetComponent<UnitBase>();
        newUnit.GetComponent<SpriteRenderer>().sprite = data.UnitSprite;
        newUnit.transform.position = mainMap.GetCellCenterWorld(pos);
        int sanityCheck = 0;
        while (!isValidPlacement(pos))
        {
            List<Vector3Int> adj = HexTileUtility.GetAdjacentTiles(pos, mainMap);
            foreach (Vector3Int val in adj)
            {
                if (isValidPlacement(val))
                {
                    pos = val;
                    break;
                }
            }
            if (isValidPlacement(pos)) break;
            pos = adj[Random.Range(0, adj.Count)];
            sanityCheck++;
            if (sanityCheck > 20)
            {
                Debug.Log("Unit placement failed!");
                break;
            }
        }
        newBase.currentPosition = pos;
        newBase.myId = id;
        newBase.isFriendly = isFriendly;
        newBase.myTeam = team;
        newBase.Init(simple, data);
        if(team != 0) newBase.myAI = new FSM(newBase,newBase.baseData.defaultEntryState);
        if (isFriendly)
        {
            VisionManager.visionManager.UpdateFriendlyVision(newBase);
            allFriendly.Add(newBase);
        }
        else
        {
            allEnemy.Add(newBase);
            newBase.ConcealMe(newBase);
            VisionManager.visionManager.UpdateEnemyVision(newBase);
        }
        TurnController.controller.AddToQueue(newBase, repaint);
        onUnitAddedToCombat.Invoke(newBase);
        return pos;
    }

    public void registerUnitDead(UnitBase unit)
    {
        if (unit.isFriendly)
        {
            allFriendly.Remove(unit);
            if (allFriendly.Count == 0)
            {
                loseCanv.SetActive(true);
            }
        }
        else
        {
            allEnemy.Remove(unit);
            if (allEnemy.Count == 0)
            {
                winCanv.SetActive(true);
            }
        }
        VisionManager.visionManager.ConcealInRadius(unit.myId, Mathf.FloorToInt(unit.sightRadius), unit.currentPosition);
        TurnController.controller.RemoveFromQueue(unit);
    }
    
    public List<UnitBase> getUnitsInRange(Vector3Int source, int range)
    {
        List<UnitBase> result = new List<UnitBase>();
        foreach (UnitBase unit in allFriendly)
        {
            if (HexTileUtility.GetTileDistance(source, unit.currentPosition) <= range)
            {
                result.Add(unit);
            }
        }

        foreach (UnitBase unit in allEnemy)
        {
            if (HexTileUtility.GetTileDistance(source, unit.currentPosition) <= range)
            {
                result.Add(unit);
            }
        }
        return result;
    }
    
    public List<UnitBase> getEnemiesInRange(Vector3Int source, int range, int friendly)
    {
        List<UnitBase> result = new List<UnitBase>();
        List<UnitBase> checkList;
        if (friendly == 0)
        {
            checkList = allEnemy;
        }
        else
        {
            checkList = allFriendly;
        }
        foreach (UnitBase unit in checkList)
        {
            if (HexTileUtility.GetTileDistance(source, unit.currentPosition) <= range && unit.myTeam != friendly)
            {
                result.Add(unit);
            }
        }
        return result;
    }
    
    public List<UnitBase> getFriendliesInRange(Vector3Int source, int range, int friendly)
    {
        List<UnitBase> result = new List<UnitBase>();
        List<UnitBase> checkList;
        if (friendly == 0)
        {
            checkList = allFriendly;
        }
        else
        {
            checkList = allEnemy;
        }
        foreach (UnitBase unit in checkList)
        {
            if (HexTileUtility.GetTileDistance(source, unit.currentPosition) <= range && unit.myTeam == friendly)
            {
                result.Add(unit);
            }
        }
        return result;
    }
    
    public UnitBase getUnitAtPosition(Vector3Int tile)
    {
        foreach (UnitBase unit in allFriendly)
        {
            if(unit.currentPosition == tile) return unit;
        }
        foreach (UnitBase unit in allEnemy)
        {
            if(unit.currentPosition == tile) return unit;
        }
        return null;
    }
    
    public bool isTileOccupied(Vector3Int tile)
    {
        foreach (UnitBase unit in allFriendly)
        {
            if(unit.currentPosition == tile) return true;
        }
        foreach (UnitBase unit in allEnemy)
        {
            if(unit.currentPosition == tile) return true;
        }
        return false;
    }

    public bool isTilePassable(Vector3Int tile)
    {
        return !mainMap.GetTile<DataTile>(tile).data.isImpassable;
    }

    public bool isValidPlacement(Vector3Int tile)
    {
        return !isTileOccupied(tile) && isTilePassable(tile);
    }
}
