using System;
using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LoadGUIFolder
{
    public class LoadGUIManager : MonoBehaviour
    {
        public static LoadGUIManager loadGUIManager;
        
        private String GUIName;

        private List<GameObject> popUps = new List<GameObject>();
        
        //store a ref to the HUD object
        private GameObject _hud;

        private GameObject popUp;

        private UnityEvent<string> OnGUIUnload = new UnityEvent<string>();
        
        private UnityEvent<string> OnGUILoad = new UnityEvent<string>();

        public void SubtoLoad(UnityAction<String> action)
        {
            OnGUILoad.AddListener(action);
        }

        public void UnsubtoLoad(UnityAction<String> action)
        {
            OnGUILoad.RemoveListener(action);
        }
        
        public void SubtoUnload(UnityAction<String> action)
        {
            OnGUIUnload.AddListener(action);
        }

        public void UnsubtoUnload(UnityAction<String> action)
        {
            OnGUIUnload.RemoveListener(action);
        }
        
        private void Awake()
        {
            if (loadGUIManager != null)
            {
                Destroy(gameObject);
                return;
            }
            popUp = Resources.Load<GameObject>("PopUp");
            loadGUIManager = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        public void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
        }
        
        private void OnSceneLoad(Scene loaded, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)
            {
                GUIName = null;
                popUps = new List<GameObject>();
            }
        }

        public void InstantiatePopUp(String title, String msg)
        {
            GameObject window = Instantiate(popUp);
            window.GetComponent<PopUpTextManager>().SetText(title, msg);
            window.GetComponent<PopUpOnClick>().index = popUps.Count;
            window.GetComponent<Canvas>().sortingOrder += popUps.Count;
            popUps.Add(window);
            OnGUILoad.Invoke(title);
        }

        public void ClosePopUp()
        {
            Destroy(popUps.Last());
            OnGUIUnload.Invoke(popUps[^1].GetComponent<PopUpTextManager>().titleActual.text);
            popUps.RemoveAt(popUps.Count-1);
            if (!isGUIOpen() && popUps.Count == 0)
            {
                PauseCallback.pauseManager.Resume();
            }
        }

        public void ClosePopUp(int index)
        {
            Destroy(popUps[index]);
            OnGUIUnload.Invoke(popUps[index].GetComponent<PopUpTextManager>().titleActual.text);
            popUps.RemoveAt(index);
            if (!isGUIOpen() && popUps.Count == 0)
            {
                PauseCallback.pauseManager.Resume();
            }
        }

        //returns true if there are no open guis after close, false otherwise
        public bool CloseOpenGUI()
        {
            if (GUIName == null)
                return true;
            if (popUps.Count > 0)
            {
                ClosePopUp();
                return false;
            }
            PauseCallback.pauseManager.Resume();
            OnGUIUnload.Invoke(GUIName);
            SceneManager.UnloadSceneAsync(GUIName);
            GUIName = null;
            return true;
        }

        public bool Load(String toLoad)
        {
            if (toLoad.Equals(GUIName) || toLoad.Equals(""))
            {
                CloseOpenGUI();
                return false;
            }
            if (!CloseOpenGUI())
            {
                return false;
            }
            GUIName = toLoad;
            PauseCallback.pauseManager.Pause();
            SceneManager.LoadScene(GUIName, LoadSceneMode.Additive);
            OnGUILoad.Invoke(GUIName);
            return true;
        }

        public bool isGUIOpen()
        {
            return (GUIName != null);
        }
    }
}
