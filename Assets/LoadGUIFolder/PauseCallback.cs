using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Misc
{
    //define callbacks for pause and resume
    public class PauseCallback : MonoBehaviour
    {
        public static PauseCallback pauseManager;

        public bool isPaused = false;
    
        private UnityEvent pauseCallback = new UnityEvent();

        private UnityEvent resumeCallback = new UnityEvent();

        private void Awake()
        {
            if (pauseManager != null)
            {
                Destroy(gameObject);
                return;
            }
            pauseManager = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        //sub to pause event
        public void SubscribeToPause(UnityAction func)
        {
            pauseCallback.AddListener(func);
        }

        //unsub to pause event
        public void UnsubToPause(UnityAction func)
        {
            pauseCallback.RemoveListener(func);
        }
        
        //sub to resume event
        public void SubscribeToResume(UnityAction func)
        {
            resumeCallback.AddListener(func);
        }

        //unsub to resume event
        public void UnsubToResume(UnityAction func)
        {
            resumeCallback.RemoveListener(func);
        }

        //wrapper around UnityEvent.Invoke
        public void Pause()
        {
            if (!isPaused)
            {
                pauseCallback.Invoke();
                AudioListener.pause = true;
                isPaused = true;
                Time.timeScale = 0;
            }
        }

        //wrapper around UnityEvent.Invoke
        public void Resume()
        {
            if (isPaused)
            {
                resumeCallback.Invoke();
                AudioListener.pause = false;
                isPaused = false;
                Time.timeScale = 1;
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Resume();
        }
    }
}
