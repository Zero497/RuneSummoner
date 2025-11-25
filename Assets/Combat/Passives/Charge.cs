using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Charge : PassiveAbility
{
    private List<Vector3Int> mostRecentLine = new List<Vector3Int>();
    
    private ActionPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode> onMoved;

    private ActionPriorityWrapper<UnitBase> onTurnStarted;

    private ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget> onAttack;
    
    
    public override string GetAbilityName()
    {
        return "Charge";
    }

    /*
        Expects:
            Unit 0: unit to apply to
            Float 0: level of ability
     */
    public override void Initialize(SendData data)
    {
        base.Initialize(data);
        onMoved = new ActionPriorityWrapper<UnitBase, HexTileUtility.DjikstrasNode>();
        onMoved.priority = 64;
        onMoved.action = OnMoved;
        source.myEvents.onMoveEnd.Subscribe(onMoved);
        onTurnStarted = new ActionPriorityWrapper<UnitBase>();
        onTurnStarted.priority = 64;
        onTurnStarted.action = OnTurnStated;
        source.myEvents.onTurnStarted.Subscribe(onTurnStarted);
        onAttack = new ActionPriorityWrapper<UnitBase, Attack.AttackMessageToTarget>();
        onAttack.priority = 16;
        onAttack.action = OnAttack;
        source.myEvents.applyToOutgoingAttack.Subscribe(onAttack);
    }

    private void OnAttack(UnitBase myUnit, Attack.AttackMessageToTarget attack)
    {
        if (Mathf.FloorToInt(attack.sourceAttack.GetRange()) == 1)
        {
            attack.damage += attack.baseDamage * (1 + 0.1f * level * mostRecentLine.Count);
        }
    }

    private void OnTurnStated(UnitBase myUnit)
    {
        mostRecentLine = new List<Vector3Int>();
    }

    private void OnMoved(UnitBase myUnit, HexTileUtility.DjikstrasNode moveTarget)
    {
        HexTileUtility.DjikstrasNode cur = moveTarget;
        List<Vector3Int> newLine = new List<Vector3Int>();
        while (cur != null)
        {
            if (HexTileUtility.isInLine(cur.location, newLine[0]) && !HexTileUtility.AreAdjacent(newLine[^2], cur.location))
            {
                newLine.Add(cur.location);
                cur = cur.parent;
            }
            else
            {
                break;
            }
        }

        if (mostRecentLine.Count > 0 && HexTileUtility.isInLine(newLine[0], mostRecentLine[^1]))
        {
            if (mostRecentLine.Count == 1 || !HexTileUtility.AreAdjacent(newLine[0], mostRecentLine[^1]))
            {
                for (int i = 0; i < mostRecentLine.Count; i++)
                {
                    newLine.Add(mostRecentLine[i]);
                }
            }
        }

        mostRecentLine = newLine;
    }
    
    public static string GetFullText(int level)
    {
        string ret = "Name: Charge\n";
        ret += "Description: Damage this Unit deals with melee (range 1) attacks is increased by "+(level*10)+"% (10% base) for each tile this Unit moved in a straight line before using that attack.\n";
        ret += "Level Effect: +10% damage bonus per tile per Level.\n";
        return ret;
    }
}
