using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PaperDollArcade
{
    public class ClothingItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private Image iconImage;
        private ClothingItemData itemData;
        private RectTransform rectTransform;
        private Canvas canvas;
        private Vector2 originalPosition;
        private Transform originalParent;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            if (iconImage == null) iconImage = GetComponent<Image>();
        }

        public void Setup(ClothingItemData data)
        {
            itemData = data;
            if (iconImage != null)
            {
                Sprite sprite = Resources.Load<Sprite>($"Sprites/Clothing/{data.spriteKey}");
                if (sprite != null)
                {
                    iconImage.sprite = sprite;
                }
            }
        }

        public ClothingItemData GetData() => itemData;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemData == null) return;
            originalPosition = rectTransform.anchoredPosition;
            originalParent = transform.parent;

            if (canvas != null)
            {
                transform.SetParent(canvas.transform, true);
            }
            AudioManager.Instance?.PlaySFX("whoosh");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (itemData == null) return;
            if (canvas != null)
            {
                rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            }
            else
            {
                transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (itemData == null) return;

            bool placed = ClothingManager.Instance != null && ClothingManager.Instance.DetectClothingPlacement(itemData, transform.position);

            if (!placed)
            {
                transform.SetParent(originalParent, true);
                rectTransform.anchoredPosition = originalPosition;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemData == null) return;
            ClothingManager.Instance?.ApplyClothingToCharacter(itemData);
        }
    }
}
