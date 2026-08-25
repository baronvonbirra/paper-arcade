using UnityEngine;

namespace PaperDollArcade
{
    public abstract class MinigameBase : MonoBehaviour
    {
        [Header("Minigame Base Settings")]
        [SerializeField] protected string minigameId;
        [SerializeField] protected MinigameType minigameType;
        [SerializeField] protected string musicKey = "Minigame_Music";

        protected int currentScore = 0;
        protected int difficultyLevel = 1;
        protected bool isPlaying = false;
        protected bool isPaused = false;

        public virtual void Initialize(int difficulty)
        {
            difficultyLevel = difficulty;
            currentScore = 0;
            isPlaying = false;
            isPaused = false;
        }

        public virtual void StartGame()
        {
            isPlaying = true;
            isPaused = false;
            currentScore = 0;
            PlayGameMusic();
        }

        public virtual void EndGame()
        {
            isPlaying = false;
            AudioManager.Instance?.StopMusic(0.5f);

            // Record high score
            PlayerSaveData saveData = SaveManager.Instance?.LoadGame();
            if (saveData != null)
            {
                saveData.totalMinigamesPlayed++;
                saveData.SetHighScore(minigameId, currentScore);
                SaveManager.Instance.SaveGame(saveData);
            }

            // Grant reward
            RewardData reward = GetReward();
            if (reward != null)
            {
                RewardSystem.Instance?.ApplyReward(reward);
                RewardSystem.Instance?.ShowRewardAnimation(reward);
            }

            QuestManager.Instance?.UpdateQuestProgress(QuestType.PlayMinigame, 1);
        }

        public virtual void AddScore(int points)
        {
            if (!isPlaying || isPaused) return;
            currentScore += points;
        }

        public virtual int GetFinalScore() => currentScore;

        public virtual RewardData GetReward()
        {
            return RewardSystem.Instance?.GenerateRewardByScore(currentScore) ?? RewardSystem.Instance?.GenerateRandomReward();
        }

        public virtual void PlayGameMusic()
        {
            if (!string.IsNullOrEmpty(musicKey))
            {
                AudioManager.Instance?.PlayMusic(musicKey);
            }
        }

        public virtual void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0f;
        }

        public virtual void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f;
        }
    }
}
