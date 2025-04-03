

public class UnitEvents
{
    public EventPriorityWrapper<UnitBase, Float> onPayStam = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, Float> onPayMana = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, ActiveAbility, Float> modStamCost = new EventPriorityWrapper<UnitBase, ActiveAbility, Float>();
    
    public EventPriorityWrapper<UnitBase, ActiveAbility, Float> modManaCost = new EventPriorityWrapper<UnitBase, ActiveAbility, Float>();
    
    public EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttacked = new EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
    
    public EventPriorityWrapper<UnitBase, float> onTakeDamage = new EventPriorityWrapper<UnitBase, float>();
    
    public EventPriorityWrapper<UnitBase> onDeath = new EventPriorityWrapper<UnitBase>();
    
    public EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> applyToOutgoingAttack = new EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();

    public EventPriorityWrapper<UnitBase, Float> modManaRegen = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, Float> modStamRegen = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, Float> onManaRegen = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, Float> onStamRegen = new EventPriorityWrapper<UnitBase, Float>();

    public EventPriorityWrapper<UnitBase> onTurnStarted = new EventPriorityWrapper<UnitBase>();
}
