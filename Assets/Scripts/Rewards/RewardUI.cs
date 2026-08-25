using UnityEngine;
using UnityEngine.UI;

namespace PaperDollArcade
{
    public class RewardUI : MonoBehaviour
    {
        public static RewardUI Instance { get; private set; }

        [SerializeField] private GameObject popupPanel;
        [SerializeField] private Image rewardImage;
        [SerializeField] private Text rewardNameText;
        [SerializeField] private Text rarityText;
        [SerializeField] private Button claimButton;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (claimButton != null)
            {
                claimButton.onClick.AddListener(HideReward);
            }
            if (popupPanel != null)
            {
                popupPanel.SetActive(false);
            }
        }

        public void DisplayReward(RewardData reward)
        {
            if (reward == null) return;

            if (rewardNameText != null) rewardNameText.text = reward.rewardName;
            if (rarityText != null) rarityText.text = reward.rarity.ToString();

            if (rewardImage != null)
            {
                Sprite sprite = Resources.Load<Sprite>($"Sprites/Clothing/{reward.spriteKey}");
                if (sprite == null) sprite = Resources.Load<Sprite>($"Sprites/{reward.spriteKey}");
                rewardImage.sprite = sprite;
                rewardImage.enabled = sprite != null;
            }

            if (popupPanel != null)
            {
                popupPanel.SetActive(true);
            }
        }

        public void HideReward()
        {
            if (popupPanel != null)
            {
                popupPanel.SetActive(false);
            }
        }
    }
}
