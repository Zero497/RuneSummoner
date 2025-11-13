using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour
{
    public void OnClick()
    {
        SaveHandler.NewGameNewSlot();
        SceneManager.LoadScene("MainScene");
    }
}
