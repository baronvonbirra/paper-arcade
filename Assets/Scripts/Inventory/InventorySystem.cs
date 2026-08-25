using System.Collections.Generic;
using UnityEngine;

namespace PaperDollArcade
{
    public class InventoryItem
    {
        public ClothingItemData itemData;
        public bool isEquipped;

        public InventoryItem(ClothingItemData data)
        {
            itemData = data;
            isEquipped = false;
        }
    }

    public class InventorySystem : MonoBehaviour
    {
        public static InventorySystem Instance { get; private set; }

        private List<string> unlockedItemIds = new List<string>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadInventory();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadInventory()
        {
            PlayerSaveData saveData = SaveManager.Instance.LoadGame();
            if (saveData != null && saveData.unlockedClothingIds != null)
            {
                unlockedItemIds = saveData.unlockedClothingIds;
            }
        }

        public void AddItem(ClothingItemData item)
        {
            if (item == null) return;
            if (!HasItem(item.id))
            {
                unlockedItemIds.Add(item.id);
                PlayerSaveData saveData = SaveManager.Instance.LoadGame();
                if (!saveData.unlockedClothingIds.Contains(item.id))
                {
                    saveData.unlockedClothingIds.Add(item.id);
                    SaveManager.Instance.SaveGame(saveData);
                }
            }
        }

        public void RemoveItem(string itemId)
        {
            if (HasItem(itemId))
            {
                unlockedItemIds.Remove(itemId);
                PlayerSaveData saveData = SaveManager.Instance.LoadGame();
                saveData.unlockedClothingIds.Remove(itemId);
                SaveManager.Instance.SaveGame(saveData);
            }
        }

        public bool HasItem(string itemId)
        {
            return unlockedItemIds.Contains(itemId);
        }

        public List<ClothingItemData> GetAllItems()
        {
            List<ClothingItemData> result = new List<ClothingItemData>();
            foreach (var id in unlockedItemIds)
            {
                ClothingItemData item = ClothingManager.Instance?.GetClothingItemById(id);
                if (item != null)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public List<ClothingItemData> GetItemsByType(ClothingType type)
        {
            return GetAllItems().FindAll(item => item.type == type);
        }

        public List<ClothingItemData> GetItemsByRarity(Rarity rarity)
        {
            return GetAllItems().FindAll(item => item.rarity == rarity);
        }

        public List<ClothingItemData> GetItemsByTheme(CollectionTheme theme)
        {
            return GetAllItems().FindAll(item => item.theme == theme);
        }
    }
}
