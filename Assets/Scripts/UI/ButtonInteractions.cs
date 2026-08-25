using UnityEngine;
using UnityEngine.EventSystems;

namespace PaperDollArcade
{
    public class ButtonInteractions : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] private string hoverSoundKey = "click";
        [SerializeField] private string clickSoundKey = "pop";
        [SerializeField] private Vector3 hoverScaleMultiplier = new Vector3(1.05f, 1.05f, 1.05f);

        private Vector3 defaultScale;

        private void Awake()
        {
            defaultScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!string.IsNullOrEmpty(hoverSoundKey))
            {
                AudioManager.Instance?.PlaySFX(hoverSoundKey);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!string.IsNullOrEmpty(clickSoundKey))
            {
                AudioManager.Instance?.PlaySFX(clickSoundKey);
            }
        }
    }
}
