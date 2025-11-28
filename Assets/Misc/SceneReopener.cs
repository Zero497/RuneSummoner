using System;
using LoadGUIFolder;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReopener : MonoBehaviour
{
    public static string Reopen = "";

    private void Awake()
    {
        if (Reopen.Equals("")) return;
        LoadGUIManager.loadGUIManager.Load(Reopen);
        Reopen = "";
    }
}
