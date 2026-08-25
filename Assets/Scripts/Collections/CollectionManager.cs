using System.Collections.Generic;
using UnityEngine;

namespace PaperDollArcade
{
    public class CollectionManager : MonoBehaviour
    {
        public static CollectionManager Instance { get; private set; }

        private List<Collection> allCollections = new List<Collection>();
        private List<SpecialCombination> specialCombinations = new List<SpecialCombination>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadAllCollections();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadAllCollections()
        {
            allCollections.Clear();
            TextAsset jsonAsset = Resources.Load<TextAsset>("Data/Collections/collections");
            if (jsonAsset != null)
            {
                CollectionDataList list = JsonUtility.FromJson<CollectionDataList>(jsonAsset.text);
                if (list != null && list.collections != null)
                {
                    allCollections = list.collections;
                }
            }
        }

        public Collection GetCollectionByTheme(CollectionTheme theme)
        {
            return allCollections.Find(c => c.theme == theme);
        }

        public bool CheckCollectionCompletion(CollectionTheme theme)
        {
            Collection col = GetCollectionByTheme(theme);
            if (col == null || col.items == null || col.items.Count == 0) return false;

            foreach (var item in col.items)
            {
                if (InventorySystem.Instance != null && !InventorySystem.Instance.HasItem(item.id))
                {
                    return false;
                }
            }
            return true;
        }

        public float GetCompletionPercentage(CollectionTheme theme)
        {
            Collection col = GetCollectionByTheme(theme);
            if (col == null || col.items == null || col.items.Count == 0) return 0f;

            int count = 0;
            foreach (var item in col.items)
            {
                if (InventorySystem.Instance != null && InventorySystem.Instance.HasItem(item.id))
                {
                    count++;
                }
            }
            return (float)count / col.items.Count * 100f;
        }

        public SpecialCombination DetectSpecialCombination(CurrentOutfit outfit)
        {
            if (outfit == null) return null;

            List<string> currentEquippedIds = new List<string>();
            if (!string.IsNullOrEmpty(outfit.hatId)) currentEquippedIds.Add(outfit.hatId);
            if (!string.IsNullOrEmpty(outfit.headId)) currentEquippedIds.Add(outfit.headId);
            if (!string.IsNullOrEmpty(outfit.glassesId)) currentEquippedIds.Add(outfit.glassesId);
            if (!string.IsNullOrEmpty(outfit.suitId)) currentEquippedIds.Add(outfit.suitId);
            if (!string.IsNullOrEmpty(outfit.jacketId)) currentEquippedIds.Add(outfit.jacketId);
            if (!string.IsNullOrEmpty(outfit.capeId)) currentEquippedIds.Add(outfit.capeId);
            if (outfit.accessoryIds != null) currentEquippedIds.AddRange(outfit.accessoryIds);

            foreach (var combo in specialCombinations)
            {
                bool match = true;
                foreach (var reqId in combo.requiredClothingIds)
                {
                    if (!currentEquippedIds.Contains(reqId))
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return combo;
            }
            return null;
        }

        public void UnlockCollectionReward(CollectionTheme theme)
        {
            Collection col = GetCollectionByTheme(theme);
            if (col != null && col.reward != null)
            {
                RewardSystem.Instance?.ApplyReward(col.reward);
                col.isCompleted = true;
            }
        }

        public List<Collection> GetAllCollections() => allCollections;
    }
}
