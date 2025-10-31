

public class UnitEvents
{
    public EventPriorityWrapper<UnitBase, Float> onPayStam = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, Float> onPayMana = new EventPriorityWrapper<UnitBase, Float>();
    
    //original cost before event and new cost
    public EventPriorityWrapper<UnitBase, float, Float> modStamCost = new EventPriorityWrapper<UnitBase, float, Float>();
    
    //original cost before event and new cost
    public EventPriorityWrapper<UnitBase, float, Float> modManaCost = new EventPriorityWrapper<UnitBase, float, Float>();
    
    public EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttacked = new EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();

    public EventPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode> onMoveStart =
        new EventPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode>();
    
    public EventPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode> onMoveEnd =
        new EventPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode>();
    
    //original damage before event and new damage
    public EventPriorityWrapper<UnitBase, float, Float> modifyIncomingDamageAfterDef = new EventPriorityWrapper<UnitBase, float, Float>();

    //original damage before event and new damage
    public EventPriorityWrapper<UnitBase, float, Float> modifyIncomingDamageBeforeDef = new EventPriorityWrapper<UnitBase, float, Float>();
    
    public EventPriorityWrapper<UnitBase, float> onTakeDamage = new EventPriorityWrapper<UnitBase, float>();
    
    public EventPriorityWrapper<UnitBase, float> onRegainHealth = new EventPriorityWrapper<UnitBase, float>();
    
    public EventPriorityWrapper<UnitBase> onDeath = new EventPriorityWrapper<UnitBase>();
    
    public EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> applyToOutgoingAttack = new EventPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();

    public EventPriorityWrapper<UnitBase, Float> modManaRegen = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, Float> modStamRegen = new EventPriorityWrapper<UnitBase, Float>();
    
    public EventPriorityWrapper<UnitBase, float> onManaRegen = new EventPriorityWrapper<UnitBase, float>();
    
    public EventPriorityWrapper<UnitBase, float> onStamRegen = new EventPriorityWrapper<UnitBase, float>();

    public EventPriorityWrapper<UnitBase> onTurnStarted = new EventPriorityWrapper<UnitBase>();
    
    public EventPriorityWrapper<UnitBase> onTurnEnded = new EventPriorityWrapper<UnitBase>();

    public EventPriorityWrapper<UnitBase, Effect, int> modifyIncomingEffect = new EventPriorityWrapper<UnitBase, Effect, int>();

    public EventPriorityWrapper<UnitBase, Effect, int> onEffectApplied = new EventPriorityWrapper<UnitBase, Effect, int>();
    
    public EventPriorityWrapper<UnitBase, Effect> onEffectRemoved = new EventPriorityWrapper<UnitBase, Effect>();

    //self, revealing unit
    public EventPriorityWrapper<UnitBase, UnitBase> onPositionRevealed = new EventPriorityWrapper<UnitBase, UnitBase>();
    
    public EventPriorityWrapper<UnitBase> onPositionConcealed = new EventPriorityWrapper<UnitBase>();
}
