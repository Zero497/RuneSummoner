using System;
using TMPro;
using UnityEngine;

public class AbilityTextPanel : MonoBehaviour
{
    public static AbilityTextPanel panel;

    public TextMeshProUGUI descriptionText;

    public void Awake()
    {
        panel = this;
        gameObject.SetActive(false);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
