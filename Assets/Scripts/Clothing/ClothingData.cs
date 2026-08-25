using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaperDollArcade
{
    [Serializable]
    public class ClothingItemData
    {
        public string id;
        public string name;
        public string description;
        public ClothingType type;
        public Rarity rarity;
        public string spriteKey;
        public Vector2 positionOffset;
        public Vector2 scale = Vector2.one;
        public int sortingOrder;
        public bool isUnlocked;
        public CollectionTheme theme;
        public string[] specialTags;
    }

    [Serializable]
    public class ClothingCollectionData
    {
        public List<ClothingItemData> items = new List<ClothingItemData>();
    }
}
