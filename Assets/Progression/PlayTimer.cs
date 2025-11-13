using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayTimer : MonoBehaviour
{
    public static PlayTimer timer;

    [NonSerialized]public float time;

    private ActionPriorityWrapper<string> load;

    private ActionPriorityWrapper<string> newGame;

    private void Awake()
    {
        if (timer != null)
        {
            Destroy(gameObject);
            return;
        }
        timer = this;
        DontDestroyOnLoad(gameObject);
        SaveHandler.Init();
        UnitManager.Init();
        InventoryManager.Init();
        StartCoroutine(Timer());
        load = new ActionPriorityWrapper<string>();
        load.action = Load;
        SaveHandler.load.Subscribe(load);
        newGame = new ActionPriorityWrapper<string>();
        newGame.action = NewGame;
        SaveHandler.newGame.Subscribe(newGame);
    }

    private void OnApplicationQuit()
    {
        SaveHandler.Save();
    }

    private void Load(string path)
    {
        time = float.Parse(SaveHandler.readMetaFile(path).Item2);
    }

    private void NewGame(string path)
    {
        time = 0;
    }

    private IEnumerator Timer()
    {
        yield return new WaitForNextFrameUnit();
        time += Time.deltaTime;
    }
}
