using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PaperDollArcade
{
    public class QuestUIItem : MonoBehaviour
    {
        [SerializeField] private Text titleText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text progressText;
        [SerializeField] private Button claimButton;

        private QuestDefinition questData;

        public void Setup(QuestDefinition quest)
        {
            questData = quest;
            if (titleText != null) titleText.text = quest.title;
            if (descriptionText != null) descriptionText.text = quest.description;
            if (progressText != null) progressText.text = $"{quest.currentProgress}/{quest.targetValue}";

            if (claimButton != null)
            {
                bool canClaim = quest.currentProgress >= quest.targetValue && !quest.isCompleted;
                claimButton.interactable = canClaim;
                claimButton.onClick.RemoveAllListeners();
                claimButton.onClick.AddListener(() =>
                {
                    QuestManager.Instance?.CompleteQuest(quest.questId);
                    Setup(quest);
                });
            }
        }
    }

    public class QuestUI : MonoBehaviour
    {
        [SerializeField] private Transform listContainer;
        [SerializeField] private GameObject questItemPrefab;
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
            if (listContainer == null || questItemPrefab == null) return;

            foreach (Transform child in listContainer)
            {
                Destroy(child.gameObject);
            }

            List<QuestDefinition> quests = QuestManager.Instance?.GetActiveQuests();
            if (quests == null) return;

            foreach (var q in quests)
            {
                GameObject obj = Instantiate(questItemPrefab, listContainer);
                QuestUIItem uiItem = obj.GetComponent<QuestUIItem>();
                if (uiItem != null)
                {
                    uiItem.Setup(q);
                }
            }
        }
    }
}
