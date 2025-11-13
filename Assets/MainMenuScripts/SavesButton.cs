using System.IO;
using TMPro;
using UnityEngine;

public class SavesButton : MonoBehaviour
{
    public GameObject mainObject;
    
    public GameObject savesObject;

    public GameObject saveButtonPrefab;

    public GameObject backButton;

    public GameObject topText;

    public GameObject newGameButton;
    
    public void OnClick()
    {
        mainObject.SetActive(false);
        savesObject.SetActive(true);
        backButton.SetActive(true);
        Instantiate(topText, savesObject.transform);
        string[] files = Directory.GetDirectories(Application.persistentDataPath);
        foreach (string file in files)
        {
            string slot = file.Split("/")[^1].Split("\\")[^1];
            if (slot.Equals("settings") || slot.Equals("lastPath"))
            {
                continue;
            }
            GameObject newButton = Instantiate(saveButtonPrefab, savesObject.transform);
            newButton.GetComponent<LaunchSaveButton>().slotName = slot;
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = slot;
        }
        Instantiate(newGameButton, savesObject.transform);
    }
}
