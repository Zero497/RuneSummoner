using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInfoButton : MonoBehaviour
{
    private bool isActive;
    
    private ActiveAbility.ActiveAbilityDes active;

    private PassiveAbility.PassiveAbilityDes passive;

    private int level;

    public UnitSimple unit;

    public TextMeshProUGUI nameText;

    public TextMeshProUGUI descText;

    public Image icon;

    public Sprite selectedSprite;

    public Sprite defaultSprite;

    public Image buttonRenderer;

    private static AbilityInfoButton openAbility;

    public void Init(ActiveAbility.ActiveAbilityDes AAb, int lvl, UnitSimple unitSimple)
    {
        active = AAb;
        level = lvl;
        isActive = true;
        unit = unitSimple;
        ActiveAbility.AbilityText txt = ActiveAbility.GetAbilityText(AAb, unit, level);
        nameText.text = txt.name;
        descText.text = txt.desc;
        icon.sprite = txt.icon;
    }

    public void Init(PassiveAbility.PassiveAbilityDes PAb, int lvl, UnitSimple unitSimple)
    {
        passive = PAb;
        level = lvl;
        isActive = false;
        unit = unitSimple;
        PassiveData data = PassiveAbility.GetPassiveData(PAb);
        nameText.text = data.name;
        descText.text = data.description;
        icon.sprite = data.icon;
    }

    public void OnClick()
    {
        if (openAbility == null || openAbility != this)
        {
            if (openAbility != null)
                openAbility.buttonRenderer.sprite = openAbility.defaultSprite;
            openAbility = this;
            AbilityTextPanel.panel.gameObject.SetActive(true);
            if (isActive)
            {
                AbilityTextPanel.panel.descriptionText.text =
                    ActiveAbility.AbilityTextToFullDesc(ActiveAbility.GetAbilityText(active, unit, level), level);
            }
            else
            {
                AbilityTextPanel.panel.descriptionText.text = PassiveAbility.GetPassiveFullText(passive, unit, level);
            }

            buttonRenderer.sprite = selectedSprite;
        }
        else
        {
            openAbility = null;
            AbilityTextPanel.panel.gameObject.SetActive(false);
            buttonRenderer.sprite = defaultSprite;
        }
        
    }
}
