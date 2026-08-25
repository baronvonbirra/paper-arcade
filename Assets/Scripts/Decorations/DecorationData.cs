using System;
using UnityEngine;

namespace PaperDollArcade
{
    [Serializable]
    public class BackgroundData
    {
        public string backgroundId;
        public string name;
        public BackgroundType type;
        public string spriteKey;
        public bool isUnlocked;
    }

    [Serializable]
    public class DecorationData
    {
        public string decorationId;
        public string name;
        public string spriteKey;
        public Vector2 defaultPosition;
        public Vector2 scale = Vector2.one;
        public bool isUnlocked;
    }
}
