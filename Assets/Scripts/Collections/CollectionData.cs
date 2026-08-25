using System;
using System.Collections.Generic;

namespace PaperDollArcade
{
    [Serializable]
    public class SpecialCombination
    {
        public string combinationId;
        public string name;
        public string description;
        public CollectionTheme theme;
        public List<string> requiredClothingIds = new List<string>();
        public RewardData reward;
    }

    [Serializable]
    public class Collection
    {
        public CollectionTheme theme;
        public string name;
        public string description;
        public List<ClothingItemData> items = new List<ClothingItemData>();
        public RewardData reward;
        public bool isCompleted;
    }

    [Serializable]
    public class CollectionDataList
    {
        public List<Collection> collections = new List<Collection>();
    }
}
