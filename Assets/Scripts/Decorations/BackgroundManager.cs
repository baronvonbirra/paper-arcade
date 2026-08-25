using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PaperDollArcade
{
    public class BackgroundManager : MonoBehaviour
    {
        public static BackgroundManager Instance { get; private set; }

        [SerializeField] private SpriteRenderer backgroundRenderer;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Transform decorationContainer;

        private List<BackgroundData> allBackgrounds = new List<BackgroundData>();
        private Dictionary<string, DecorationData> allDecorations = new Dictionary<string, DecorationData>();
        private BackgroundData currentBackground;
        private List<DecorationData> activeDecorations = new List<DecorationData>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadAllBackgrounds();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadAllBackgrounds()
        {
            allBackgrounds.Clear();
            allDecorations.Clear();

            // Load background configurations from JSON or Resources
            BackgroundData room = new BackgroundData { backgroundId = "room", name = "Room", type = BackgroundType.Room, spriteKey = "room", isUnlocked = true };
            BackgroundData beach = new BackgroundData { backgroundId = "beach", name = "Beach", type = BackgroundType.Beach, spriteKey = "beach", isUnlocked = true };
            BackgroundData space = new BackgroundData { backgroundId = "space", name = "Space", type = BackgroundType.Space, spriteKey = "space", isUnlocked = true };

            allBackgrounds.Add(room);
            allBackgrounds.Add(beach);
            allBackgrounds.Add(space);

            // Restore saved background if present
            PlayerSaveData saveData = SaveManager.Instance?.LoadGame();
            if (saveData != null && saveData.currentOutfit != null && !string.IsNullOrEmpty(saveData.currentOutfit.backgroundId))
            {
                SetBackground(saveData.currentOutfit.backgroundId);
            }
            else
            {
                SetBackground("room");
            }
        }

        public void SetBackground(string backgroundId)
        {
            BackgroundData bg = allBackgrounds.Find(x => x.backgroundId == backgroundId);
            if (bg != null)
            {
                currentBackground = bg;
                Sprite sprite = Resources.Load<Sprite>($"Sprites/Backgrounds/{bg.spriteKey}");

                if (backgroundRenderer != null)
                {
                    backgroundRenderer.sprite = sprite;
                }
                if (backgroundImage != null)
                {
                    backgroundImage.sprite = sprite;
                    backgroundImage.enabled = sprite != null;
                }

                PlayerSaveData saveData = SaveManager.Instance?.LoadGame();
                if (saveData != null)
                {
                    saveData.currentOutfit.backgroundId = backgroundId;
                    SaveManager.Instance.SaveGame(saveData);
                }
            }
        }

        public void AddDecoration(string decorationId)
        {
            if (allDecorations.TryGetValue(decorationId, out DecorationData dec))
            {
                if (!activeDecorations.Contains(dec))
                {
                    activeDecorations.Add(dec);
                    SpawnDecorationObject(dec);
                }
            }
        }

        public void RemoveDecoration(string decorationId)
        {
            activeDecorations.RemoveAll(x => x.decorationId == decorationId);
        }

        public BackgroundData GetCurrentBackground()
        {
            return currentBackground;
        }

        public List<DecorationData> GetAllDecorations()
        {
            return activeDecorations;
        }

        private void SpawnDecorationObject(DecorationData dec)
        {
            if (decorationContainer == null) return;
            GameObject decObj = new GameObject($"Dec_{dec.decorationId}");
            decObj.transform.SetParent(decorationContainer, false);
            decObj.transform.localPosition = dec.defaultPosition;
            decObj.transform.localScale = dec.scale;

            SpriteRenderer sr = decObj.AddComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>($"Sprites/Decorations/{dec.spriteKey}");
        }
    }
}
