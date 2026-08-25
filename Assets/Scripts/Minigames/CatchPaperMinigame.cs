using UnityEngine;

namespace PaperDollArcade
{
    public class CatchPaperMinigame : MinigameBase
    {
        [Header("Catch Paper Settings")]
        [SerializeField] private float basketSpeed = 8f;
        [SerializeField] private float fallSpeed = 3f;
        [SerializeField] private int maxLives = 3;

        private int remainingLives;
        private Vector3 basketPosition;
        private int consecutiveCatches = 0;

        private void Awake()
        {
            minigameId = "catch_paper";
            minigameType = MinigameType.CatchPaper;
            musicKey = "Minigame_Music";
        }

        public override void Initialize(int difficulty)
        {
            base.Initialize(difficulty);
            remainingLives = maxLives;
            consecutiveCatches = 0;
            fallSpeed = 2f + difficulty;
            basketPosition = Vector3.zero;
        }

        private void Update()
        {
            if (!isPlaying || isPaused) return;

            HandleBasketInput();
        }

        private void HandleBasketInput()
        {
            float horizontal = Input.GetAxis("Horizontal");
            basketPosition.x += horizontal * basketSpeed * Time.deltaTime;
            transform.position = basketPosition;
        }

        public void OnCatchPaper(string paperType)
        {
            consecutiveCatches++;
            int points = paperType == "color" ? 10 : 5;
            if (consecutiveCatches % 5 == 0)
            {
                points += 20; // Bonus
            }
            AddScore(points);
            AudioManager.Instance?.PlaySFX("pop");
        }

        public void OnMissPaper()
        {
            consecutiveCatches = 0;
            remainingLives--;
            AudioManager.Instance?.PlaySFX("defeat");

            if (remainingLives <= 0)
            {
                EndGame();
            }
        }
    }
}
