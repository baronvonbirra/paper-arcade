using System;
using System.Collections.Generic;

namespace PaperDollArcade
{
    [Serializable]
    public class QuestDefinition
    {
        public string questId;
        public string title;
        public string description;
        public QuestType type;
        public int targetValue;
        public RewardData reward;
        public bool isDaily;
        public int currentProgress;
        public bool isCompleted;
    }

    [Serializable]
    public class QuestDataList
    {
        public List<QuestDefinition> quests = new List<QuestDefinition>();
    }
}
