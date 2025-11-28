using TMPro;
using UnityEngine;

public class UpgradeTreeTextPanel : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;

    public void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
