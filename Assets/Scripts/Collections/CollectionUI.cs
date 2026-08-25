using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PaperDollArcade
{
    public class CollectionUIItem : MonoBehaviour
    {
        [SerializeField] private Text titleText;
        [SerializeField] private Text progressText;
        [SerializeField] private Image progressBar;
        [SerializeField] private Button claimRewardButton;

        private Collection collectionData;

        public void Setup(Collection collection)
        {
            collectionData = collection;
            if (titleText != null) titleText.text = collection.name;

            float pct = CollectionManager.Instance != null ? CollectionManager.Instance.GetCompletionPercentage(collection.theme) : 0f;
            if (progressText != null) progressText.text = $"{pct:F0}%";
            if (progressBar != null) progressBar.fillAmount = pct / 100f;

            if (claimRewardButton != null)
            {
                claimRewardButton.interactable = pct >= 100f && !collection.isCompleted;
                claimRewardButton.onClick.RemoveAllListeners();
                claimRewardButton.onClick.AddListener(() =>
                {
                    CollectionManager.Instance?.UnlockCollectionReward(collection.theme);
                    Setup(collection);
                });
            }
        }
    }

    public class CollectionUI : MonoBehaviour
    {
        [SerializeField] private Transform listContainer;
        [SerializeField] private GameObject collectionItemPrefab;
        [SerializeField] private Button closeButton;

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

        public void RefreshUI()
        {
            if (listContainer == null || collectionItemPrefab == null) return;

            foreach (Transform child in listContainer)
            {
                Destroy(child.gameObject);
            }

            List<Collection> collections = CollectionManager.Instance?.GetAllCollections();
            if (collections == null) return;

            foreach (var col in collections)
            {
                GameObject obj = Instantiate(collectionItemPrefab, listContainer);
                CollectionUIItem uiItem = obj.GetComponent<CollectionUIItem>();
                if (uiItem != null)
                {
                    uiItem.Setup(col);
                }
            }
        }
    }
}
