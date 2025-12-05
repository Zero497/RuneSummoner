using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public TextMeshProUGUI myText;

    public Image myImage;
    
    private ActiveAbility myAbility;

    public void SetAbility(ActiveAbility ability)
    {
        myAbility = ability;
        myText.text = ability.abilityData.abilityName;
        myImage.sprite = ActiveAbility.GetAbilityText(ability.myDes, TurnController.controller.currentActor.mySimple,
            TurnController.controller.currentActor.level).icon;
    }
    
    public void OnClick()
    {
        myAbility.PrepAction();
    }
}
