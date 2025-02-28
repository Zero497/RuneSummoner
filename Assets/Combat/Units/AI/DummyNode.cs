using UnityEngine;

[CreateAssetMenu(fileName = "DummyNode", menuName = "FSMNodes/DummyNode")]
public class DummyNode : FSMNode
{
    public override void OnEnter()
    {
        return;
    }

    public override void OnExit()
    {
        return;
    }

    public override void OnTurnStarted()
    {
        MainCombatManager.manager.EndTurn();
    }
}
