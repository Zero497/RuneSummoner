using UnityEngine;

[CreateAssetMenu(fileName = "DummyNode", menuName = "FSMNodes/DummyNode")]
public class DummyNode : FSMNode
{
    public override void OnEnter(UnitBase unit)
    {
        return;
    }

    public override void OnExit(UnitBase unit)
    {
        return;
    }

    public override void OnTurnStarted(UnitBase unit)
    {
        MainCombatManager.manager.EndTurn();
    }
}
