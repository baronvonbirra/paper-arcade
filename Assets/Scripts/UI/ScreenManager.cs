using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaperDollArcade
{
    [Serializable]
    public class ScreenEntry
    {
        public string screenName;
        public GameObject screenGameObject;
        public string sceneName;
    }

    public class ScreenManager : MonoBehaviour
    {
        public static ScreenManager Instance { get; private set; }

        [SerializeField] private List<ScreenEntry> screens = new List<ScreenEntry>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OpenScreen(string screenName)
        {
            ScreenEntry target = screens.Find(s => s.screenName.Equals(screenName, StringComparison.OrdinalIgnoreCase));
            if (target != null)
            {
                if (target.screenGameObject != null)
                {
                    target.screenGameObject.SetActive(true);
                }
                else if (!string.IsNullOrEmpty(target.sceneName))
                {
                    SceneManager.LoadScene(target.sceneName);
                }
            }
            else
            {
                // Fallback attempt to load scene by screenName
                if (Application.CanStreamedLevelBeLoaded(screenName))
                {
                    SceneManager.LoadScene(screenName);
                }
            }
        }

        public void CloseScreen(string screenName)
        {
            ScreenEntry target = screens.Find(s => s.screenName.Equals(screenName, StringComparison.OrdinalIgnoreCase));
            if (target != null && target.screenGameObject != null)
            {
                target.screenGameObject.SetActive(false);
            }
        }
    }
}
