using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DescriptionViewLoader
{
    public static Vector3Int targetTile = new Vector3Int(-1000, 0,0);

    public static void LoadDescriptionView(Vector3Int target)
    {
        if (target.Equals(targetTile))
        {
            if (SceneManager.GetSceneByName("UnitDescriptionView").isLoaded)
            {
                SceneManager.UnloadSceneAsync("UnitDescriptionView");
                SceneManager.LoadScene("TileDescriptionView", LoadSceneMode.Additive);
            }
            else if (SceneManager.GetSceneByName("TileDescriptionView").isLoaded)
            {
                SceneManager.UnloadSceneAsync("TileDescriptionView");
            }
            else
            {
                targetTile = new Vector3Int(-1000, 0,0);
            }
        }
        else
        {
            targetTile = target;
            if (MainCombatManager.manager.isTileOccupied(target))
            {
                if (SceneManager.GetSceneByName("UnitDescriptionView").isLoaded)
                {
                    GameObject.Find("BG").GetComponent<UnitDescManager>().Reset();
                }
                else
                {
                    SceneManager.LoadScene("UnitDescriptionView", LoadSceneMode.Additive);
                }
                if (SceneManager.GetSceneByName("TileDescriptionView").isLoaded)
                {
                    SceneManager.UnloadSceneAsync("TileDescriptionView");
                }
            }
            else
            {
                if (SceneManager.GetSceneByName("TileDescriptionView").isLoaded)
                {
                    GameObject.Find("BG").GetComponent<TileDescManager>().Reset();
                }
                else
                {
                    SceneManager.LoadScene("TileDescriptionView", LoadSceneMode.Additive);
                }
            }
        }
    }

    public static void CloseDescriptionView()
    {
        if (SceneManager.GetSceneByName("UnitDescriptionView").isLoaded)
        {
            SceneManager.UnloadSceneAsync("UnitDescriptionView");
        }
        if (SceneManager.GetSceneByName("TileDescriptionView").isLoaded)
        {
            SceneManager.UnloadSceneAsync("TileDescriptionView");
        }
    }
}
