using System;
using System.Collections.Generic;

namespace PaperDollArcade
{
    [Serializable]
    public class CurrentOutfit
    {
        public string hatId;
        public string headId;
        public string glassesId;
        public string suitId;
        public string jacketId;
        public string capeId;
        public List<string> accessoryIds = new List<string>();
        public string backgroundId;
        public List<string> decorationIds = new List<string>();
    }

    [Serializable]
    public class PlayerSaveData
    {
        public List<string> unlockedClothingIds = new List<string>();
        public List<string> unlockedBackgroundIds = new List<string>();
        public List<string> unlockedDecorationIds = new List<string>();
        public List<MinigameHighScoreEntry> minigameHighScores = new List<MinigameHighScoreEntry>();
        public int totalPlayTime;
        public int totalMinigamesPlayed;
        public int totalCollectionsCompleted;
        public CurrentOutfit currentOutfit = new CurrentOutfit();
        public List<string> completedQuests = new List<string>();

        public int GetHighScore(string minigameId)
        {
            var entry = minigameHighScores.Find(x => x.minigameId == minigameId);
            return entry != null ? entry.highScore : 0;
        }

        public void SetHighScore(string minigameId, int score)
        {
            var entry = minigameHighScores.Find(x => x.minigameId == minigameId);
            if (entry != null)
            {
                if (score > entry.highScore) entry.highScore = score;
            }
            else
            {
                minigameHighScores.Add(new MinigameHighScoreEntry { minigameId = minigameId, highScore = score });
            }
        }
    }

    [Serializable]
    public class MinigameHighScoreEntry
    {
        public string minigameId;
        public int highScore;
    }
}
