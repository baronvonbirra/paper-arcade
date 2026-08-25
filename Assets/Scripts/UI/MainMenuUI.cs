using UnityEngine;
using UnityEngine.UI;

namespace PaperDollArcade
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button galleryButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            if (playButton != null) playButton.onClick.AddListener(OnPlayClicked);
            if (galleryButton != null) galleryButton.onClick.AddListener(OnGalleryClicked);
            if (settingsButton != null) settingsButton.onClick.AddListener(OnSettingsClicked);
            if (quitButton != null) quitButton.onClick.AddListener(OnQuitClicked);

            AudioManager.Instance?.PlayMusic("Menu_Background");
        }

        private void OnPlayClicked()
        {
            AudioManager.Instance?.PlaySFX("click");
            ScreenManager.Instance?.OpenScreen("Gameplay");
        }

        private void OnGalleryClicked()
        {
            AudioManager.Instance?.PlaySFX("click");
            ScreenManager.Instance?.OpenScreen("Collections");
        }

        private void OnSettingsClicked()
        {
            AudioManager.Instance?.PlaySFX("click");
            ScreenManager.Instance?.OpenScreen("Settings");
        }

        private void OnQuitClicked()
        {
            AudioManager.Instance?.PlaySFX("click");
            Application.Quit();
        }
    }
}
