using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UnitBase : MonoBehaviour, IEquatable<UnitBase>
{
    public UnitData baseData;

    public SpriteRenderer mySprite;

    public Image healthBar;

    public Image manaBar;
    
    public Image stamBar;

    [NonSerialized]public bool usedAbilityThisTurn;
    
    [NonSerialized]public UnitData.Element myElement;
    
    [NonSerialized]public float summonCost;

    [NonSerialized]public int level;
    
    [NonSerialized]public float health;
    
    [NonSerialized]public int initiative;
    
    [NonSerialized]public float abilityPower;

    [NonSerialized]public float magicalAttack;
    
    [NonSerialized]public float physicalAttack;
    
    [NonSerialized]public float magicalDefence;
    
    [NonSerialized]public float physicalDefence;

    [NonSerialized]public float mana;

    [NonSerialized]public float manaRegen;
    
    [NonSerialized]public int sightRadius;

    [NonSerialized]public float speed;

    [NonSerialized]public float stamina;
    
    [NonSerialized]public float staminaRegen;
    
    [NonSerialized]public List<ActiveAbility> activeAbilities = new List<ActiveAbility>();

    [NonSerialized] public List<PassiveAbility> PassiveAbilities = new List<PassiveAbility>();

    [NonSerialized]public float moveRemaining;

    [NonSerialized]public Vector3Int currentPosition;
    
    [NonSerialized]public string myId;

    [NonSerialized]public bool isFriendly;
    
    [NonSerialized]public int myTeam = 0;

    [NonSerialized]public FSM myAI = null;

    [NonSerialized]public bool forceMove;
    
    [NonSerialized]public float currentHealth;
    
    [NonSerialized]public float currentMana;
    
    [NonSerialized]public float currentStamina;

    public UnitEvents myEvents = new UnitEvents();
    
    private bool initialized = false;

    //number of seconds to reach destination (from 1 tile to another)
    private static float moveSpeed = 0.2f;

    private void Start()
    {
        Init(1, baseData);
    }

    private void updateBars()
    {
        healthBar.fillAmount = (currentHealth / health);
        manaBar.fillAmount = (currentMana / mana);
        stamBar.fillAmount = (currentStamina / stamina);
    }

    public void TurnStarted()
    { 
        Init(1, baseData);
        usedAbilityThisTurn = false;
        moveRemaining = speed;
        Float stamToRegen = new Float(staminaRegen);
        myEvents.modStamRegen.Invoke(this, stamToRegen);
        currentStamina = Mathf.Min(stamina, currentStamina + stamToRegen.flt);
        Float manaToRegen = new Float(manaRegen);
        myEvents.modManaRegen.Invoke(this, manaToRegen);
        currentMana = Mathf.Min(mana, currentMana + manaToRegen.flt);
        updateBars();
        myEvents.onTurnStarted.Invoke(this);
        if (myTeam != 0)
        {
            myAI.OnTurnStarted();
        }
        else
        {
            MainCombatManager.manager.SendAbilities(activeAbilities);
        }
    }

    public void Init(int inputlevel, UnitData inBaseData=null /*, TODO: EquipmentData */)
    {
        if (inBaseData == null) inBaseData = baseData;
        if (initialized) return;
        baseData = inBaseData;
        level = inputlevel;
        inputlevel -= 1;
        myElement = inBaseData.myElement;
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
        foreach (string abilityID in inBaseData.baseActiveAbilities)
        {
            ActiveAbility newAbility = AbilityFactory.getActiveAbility(abilityID, this);
            activeAbilities.Add(newAbility);
        }
        currentHealth = health;
        currentMana = mana;
        currentStamina = stamina;
        initialized = true;
        if (!isFriendly)
        {
            VisionManager.visionManager.positionConcealed.AddListener(ConcealMe);
            VisionManager.visionManager.positionRevealed.AddListener(RevealMe);
        }
    }

    private void OnDisable()
    {
        if (!isFriendly)
        {
            VisionManager.visionManager.positionConcealed.RemoveListener(ConcealMe);
            VisionManager.visionManager.positionRevealed.RemoveListener(RevealMe);
        }
    }

    public bool PayCost(ActiveAbility ability, bool payNow = true)
    {
        if (ability.abilityData.staminaCost > 0)
        {
            Float cost = new Float(ability.abilityData.staminaCost);
            myEvents.modStamCost.Invoke(this, ability, cost);
            if (cost.flt > currentStamina)
            {
                return false;
            }

            if (payNow)
            {
                currentStamina -= cost.flt;
                myEvents.onPayStam.Invoke(this, cost);
            }
            
        }
        if (ability.abilityData.manaCost > 0)
        {
            Float cost = new Float(ability.abilityData.manaCost);
            myEvents.modManaCost.Invoke(this, ability, cost);
            if (cost.flt > currentMana)
            {
                return false;
            }

            if (payNow)
            {
                currentMana -= cost.flt;
                myEvents.onPayMana.Invoke(this, cost);
            }
            
        }
        if(payNow) updateBars();
        return true;
    }

    public void ReceiveAttack(Attack.AttackMessageToTarget attack)
    {
        attack.damage *= typeMatchups[myElement][attack.element];
        //TODO: effects for certain type matchups
        myEvents.onAttacked.Invoke(this, attack);
        if (attack.damageType == AttackData.DamageType.Physical)
        {
            attack.damage -= physicalDefence;
        }
        else if (attack.damageType == AttackData.DamageType.Magic)
        {
            attack.damage -= magicalDefence;
        }
        if (attack.damage < 0)
            attack.damage = 0;
        currentHealth -= attack.damage;
        updateBars();
        //TODO: apply effect on damage
        myEvents.onTakeDamage.Invoke(this, attack.damage);
        if (currentHealth <= 0)
        {
            myEvents.onDeath.Invoke(this);
            Die();
        }
    }

    public void ModifyOutgoingAttack(Attack.AttackMessageToTarget attack)
    {
        myEvents.applyToOutgoingAttack.Invoke(this, attack);
    }

    public void Die()
    {
        MainCombatManager.manager.registerUnitDead(this);
        Destroy(gameObject);
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

    public void RevealMe(Vector3Int position)
    {
        if (currentPosition.Equals(position))
        {
            mySprite.enabled = true;
            mySprite.enabled = true;
            healthBar.enabled = true;
            manaBar.enabled = true;
            stamBar.enabled = true;
        }
    }

    public void ConcealMe(Vector3Int position)
    {
        if (currentPosition.Equals(position))
        {
            mySprite.enabled = false;
            healthBar.enabled = false;
            manaBar.enabled = false;
            stamBar.enabled = false;
        }
    }

    public IEnumerator MoveUnit(HexTileUtility.DjikstrasNode target, Tilemap mainMap, UnityAction<bool> returnToCaller = null)
    {
        forceMove = false;
        List<Vector3Int> allInRange = VisionManager.visionManager.DjikstrasSightCheck(currentPosition, sightRadius);
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
            if(isFriendly)
                VisionManager.visionManager.UpdateVision(this);
            else
            {
                if (VisionManager.visionManager.isRevealed(currentPosition))
                {
                    RevealMe(currentPosition);
                }
                else
                {
                    ConcealMe(currentPosition);
                }
            }
            List<Vector3Int> newInRange = VisionManager.visionManager.DjikstrasSightCheck(currentPosition, sightRadius);
            List<UnitBase> compList;
            if (isFriendly) compList = MainCombatManager.manager.allEnemy;
            else compList = MainCombatManager.manager.allFriendly;
            if (UnitInDiff(newInRange.diff(allInRange), compList, isFriendly))
            {
                if (returnToCaller != null) returnToCaller(false);
                break;
            }

            allInRange = newInRange;
        }
    }

    private bool UnitInDiff(List<Vector3Int> tiles, List<UnitBase> units, bool sharedSight = true)
    {
        foreach (UnitBase unit in units)
        {
            if (tiles.Contains(unit.currentPosition))
            {
                if(VisionManager.visionManager.GetViewers(unit.currentPosition).Count == 1 || !sharedSight)
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

    public bool Equals(UnitBase other)
    {
        if (other == null) return false;
        return other.myId.Equals(myId);
    }
}
