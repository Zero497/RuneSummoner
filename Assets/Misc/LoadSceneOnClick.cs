using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public string sceneName;
    public void OnClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}
