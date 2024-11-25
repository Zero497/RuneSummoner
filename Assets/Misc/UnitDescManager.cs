using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitDescManager : MonoBehaviour
{
    public TextMeshProUGUI unitID;
    
    public Image unitPortrait;

    public TextMeshProUGUI unitName;

    public TextMeshProUGUI team;

    public TextMeshProUGUI element;

    public TextMeshProUGUI combatType;
    
    public TextMeshProUGUI level;

    public TextMeshProUGUI health;

    public TextMeshProUGUI mana;

    public TextMeshProUGUI stamina;

    public TextMeshProUGUI initiative;

    public TextMeshProUGUI abilityPower;

    public TextMeshProUGUI magicAttack;

    public TextMeshProUGUI physicalAttack;
    
    public TextMeshProUGUI magicDefense;
    
    public TextMeshProUGUI physicalDefense;
    
    public TextMeshProUGUI manaRegen;
    
    public TextMeshProUGUI staminaRegen;

    public TextMeshProUGUI speed;

    public TextMeshProUGUI sightRadius;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        UnitBase target = TurnController.controller.getUnitAtPosition(DescriptionViewLoader.targetTile);
        unitID.text = target.myId;
        unitName.text = "Unit Name: "+target.baseData.name;
        unitPortrait.sprite = target.baseData.portrait;
        if(target.isFriendly)
            team.text = "Allegiance: Friendly";
        else
        {
            team.text = "Allegiance: Team "+target.myTeam;
        }
        element.text = "Type: "+target.myElement.ToString();
        combatType.text = "Specialization: "+target.baseData.myType.ToString();
        level.text = "Level: "+target.level;
        health.text = "Health: "+target.health+"/"+target.currentHealth;
        mana.text = "Mana: "+target.mana+"/"+target.currentMana;
        stamina.text = "Stamina: "+target.stamina+"/"+target.currentStamina;
        initiative.text = "Initiative: "+target.initiative;
        abilityPower.text = "Ability Power: "+target.abilityPower;
        magicAttack.text = "Magical Attack: " + target.magicalAttack;
        physicalAttack.text = "Physical Attack: " + target.physicalAttack;
        magicDefense.text = "Magical Defense: " + target.magicalDefence;
        physicalDefense.text = "Physical Defense: " + target.physicalDefence;
        manaRegen.text = "Mana Regen: "+target.manaRegen;
        staminaRegen.text = "Stamina Regen: "+target.staminaRegen;
        speed.text = "Speed: "+target.speed;
        sightRadius.text = "Sight Radius: "+target.sightRadius;
    }
}
