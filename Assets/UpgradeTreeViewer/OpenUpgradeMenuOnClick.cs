using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenUpgradeMenuOnClick : MonoBehaviour
{
    public static UnitSimple curUnit;
    
    public void OnClick()
    {
        UpgradeTreeManager.unit = curUnit;
        SceneReopener.Reopen = "SpiritManager";
        SceneManager.LoadScene("UpgradeScene");
    }
}
