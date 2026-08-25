using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaperDollArcade
{
    public class RewardSystem : MonoBehaviour
    {
        public static RewardSystem Instance { get; private set; }

        private List<RewardData> rewardPool = new List<RewardData>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadRewardPool();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadRewardPool()
        {
            rewardPool.Clear();
            TextAsset jsonAsset = Resources.Load<TextAsset>("Data/Rewards/rewards");
            if (jsonAsset != null)
            {
                // Simple parsing or custom structure
            }
        }

        public RewardData GenerateRandomReward()
        {
            Rarity rarity = RollRarityByWeights(
                Constants.COMMON_REWARD_WEIGHT,
                Constants.UNCOMMON_REWARD_WEIGHT,
                Constants.RARE_REWARD_WEIGHT,
                Constants.EPIC_REWARD_WEIGHT,
                Constants.LEGENDARY_REWARD_WEIGHT
            );
            return GenerateRewardByRarity(rarity);
        }

        public RewardData GenerateRewardByScore(int score)
        {
            Rarity rarity;
            if (score <= 100)
            {
                rarity = RollRarityByWeights(70, 20, 8, 2, 0);
            }
            else if (score <= 500)
            {
                rarity = RollRarityByWeights(50, 30, 15, 4, 1);
            }
            else
            {
                rarity = RollRarityByWeights(30, 35, 25, 8, 2);
            }
            return GenerateRewardByRarity(rarity);
        }

        public Rarity RollRarityByWeights(int common, int uncommon, int rare, int epic, int legendary)
        {
            int total = common + uncommon + rare + epic + legendary;
            int roll = Random.Range(0, total);

            if (roll < common) return Rarity.Common;
            roll -= common;
            if (roll < uncommon) return Rarity.Uncommon;
            roll -= uncommon;
            if (roll < rare) return Rarity.Rare;
            roll -= rare;
            if (roll < epic) return Rarity.Epic;

            return Rarity.Legendary;
        }

        public RewardData GenerateRewardByRarity(Rarity rarity)
        {
            List<ClothingItemData> availableClothing = ClothingManager.Instance?.GetAllClothing();
            if (availableClothing != null && availableClothing.Count > 0)
            {
                List<ClothingItemData> matchingRarity = availableClothing.FindAll(c => c.rarity == rarity);
                if (matchingRarity.Count > 0)
                {
                    ClothingItemData item = matchingRarity[Random.Range(0, matchingRarity.Count)];
                    return new RewardData
                    {
                        rewardId = $"rw_{item.id}",
                        rewardName = item.name,
                        spriteKey = item.spriteKey,
                        rarity = item.rarity,
                        type = RewardType.Clothing,
                        linkedItemId = item.id
                    };
                }
            }

            return new RewardData
            {
                rewardId = "rw_default",
                rewardName = "Sample Reward",
                spriteKey = "hat_pirate",
                rarity = rarity,
                type = RewardType.Clothing,
                linkedItemId = "hat_pirate_1"
            };
        }

        public void ApplyReward(RewardData reward)
        {
            if (reward == null) return;

            PlayRewardSound();

            if (reward.type == RewardType.Clothing && !string.IsNullOrEmpty(reward.linkedItemId))
            {
                ClothingItemData item = ClothingManager.Instance?.GetClothingItemById(reward.linkedItemId);
                if (item != null)
                {
                    InventorySystem.Instance?.AddItem(item);
                }
            }
            else if (reward.type == RewardType.Background && !string.IsNullOrEmpty(reward.linkedItemId))
            {
                PlayerSaveData saveData = SaveManager.Instance?.LoadGame();
                if (saveData != null && !saveData.unlockedBackgroundIds.Contains(reward.linkedItemId))
                {
                    saveData.unlockedBackgroundIds.Add(reward.linkedItemId);
                    SaveManager.Instance?.SaveGame(saveData);
                }
            }
        }

        public void ShowRewardAnimation(RewardData reward)
        {
            if (RewardUI.Instance != null)
            {
                RewardUI.Instance.DisplayReward(reward);
            }
        }

        public void PlayRewardSound()
        {
            AudioManager.Instance?.PlaySFX("reward_open");
            AudioManager.Instance?.PlaySFX("success_chime");
        }
    }
}
