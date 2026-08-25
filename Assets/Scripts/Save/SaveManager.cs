using System;
using System.IO;
using UnityEngine;

namespace PaperDollArcade
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private PlayerSaveData currentSaveData;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadGame();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public string GetSaveFilePath()
        {
            return Path.Combine(Application.persistentDataPath, Constants.SAVE_FILE_NAME);
        }

        public bool ExistsSave()
        {
            return File.Exists(GetSaveFilePath());
        }

        public void SaveGame(PlayerSaveData saveData)
        {
            currentSaveData = saveData;
            string json = SerializeToJson(saveData);
            try
            {
                File.WriteAllText(GetSaveFilePath(), json);
                Debug.Log($"Game saved successfully to {GetSaveFilePath()}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save game: {ex.Message}");
            }
        }

        public PlayerSaveData LoadGame()
        {
            if (currentSaveData != null) return currentSaveData;

            string filePath = GetSaveFilePath();
            if (ExistsSave())
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    currentSaveData = DeserializeFromJson(json);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load save file: {ex.Message}. Creating default save.");
                    currentSaveData = CreateDefaultSaveData();
                }
            }
            else
            {
                currentSaveData = CreateDefaultSaveData();
            }

            return currentSaveData;
        }

        public void DeleteSave()
        {
            string filePath = GetSaveFilePath();
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            currentSaveData = CreateDefaultSaveData();
        }

        public string SerializeToJson(PlayerSaveData data)
        {
            return JsonUtility.ToJson(data, true);
        }

        public PlayerSaveData DeserializeFromJson(string json)
        {
            return JsonUtility.FromJson<PlayerSaveData>(json);
        }

        private PlayerSaveData CreateDefaultSaveData()
        {
            PlayerSaveData data = new PlayerSaveData();
            // Default initial items
            data.unlockedBackgroundIds.Add("room");
            return data;
        }
    }
}
