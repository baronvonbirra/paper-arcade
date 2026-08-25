using System;
using UnityEngine;

namespace PaperDollArcade
{
    [Serializable]
    public class SoundData
    {
        public string key;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop;
    }
}
