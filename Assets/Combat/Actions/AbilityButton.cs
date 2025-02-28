using TMPro;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    public TextMeshProUGUI myText;
    
    private ActiveAbility myAbility;

    public void SetAbility(ActiveAbility ability)
    {
        myAbility = ability;
        myText.text = ability.abilityData.abilityName;
    }
    
    public void OnClick()
    {
        if(!myAbility.source.usedAbilityThisTurn)
            myAbility.PrepAction();
    }
}
