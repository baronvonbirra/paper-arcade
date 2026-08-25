using System;

namespace PaperDollArcade
{
    [Serializable]
    public class RewardData
    {
        public string rewardId;
        public string rewardName;
        public string spriteKey;
        public Rarity rarity;
        public RewardType type;
        public string linkedItemId;
    }
}
