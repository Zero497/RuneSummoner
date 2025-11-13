using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchSaveButton : MonoBehaviour
{
    [NonSerialized]public string slotName;
    
    public void OnClick()
    {
        SaveHandler.setSavePath(slotName);
        SceneManager.LoadScene("MainScene");
    }
}
