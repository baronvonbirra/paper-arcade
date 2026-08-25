using UnityEngine;
using UnityEngine.UI;

namespace PaperDollArcade
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private Button catalogButton;
        [SerializeField] private Button minigamesButton;
        [SerializeField] private Button collectionsButton;
        [SerializeField] private Button questsButton;
        [SerializeField] private Button settingsButton;

        [SerializeField] private GameObject catalogPanel;
        [SerializeField] private GameObject minigamesPanel;
        [SerializeField] private GameObject collectionsPanel;
        [SerializeField] private GameObject questsPanel;

        private void Start()
        {
            if (catalogButton != null) catalogButton.onClick.AddListener(OnCatalogClicked);
            if (minigamesButton != null) minigamesButton.onClick.AddListener(OnMinigamesClicked);
            if (collectionsButton != null) collectionsButton.onClick.AddListener(OnCollectionsClicked);
            if (questsButton != null) questsButton.onClick.AddListener(OnQuestsClicked);

            AudioManager.Instance?.PlayMusic("Gameplay_Default");
        }

        private void OnCatalogClicked()
        {
            AudioManager.Instance?.PlaySFX("click");
            TogglePanel(catalogPanel);
        }

        private void OnMinigamesClicked()
        {
            AudioManager.Instance?.PlaySFX("click");
            TogglePanel(minigamesPanel);
        }

        private void OnCollectionsClicked()
        {
            AudioManager.Instance?.PlaySFX("click");
            TogglePanel(collectionsPanel);
        }

        private void OnQuestsClicked()
        {
            AudioManager.Instance?.PlaySFX("click");
            TogglePanel(questsPanel);
        }

        private void TogglePanel(GameObject targetPanel)
        {
            if (targetPanel != null)
            {
                bool currentState = targetPanel.activeSelf;
                targetPanel.SetActive(!currentState);
            }
        }
    }
}
