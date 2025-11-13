using System;
using UnityEngine;

//handles GUI additive loading
namespace LoadGUIFolder
{
    public class LoadGUI : MonoBehaviour
    {
        [Tooltip("Scene to load")]
        public String loadScene;

        //if the gui is open, reenable player movement and close it
        //otherwise close any other active gui and open this one
        public virtual void ONOpenTrigger()
        {
            LoadGUIManager.loadGUIManager.Load(loadScene);
        }
    }
}
