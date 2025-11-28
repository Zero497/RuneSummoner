using UnityEngine;

public class Berserker : PassiveAbility
{
    private bool isActive = false;

    private ActionPriorityWrapper<UnitBase, string, float> onStatChanged;

    private ActionPriorityWrapper<UnitBase, float> onRegainedHealth;
    
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        CheckValid();
        onStatChanged = new ActionPriorityWrapper<UnitBase, string, float>();
        onStatChanged.priority = 50;
        onStatChanged.action = OnStatChanged;
        source.myCombatStats.onStatChanged.Subscribe(onStatChanged);
        onRegainedHealth = new ActionPriorityWrapper<UnitBase, float>();
        onRegainedHealth.priority = 50;
        onRegainedHealth.action = OnRegainedHealth;
        source.myEvents.onRegainHealth.Subscribe(onRegainedHealth);
    }
    
    public override string GetAbilityName()
    {
        return "Berserker";
    }

    private void OnRegainedHealth(UnitBase myUnit, float heal)
    {
        CheckValid();
    }

    private void OnStatChanged(UnitBase myUnit, string changeStat, float changAmt)
    {
        if (changeStat.Equals("health"))
        {
            CheckValid();
        }
    }

    private void CheckValid()
    {
        if (source.currentHealth <= source.health * 0.4f)
        {
            ChangeActive(true);
        }
        else
        {
            ChangeActive(false);
        }
    }

    private void ChangeActive(bool change)
    {
        if (isActive == change) return;
        int mod = -1;
        if (change)
        {
            mod = 1;
        }
        source.myCombatStats.AddAbilityPower(source.myCombatStats.getAbilityPower(true)*mod*0.5f*level);
        source.myCombatStats.AddPhysicalAttack(source.myCombatStats.getPhysicalAttack(true)*mod*0.5f*level);
        source.myCombatStats.AddMagicalAttack(source.myCombatStats.getMagicalAttack(true)*mod*0.5f*level);
        source.myCombatStats.AddStaminaRegen(source.myCombatStats.getStaminaRegen(true)*mod*0.5f*level);
        source.myCombatStats.AddManaRegen(source.myCombatStats.getManaRegen(true)*mod*0.5f*level);
        isActive = change;
    }
    
    public static PassiveText GetFullText(int level)
    {
        PassiveText ret = new PassiveText();
        ret.pName = "Berserker";
        ret.desc = "While this Unit has less than 40% max Health remaining, its Ability Power, Physical Attack, Magical Attack, Stamina Regeneration, and Mana Regeneration are all increased by "+(50*level)+"% (50% base).";
        ret.levelEffect = "+50% increase to Ability Power, Physical Attack, Magical Attack, Stamina Regeneration, and Mana Regeneration per Level.";
        return ret;
    }
}
