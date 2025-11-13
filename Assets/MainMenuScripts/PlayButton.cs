using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void OnClick()
    {
        SaveHandler.setSavePath();
        SaveHandler.Load();
        SceneManager.LoadScene("MainScene");
    }
}
