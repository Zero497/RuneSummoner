using UnityEngine;

public class SavesBackButton : MonoBehaviour
{
    public GameObject mainObject;
    
    public GameObject savesObject;

    public Transform savesContent;

    public GameObject backButton;
    
    public void OnClick()
    {
        foreach (Transform child in savesContent)
        {
            Destroy(child.gameObject);
        }
        mainObject.SetActive(true);
        savesObject.SetActive(false);
        backButton.SetActive(false);
    }
}
