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
    //Inspector assignments
    public UnitData baseData;
    public SpriteRenderer mySprite;
    public Image healthBar;
    public Image manaBar;
    public Image stamBar;

    //stats
    [NonSerialized]public UnitData.Element myElement;
    [NonSerialized]public int level;
    [NonSerialized]public float summonCost;

    [NonSerialized] public UnitCombatStats myCombatStats;
    public float health => myCombatStats.getHealth();
    public float initiative => myCombatStats.getInitiative();
    public float abilityPower => myCombatStats.getAbilityPower();
    public float magicalAttack => myCombatStats.getMagicalAttack();
    public float physicalAttack => myCombatStats.getPhysicalAttack();
    public float magicalDefence => myCombatStats.getMagicalDefense();
    public float physicalDefence => myCombatStats.getPhysicalDefense();
    public float mana => myCombatStats.getMana();
    public float manaRegen => myCombatStats.getManaRegen();
    public float sightRadius => myCombatStats.getSightRadius();
    public float speed => myCombatStats.getSpeed();
    public float stamina => myCombatStats.getStamina();
    public float staminaRegen => myCombatStats.getStaminaRegen();
    
    public float currentHealth => myCombatStats.getCurrentHealth();
    public float currentMana => myCombatStats.getCurrentMana();
    public float currentStamina => myCombatStats.getCurrentStamina();
    
    //abilities and effects
    [NonSerialized]public List<ActiveAbility> activeAbilities = new List<ActiveAbility>();
    [NonSerialized]public List<PassiveAbility> passiveAbilities = new List<PassiveAbility>();
    [NonSerialized] public List<Effect> activeEffects = new List<Effect>();
    [NonSerialized] public Move myMovement;
    
    //tracking data
    [NonSerialized]public bool usedAbilityThisTurn;
    [NonSerialized]public float moveRemaining;
    [NonSerialized]public Vector3Int currentPosition;
    
    //identifying data
    [NonSerialized]public string myId;
    [NonSerialized]public bool isFriendly;
    [NonSerialized]public int myTeam = 0;

    //other
    [NonSerialized]public FSM myAI = null;
    [NonSerialized]public bool forceMove;
    private bool initialized = false;
    
    //events
    public UnitEvents myEvents = new UnitEvents();

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
        myElement = inBaseData.myElement;
        summonCost = inBaseData.summonCost;
        myCombatStats = new UnitCombatStats(inBaseData, inputlevel, myEvents, this);
        myMovement = new Move();
        SendData data = new SendData(this);
        data.AddStr("move");
        data.AddStr("move");
        foreach (string abilityID in inBaseData.baseActiveAbilities)
        {
            ActiveAbility newAbility = AbilityFactory.getActiveAbility(abilityID, this);
            activeAbilities.Add(newAbility);
        }
        initialized = true;
        if (!isFriendly)
        {
            ActionPriorityWrapper<UnitBase> conceal = new ActionPriorityWrapper<UnitBase>();
            conceal.priority = 32;
            conceal.action = ConcealMe;
            ActionPriorityWrapper<UnitBase, UnitBase> reveal = new ActionPriorityWrapper<UnitBase, UnitBase>();
            reveal.priority = 32;
            reveal.action = RevealMe;
            myEvents.onPositionConcealed.Subscribe(conceal);
            myEvents.onPositionRevealed.Subscribe(reveal);
        }
        TurnController.controller.nextEventStarting.AddListener(Regen);
    }

    public void Regen(float time, TurnController.TimeWrapper n)
    {
        myCombatStats.RegenStamina(time);
        myCombatStats.RegenMana(time);
    }

    private void OnDisable()
    {
        TurnController.controller.nextEventStarting.RemoveListener(Regen);
    }

    public bool PayCost(ActiveAbility ability, bool payNow = true)
    {
        if (ability.abilityData.staminaCost > 0)
        {
            Float cost = ability.GetStaminaCost();
            myEvents.modStamCost.Invoke(this, ability, cost);
            if (cost.flt > currentStamina)
            {
                return false;
            }

            if (payNow)
            {
                myCombatStats.AddCurrentStamina(-cost.flt);
                myEvents.onPayStam.Invoke(this, cost);
            }
            
        }
        if (ability.abilityData.manaCost > 0)
        {
            Float cost = ability.GetManaCost();
            myEvents.modManaCost.Invoke(this, ability, cost);
            if (cost.flt > currentMana)
            {
                return false;
            }

            if (payNow)
            {
                myCombatStats.AddCurrentMana(-cost.flt);
                myEvents.onPayMana.Invoke(this, cost);
            }
            
        }
        if(payNow) updateBars();
        return true;
    }

    public void TakeDamage(AttackData.DamageType dtype, UnitData.Element element, float damage)
    {
        damage *= typeMatchups[myElement][element];
        //TODO: effects for certain type matchups
        if (dtype == AttackData.DamageType.Physical)
        {
            damage -= physicalDefence;
        }
        else if (dtype == AttackData.DamageType.Magic)
        {
            damage -= magicalDefence;
        }
        if (damage < 0)
            damage = 0;
        myCombatStats.AddCurrentHealth(damage);
    }

    public void ReceiveAttack(Attack.AttackMessageToTarget attack)
    {
        if(attack.element != UnitData.Element.none)
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
        myCombatStats.AddCurrentHealth(attack.damage);
        if (attack.effectsToApplyTarget != null)
        {
            for(int i = 0; i<attack.effectsToApplyTarget.Count; i++)
            {
                activeEffects.Add(EffectFactory.GetEffect(attack.effectsToApplyTarget[i], this, attack.stacksToApply[i]));
            }
        }
        updateBars();
        
        
        if (currentHealth <= 0)
        {
            myEvents.onDeath.Invoke(this);
            Die();
        }
    }

    public void RemoveEffect(SendData data)
    {
        foreach (Effect effect in activeEffects)
        {
            if (effect.Equals(data))
            {
                effect.RemoveEffect();
            }
        }
    }

    public void RemoveEffect(Effect toRemove)
    {
        activeEffects.Remove(toRemove);
    }

    public Effect AddEffect(Effect effect)
    {
        foreach (Effect activeEffect in activeEffects)
        {
            if (activeEffect.MergeEffects(effect))
            {
                myEvents.onEffectApplied.Invoke(this, activeEffect, effect.getStacks());
                return activeEffect;
            }
        }
        myEvents.onEffectApplied.Invoke(this, effect, effect.getStacks()); 
        activeEffects.Add(effect);
        return effect;
    }

    public Effect AddEffect(SendData data)
    {
        foreach (Effect activeEffect in activeEffects)
        {
            if (activeEffect.MergeEffects(data))
            {
                myEvents.onEffectApplied.Invoke(this, activeEffect, (int) data.floatData[0]);
                return activeEffect;
            }
        }
        Effect newEffect = EffectFactory.GetEffect(data);
        myEvents.onEffectApplied.Invoke(this, newEffect, (int) data.floatData[0]); 
        activeEffects.Add(newEffect);
        return newEffect;
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

    public void RevealMe(UnitBase me, UnitBase revealer)
    {
        mySprite.enabled = true;
        mySprite.enabled = true;
        healthBar.enabled = true;
        manaBar.enabled = true;
        stamBar.enabled = true;
    }

    public void ConcealMe(UnitBase me)
    {
        mySprite.enabled = false;
        healthBar.enabled = false;
        manaBar.enabled = false;
        stamBar.enabled = false;
    }

    public IEnumerator MoveUnit(HexTileUtility.DjikstrasNode target, Tilemap mainMap, UnityAction<bool> returnToCaller = null)
    {
        forceMove = false;
        List<Vector3Int> allInRange = VisionManager.visionManager.DjikstrasSightCheck(currentPosition, sightRadius);
        myEvents.onMoveStart.Invoke(this, target);
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
                VisionManager.visionManager.UpdateFriendlyVision(this);
            else
            {
                VisionManager.visionManager.UpdateEnemyVision(this);
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
        myEvents.onMoveEnd.Invoke(this, target);
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
        {UnitData.Element.beastial, new Dictionary<UnitData.Element, float>()
        {
            { UnitData.Element.beastial, 1.25f },
            { UnitData.Element.humanoid, 1.25f },
            { UnitData.Element.construct, 1f }
        }},
        {UnitData.Element.humanoid, new Dictionary<UnitData.Element, float>()
        {
            { UnitData.Element.beastial, 0.75f },
            { UnitData.Element.humanoid, 1.25f },
            { UnitData.Element.construct, 1f }
        }},
        {UnitData.Element.construct, new Dictionary<UnitData.Element, float>()
        {
            { UnitData.Element.beastial, 0.75f },
            { UnitData.Element.humanoid, 1f },
            { UnitData.Element.construct, 0.75f }
        }}
    };

    public bool Equals(UnitBase other)
    {
        if (other == null) return false;
        return other.myId.Equals(myId);
    }
}
