using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PaperDollArcade
{
    public class InventoryItemUI : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private Text itemNameText;
        [SerializeField] private Text itemRarityText;
        [SerializeField] private Button actionButton;

        private ClothingItemData currentData;

        public void Setup(ClothingItemData data)
        {
            currentData = data;
            if (itemNameText != null) itemNameText.text = data.name;
            if (itemRarityText != null) itemRarityText.text = data.rarity.ToString();

            if (itemIcon != null)
            {
                Sprite sprite = Resources.Load<Sprite>($"Sprites/Clothing/{data.spriteKey}");
                if (sprite != null) itemIcon.sprite = sprite;
            }

            if (actionButton != null)
            {
                actionButton.onClick.RemoveAllListeners();
                actionButton.onClick.AddListener(() =>
                {
                    ClothingManager.Instance?.ApplyClothingToCharacter(currentData);
                });
            }
        }
    }

    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Transform gridContainer;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Button closeButton;

        private ClothingType activeFilterType = ClothingType.Hat;
        private bool isTypeFilterActive = false;

        private void OnEnable()
        {
            RefreshUI();
        }

        private void Start()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(() => gameObject.SetActive(false));
            }
        }

        public void FilterByCategory(ClothingType category)
        {
            activeFilterType = category;
            isTypeFilterActive = true;
            RefreshUI();
        }

        public void ShowAllCategories()
        {
            isTypeFilterActive = false;
            RefreshUI();
        }

        public void RefreshUI()
        {
            if (gridContainer == null || itemPrefab == null) return;

            foreach (Transform child in gridContainer)
            {
                Destroy(child.gameObject);
            }

            List<ClothingItemData> items = isTypeFilterActive
                ? InventorySystem.Instance?.GetItemsByType(activeFilterType)
                : InventorySystem.Instance?.GetAllItems();

            if (items == null) return;

            foreach (var item in items)
            {
                GameObject obj = Instantiate(itemPrefab, gridContainer);
                InventoryItemUI uiItem = obj.GetComponent<InventoryItemUI>();
                if (uiItem != null)
                {
                    uiItem.Setup(item);
                }
                ClothingItem dragItem = obj.GetComponent<ClothingItem>();
                if (dragItem != null)
                {
                    dragItem.Setup(item);
                }
            }
        }
    }
}
