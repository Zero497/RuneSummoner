using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    public void OnClick()
    {
        if (TurnController.controller.currentActor.myTeam == 0)
        {
            MainCombatManager.manager.EndTurn();
        }
    }
}
