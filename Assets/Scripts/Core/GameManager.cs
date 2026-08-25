using UnityEngine;
using UnityEngine.Events;

namespace PaperDollArcade
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public UnityEvent<string> OnGameStateChanged = new UnityEvent<string>();

        private float playSessionTimer;

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

        private void Start()
        {
            InitializeGame();
        }

        private void Update()
        {
            playSessionTimer += Time.deltaTime;
        }

        private void OnApplicationQuit()
        {
            SaveProgress();
        }

        public void InitializeGame()
        {
            PlayerSaveData saveData = SaveManager.Instance.LoadGame();
            Debug.Log("GameManager initialized.");
        }

        public void SaveProgress()
        {
            PlayerSaveData saveData = SaveManager.Instance.LoadGame();
            saveData.totalPlayTime += Mathf.RoundToInt(playSessionTimer);
            playSessionTimer = 0f;
            SaveManager.Instance.SaveGame(saveData);
        }

        public void ChangeState(string newState)
        {
            OnGameStateChanged.Invoke(newState);
        }
    }
}
