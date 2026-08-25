using System.Collections.Generic;
using UnityEngine;

namespace PaperDollArcade
{
    public class SpaceshipMinigame : MinigameBase
    {
        [Header("Spaceship Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float spawnInterval = 1f;
        [SerializeField] private Vector2 moveBounds = new Vector2(-8f, 8f);

        private float spawnTimer = 0f;
        private Vector3 playerPosition;

        private void Awake()
        {
            minigameId = "spaceship";
            minigameType = MinigameType.Spaceship;
            musicKey = "Minigame_Spaceship";
        }

        public override void Initialize(int difficulty)
        {
            base.Initialize(difficulty);
            playerPosition = Vector3.zero;
            spawnInterval = Mathf.Max(0.2f, 1.2f - (difficulty * 0.2f));
            moveSpeed = 4f + difficulty;
        }

        private void Update()
        {
            if (!isPlaying || isPaused) return;

            HandleInput();
            HandleSpawning();
        }

        private void HandleInput()
        {
            float horizontal = Input.GetAxis("Horizontal");
            playerPosition.x += horizontal * moveSpeed * Time.deltaTime;
            playerPosition.x = Mathf.Clamp(playerPosition.x, moveBounds.x, moveBounds.y);
            transform.position = playerPosition;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootLaser();
            }
        }

        private void ShootLaser()
        {
            AudioManager.Instance?.PlaySFX("laser");
            // Laser projectile simulation or object instantiation
        }

        private void HandleSpawning()
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            // Simulate enemy spawn
            int points = Constants.SPACESHIP_BASE_SCORE;
            AddScore(points);
        }

        public void OnEnemyDestroyed(string enemyType)
        {
            int pts = enemyType switch
            {
                "SmallMeteor" => 10,
                "BigMeteor" => 25,
                "UFO" => 50,
                _ => 10
            };
            AddScore(pts);
        }
    }
}
