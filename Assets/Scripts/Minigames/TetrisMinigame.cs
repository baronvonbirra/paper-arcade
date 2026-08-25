using UnityEngine;

namespace PaperDollArcade
{
    public class TetrisMinigame : MinigameBase
    {
        [Header("Tetris Settings")]
        [SerializeField] private int gridWidth = 10;
        [SerializeField] private int gridHeight = 20;
        [SerializeField] private float dropInterval = 1.0f;

        private float dropTimer = 0f;
        private int linesClearedTotal = 0;

        private void Awake()
        {
            minigameId = "tetris";
            minigameType = MinigameType.Tetris;
            musicKey = "Minigame_Tetris";
        }

        public override void Initialize(int difficulty)
        {
            base.Initialize(difficulty);
            dropInterval = Mathf.Max(0.1f, 1.0f - ((difficulty - 1) * 0.25f));
            linesClearedTotal = 0;
        }

        private void Update()
        {
            if (!isPlaying || isPaused) return;

            HandleInput();
            HandleGravity();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                MovePiece(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                MovePiece(Vector2.right);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                RotatePiece();
            }

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                dropTimer += Time.deltaTime * 5f;
            }
        }

        private void HandleGravity()
        {
            dropTimer += Time.deltaTime;
            if (dropTimer >= dropInterval)
            {
                dropTimer = 0f;
                MovePiece(Vector2.down);
            }
        }

        private void MovePiece(Vector2 direction)
        {
            // Simulate grid piece movement
        }

        private void RotatePiece()
        {
            AudioManager.Instance?.PlaySFX("whoosh");
        }

        public void ClearLines(int linesCount)
        {
            linesClearedTotal += linesCount;
            int points = linesCount switch
            {
                1 => 100,
                2 => 300,
                3 => 500,
                4 => 800,
                _ => linesCount * 100
            };
            AddScore(points);
            AudioManager.Instance?.PlaySFX("pop");
        }
    }
}
