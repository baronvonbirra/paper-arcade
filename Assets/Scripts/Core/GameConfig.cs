using UnityEngine;

namespace PaperDollArcade
{
    public class GameConfig : MonoBehaviour
    {
        [Header("Audio Settings")]
        [Range(0f, 1f)] public float masterVolume = 1f;
        [Range(0f, 1f)] public float musicVolume = 0.7f;
        [Range(0f, 1f)] public float sfxVolume = 0.8f;

        [Header("Gameplay Settings")]
        public float clothingSnapDistance = Constants.CLOTHING_DRAG_SNAP_DISTANCE;
        public float clothingAnimationSpeed = Constants.CLOTHING_ANIMATION_SPEED;

        [Header("Reward Weight Settings")]
        public int commonRewardWeight = Constants.COMMON_REWARD_WEIGHT;
        public int uncommonRewardWeight = Constants.UNCOMMON_REWARD_WEIGHT;
        public int rareRewardWeight = Constants.RARE_REWARD_WEIGHT;
        public int epicRewardWeight = Constants.EPIC_REWARD_WEIGHT;
        public int legendaryRewardWeight = Constants.LEGENDARY_REWARD_WEIGHT;

        [Header("Debug & Minigames")]
        public int minigameDifficulty = 1;
        public bool debugMode = false;

        public static GameConfig Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
