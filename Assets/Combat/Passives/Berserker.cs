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
}
