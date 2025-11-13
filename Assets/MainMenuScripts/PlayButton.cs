using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void OnClick()
    {
        SaveHandler.setSavePath();
        SceneManager.LoadScene("MainScene");
    }
}
