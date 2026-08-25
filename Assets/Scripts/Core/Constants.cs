namespace PaperDollArcade
{
    public static class Constants
    {
        // Gameplay
        public const float CLOTHING_DRAG_SNAP_DISTANCE = 100f;
        public const float CLOTHING_ANIMATION_SPEED = 0.3f;

        // Rewards
        public const int COMMON_REWARD_WEIGHT = 50;
        public const int UNCOMMON_REWARD_WEIGHT = 30;
        public const int RARE_REWARD_WEIGHT = 15;
        public const int EPIC_REWARD_WEIGHT = 4;
        public const int LEGENDARY_REWARD_WEIGHT = 1;

        // Minigames
        public const int SPACESHIP_BASE_SCORE = 10;
        public const int TETRIS_BASE_SCORE = 100;

        // Progression
        public const int INITIAL_CLOTHING_COUNT = 20;
        public const int INITIAL_HATS_COUNT = 5;
        public const int INITIAL_ACCESSORIES_COUNT = 5;

        // Save
        public const string SAVE_FILE_NAME = "paperdoll_save.json";
    }
}
