using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class UnitBase : MonoBehaviour
{
    public UnitData baseData;
    
    public UnitData.Element myElement;

    public UnitData.CombatType myType;
    
    public float summonCost;

    public int level;
    
    public float health;
    
    public float initiative;
    
    public float abilityPower;

    public float magicalAttack;
    
    public float physicalAttack;
    
    public float magicalDefence;
    
    public float physicalDefence;

    public float mana;

    public float manaRegen;
    
    public float sightRadius;

    public float speed;

    public float stamina;
    
    public float staminaRegen;
    
    public List<ActiveAbility> activeAbilities = new List<ActiveAbility>();

    public float moveRemaining;

    public Vector3Int currentPosition;
    
    public string myId;

    public bool isFriendly;

    [NonSerialized]public bool forceMove;
    
    [NonSerialized]public float currentHealth;
    
    [NonSerialized]public EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttacked = new EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
    
    [NonSerialized]public EventPriorityWrapper<UnitBase, float> onTakeDamage = new EventPriorityWrapper<UnitBase, float>();
    
    

    //number of seconds to reach destination (from 1 tile to another)
    private static float moveSpeed = 0.2f;

    public void TurnStarted()
    {
        moveRemaining = speed;
    }

    public void Init(int inputlevel, UnitData inBaseData /*, TODO: EquipmentData */)
    {
        baseData = inBaseData;
        level = inputlevel;
        inputlevel -= 1;
        myElement = inBaseData.myElement;
        myType = inBaseData.myType;
        summonCost = inBaseData.summonCost;
        health = inBaseData.health + inBaseData.healthPerLevel * inputlevel;
        abilityPower = inBaseData.abilityPower + inBaseData.abilityPowerPerLevel * inputlevel;
        magicalAttack = inBaseData.magicalAttack + inBaseData.magicalAttackPerLevel * inputlevel;
        physicalAttack = inBaseData.physicalAttack + inBaseData.physicalAttackPerLevel * inputlevel;
        magicalDefence = inBaseData.magicalDefence + inBaseData.magicalDefencePerLevel * inputlevel;
        physicalDefence = inBaseData.physicalDefence + inBaseData.physicalDefencePerLevel * inputlevel;
        mana = inBaseData.mana + inBaseData.manaPerLevel * inputlevel;
        manaRegen = inBaseData.manaRegen + inBaseData.manaRegenPerLevel * inputlevel;
        stamina = inBaseData.stamina + inBaseData.staminaPerLevel * inputlevel;
        staminaRegen = inBaseData.staminaRegen + inBaseData.staminaRegenPerLevel * inputlevel;
        initiative = inBaseData.initiative;
        speed = inBaseData.speed;
        sightRadius = inBaseData.sightRadius;
        activeAbilities.Add(inBaseData.defaultAttack);

    }

    public void ReceiveAttack(Attack.AttackMessageToTarget attack)
    {
        attack.damage *= typeMatchups[myElement][attack.element];
        onAttacked.Invoke(this, attack);
        if (attack.damageType == AttackData.DamageType.Physical)
        {
            attack.damage -= physicalDefence;
        }
        else if (attack.damageType == AttackData.DamageType.Magic)
        {
            attack.damage -= magicalDefence;
        }
        currentHealth -= attack.damage;
        onTakeDamage.Invoke(this, attack.damage);
    }
    
    public static int CompareByInitiative(UnitBase item1, UnitBase item2)
    {
        if (item1 == null && item2 == null) return 0;
        if (item1 == null) return -1;
        if (item2 == null) return 1;
        if (item1.initiative < item2.initiative) return -1;
        if (item1.initiative > item2.initiative) return 1;
        return 0;
    }

    public IEnumerator MoveUnit(HexTileUtility.DjikstrasNode target, Tilemap mainMap, UnityAction<bool> returnToCaller = null)
    {
        forceMove = false;
        List<HexTileUtility.DjikstrasNode> allInRange = HexTileUtility.DjikstrasGetTilesInRange(mainMap, currentPosition, sightRadius, -1);
        while (true)
        {
            HexTileUtility.DjikstrasNode next = getNext(target);
            if (next == null)
            {
                if (returnToCaller != null) returnToCaller(true);
                break;
            }
            Vector3 nextPosition = mainMap.GetCellCenterWorld(next.location);
            Vector3 moveRate = (nextPosition - transform.position)/moveSpeed;
            Vector3 lastPos = transform.position;
            while ((transform.position - nextPosition).magnitude > 0.01f)
            {
                transform.Translate(moveRate * Time.deltaTime, Space.World);
                if(forceMove) transform.Translate(moveRate * (Time.deltaTime * 3), Space.World);
                if ((lastPos - nextPosition).magnitude < (transform.position - nextPosition).magnitude)
                {
                    transform.position = nextPosition;
                    break;
                }
                lastPos = transform.position;
                yield return new WaitForSeconds(0);
            }
            moveRemaining -= mainMap.GetTile<DataTile>(next.location).data.moveCost;
            currentPosition = next.location;
            VisionManager.visionManager.UpdateVision(this);
            List<HexTileUtility.DjikstrasNode> newInRange = HexTileUtility.DjikstrasGetTilesInRange(mainMap, currentPosition, sightRadius, -1);
            List<UnitBase> compList;
            if (isFriendly) compList = TurnController.controller.allEnemy;
            else compList = TurnController.controller.allFriendly;
            if (UnitInDiff(newInRange.diff(allInRange), compList))
            {
                if (returnToCaller != null) returnToCaller(false);
                break;
            }
            allInRange = newInRange;
        }
    }

    private bool UnitInDiff(List<HexTileUtility.DjikstrasNode> tiles, List<UnitBase> units)
    {
        foreach (UnitBase unit in units)
        {
            if (tiles.Contains(new HexTileUtility.DjikstrasNode(unit.currentPosition)))
            {
                return true;
            }
        }
        return false;
    }

    private HexTileUtility.DjikstrasNode getNext(HexTileUtility.DjikstrasNode target)
    {
        HexTileUtility.DjikstrasNode ret = target;
        if (ret == null || ret.parent == null) return null;
        while (ret.parent.location != currentPosition)
        {
            ret = ret.parent;
            if (ret.parent == null) return null;
        }
        return ret;
    }
    
    private static Dictionary<UnitData.Element, Dictionary<UnitData.Element, float>> typeMatchups = new Dictionary<UnitData.Element, Dictionary<UnitData.Element, float>>
    {
        {UnitData.Element.beast, new Dictionary<UnitData.Element, float>()
        {
            { UnitData.Element.beast, 1.25f },
            { UnitData.Element.human, 1.25f },
            { UnitData.Element.machine, 1f }
        }},
        {UnitData.Element.human, new Dictionary<UnitData.Element, float>()
        {
            { UnitData.Element.beast, 0.75f },
            { UnitData.Element.human, 1.25f },
            { UnitData.Element.machine, 1f }
        }},
        {UnitData.Element.machine, new Dictionary<UnitData.Element, float>()
        {
            { UnitData.Element.beast, 0.75f },
            { UnitData.Element.human, 1f },
            { UnitData.Element.machine, 0.75f }
        }}
    };
}
