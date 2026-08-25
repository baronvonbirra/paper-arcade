using UnityEngine;

namespace PaperDollArcade
{
    public class DecorationItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private DecorationData decorationData;

        private void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Setup(DecorationData data)
        {
            decorationData = data;
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/Decorations/{data.spriteKey}");
            }
            transform.localPosition = data.defaultPosition;
            transform.localScale = data.scale;
        }

        public DecorationData GetData() => decorationData;
    }
}
