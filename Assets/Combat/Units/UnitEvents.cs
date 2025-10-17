

public class UnitEvents
{
    public EventPriorityWrapper<UnitBase, Float> onPayStam = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, Float> onPayMana = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, ActiveAbility, Float> modStamCost = new EventPriorityWrapper<UnitBase, ActiveAbility, Float>();
    
    public EventPriorityWrapper<UnitBase, ActiveAbility, Float> modManaCost = new EventPriorityWrapper<UnitBase, ActiveAbility, Float>();
    
    public EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttacked = new EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();

    public EventPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode> onMoveStart =
        new EventPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode>();
    
    public EventPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode> onMoveEnd =
        new EventPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode>();
    
    public EventPriorityWrapper<UnitBase, float> onTakeDamage = new EventPriorityWrapper<UnitBase, float>();
    
    public EventPriorityWrapper<UnitBase> onDeath = new EventPriorityWrapper<UnitBase>();
    
    public EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> applyToOutgoingAttack = new EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();

    public EventPriorityWrapper<UnitBase, Float> modManaRegen = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, Float> modStamRegen = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, Float> onManaRegen = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, Float> onStamRegen = new EventPriorityWrapper<UnitBase, Float>();

    public EventPriorityWrapper<UnitBase> onTurnStarted = new EventPriorityWrapper<UnitBase>();
    
    public EventPriorityWrapper<UnitBase> onTurnEnded = new EventPriorityWrapper<UnitBase>();

    public EventPriorityWrapper<UnitBase, Effect, int> onEffectApplied = new EventPriorityWrapper<UnitBase, Effect, int>();
    
    public EventPriorityWrapper<UnitBase, Effect> onEffectRemoved = new EventPriorityWrapper<UnitBase, Effect>();

    //self, revealing unit
    public EventPriorityWrapper<UnitBase, UnitBase> onPositionRevealed = new EventPriorityWrapper<UnitBase, UnitBase>();
    
    public EventPriorityWrapper<UnitBase> onPositionConcealed = new EventPriorityWrapper<UnitBase>();
}
