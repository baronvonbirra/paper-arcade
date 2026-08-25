using System.Collections.Generic;
using UnityEngine;

namespace PaperDollArcade
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        private List<QuestDefinition> activeQuests = new List<QuestDefinition>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadDailyQuests();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadDailyQuests()
        {
            activeQuests.Clear();
            TextAsset jsonAsset = Resources.Load<TextAsset>("Data/Quests/quests");
            if (jsonAsset != null)
            {
                QuestDataList list = JsonUtility.FromJson<QuestDataList>(jsonAsset.text);
                if (list != null && list.quests != null)
                {
                    activeQuests = list.quests;
                }
            }

            // Restore completion state from save if available
            PlayerSaveData saveData = SaveManager.Instance?.LoadGame();
            if (saveData != null && saveData.completedQuests != null)
            {
                foreach (var q in activeQuests)
                {
                    if (saveData.completedQuests.Contains(q.questId))
                    {
                        q.isCompleted = true;
                        q.currentProgress = q.targetValue;
                    }
                }
            }
        }

        public List<QuestDefinition> GetActiveQuests() => activeQuests;

        public void UpdateQuestProgress(QuestType type, int amount)
        {
            foreach (var q in activeQuests)
            {
                if (!q.isCompleted && q.type == type)
                {
                    q.currentProgress += amount;
                    if (q.currentProgress >= q.targetValue)
                    {
                        CompleteQuest(q.questId);
                    }
                }
            }
        }

        public bool CheckQuestCompletion(string questId)
        {
            QuestDefinition q = activeQuests.Find(x => x.questId == questId);
            return q != null && q.isCompleted;
        }

        public void CompleteQuest(string questId)
        {
            QuestDefinition q = activeQuests.Find(x => x.questId == questId);
            if (q != null && !q.isCompleted)
            {
                q.isCompleted = true;
                q.currentProgress = q.targetValue;

                PlayerSaveData saveData = SaveManager.Instance?.LoadGame();
                if (saveData != null && !saveData.completedQuests.Contains(questId))
                {
                    saveData.completedQuests.Add(questId);
                    SaveManager.Instance.SaveGame(saveData);
                }

                if (q.reward != null)
                {
                    RewardSystem.Instance?.ApplyReward(q.reward);
                    RewardSystem.Instance?.ShowRewardAnimation(q.reward);
                }
            }
        }

        public RewardData GetQuestReward(string questId)
        {
            QuestDefinition q = activeQuests.Find(x => x.questId == questId);
            return q?.reward;
        }
    }
}
