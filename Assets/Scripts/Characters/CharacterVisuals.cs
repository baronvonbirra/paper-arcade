using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PaperDollArcade
{
    public class CharacterVisuals : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer baseBodyRenderer;
        [SerializeField] private SpriteRenderer hatRenderer;
        [SerializeField] private SpriteRenderer headRenderer;
        [SerializeField] private SpriteRenderer glassesRenderer;
        [SerializeField] private SpriteRenderer suitRenderer;
        [SerializeField] private SpriteRenderer jacketRenderer;
        [SerializeField] private SpriteRenderer capeRenderer;
        [SerializeField] private SpriteRenderer accessoryRenderer;

        // UI Image support if in Canvas UI
        [SerializeField] private Image baseBodyImage;
        [SerializeField] private Image hatImage;
        [SerializeField] private Image headImage;
        [SerializeField] private Image glassesImage;
        [SerializeField] private Image suitImage;
        [SerializeField] private Image jacketImage;
        [SerializeField] private Image capeImage;
        [SerializeField] private Image accessoryImage;

        private Dictionary<ClothingType, SpriteRenderer> renderers = new Dictionary<ClothingType, SpriteRenderer>();
        private Dictionary<ClothingType, Image> uiImages = new Dictionary<ClothingType, Image>();

        private void Awake()
        {
            RegisterRenderers();
            RegisterUIImages();
        }

        private void RegisterRenderers()
        {
            if (hatRenderer != null) renderers[ClothingType.Hat] = hatRenderer;
            if (headRenderer != null) renderers[ClothingType.Head] = headRenderer;
            if (glassesRenderer != null) renderers[ClothingType.Glasses] = glassesRenderer;
            if (suitRenderer != null) renderers[ClothingType.Suit] = suitRenderer;
            if (jacketRenderer != null) renderers[ClothingType.Jacket] = jacketRenderer;
            if (capeRenderer != null) renderers[ClothingType.Cape] = capeRenderer;
            if (accessoryRenderer != null) renderers[ClothingType.Accessory] = accessoryRenderer;
        }

        private void RegisterUIImages()
        {
            if (hatImage != null) uiImages[ClothingType.Hat] = hatImage;
            if (headImage != null) uiImages[ClothingType.Head] = headImage;
            if (glassesImage != null) uiImages[ClothingType.Glasses] = glassesImage;
            if (suitImage != null) uiImages[ClothingType.Suit] = suitImage;
            if (jacketImage != null) uiImages[ClothingType.Jacket] = jacketImage;
            if (capeImage != null) uiImages[ClothingType.Cape] = capeImage;
            if (accessoryImage != null) uiImages[ClothingType.Accessory] = accessoryImage;
        }

        public void InitializeVisuals()
        {
            // Clear or reset all slots
            foreach (var kvp in renderers)
            {
                if (kvp.Value != null) kvp.Value.sprite = null;
            }
            foreach (var kvp in uiImages)
            {
                if (kvp.Value != null) kvp.Value.enabled = false;
            }
        }

        public void ApplyClothingVisual(ClothingType type, ClothingItemData itemData)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprites/Clothing/{itemData.spriteKey}");
            if (sprite == null)
            {
                sprite = Resources.Load<Sprite>($"Sprites/{itemData.spriteKey}");
            }

            if (renderers.TryGetValue(type, out SpriteRenderer renderer) && renderer != null)
            {
                renderer.sprite = sprite;
                renderer.transform.localPosition = itemData.positionOffset;
                renderer.transform.localScale = itemData.scale;
                if (itemData.sortingOrder != 0) renderer.sortingOrder = itemData.sortingOrder;
            }

            if (uiImages.TryGetValue(type, out Image img) && img != null)
            {
                img.sprite = sprite;
                img.enabled = sprite != null;
                img.transform.localPosition = itemData.positionOffset;
                img.transform.localScale = itemData.scale;
            }
        }

        public void RemoveClothingVisual(ClothingType type)
        {
            if (renderers.TryGetValue(type, out SpriteRenderer renderer) && renderer != null)
            {
                renderer.sprite = null;
            }

            if (uiImages.TryGetValue(type, out Image img) && img != null)
            {
                img.sprite = null;
                img.enabled = false;
            }
        }
    }
}
