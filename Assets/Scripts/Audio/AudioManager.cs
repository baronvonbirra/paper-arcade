using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaperDollArcade
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> musicClips = new Dictionary<string, AudioClip>();

        private float masterVolume = 1f;
        private float musicVolume = 0.7f;
        private float sfxVolume = 0.8f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudioSources();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeAudioSources()
        {
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
            }

            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
            }

            LoadAudioResources();
        }

        private void LoadAudioResources()
        {
            AudioClip[] sfxList = Resources.LoadAll<AudioClip>("Audio/SFX");
            foreach (var clip in sfxList)
            {
                sfxClips[clip.name] = clip;
            }

            AudioClip[] musicList = Resources.LoadAll<AudioClip>("Audio/Music");
            foreach (var clip in musicList)
            {
                musicClips[clip.name] = clip;
            }
        }

        public void PlaySFX(string soundKey, float volume = 1f)
        {
            if (sfxClips.TryGetValue(soundKey, out AudioClip clip))
            {
                sfxSource.PlayOneShot(clip, volume * sfxVolume * masterVolume);
            }
            else
            {
                AudioClip loadedClip = Resources.Load<AudioClip>($"Audio/SFX/{soundKey}");
                if (loadedClip != null)
                {
                    sfxClips[soundKey] = loadedClip;
                    sfxSource.PlayOneShot(loadedClip, volume * sfxVolume * masterVolume);
                }
                else
                {
                    Debug.LogWarning($"SFX clip '{soundKey}' not found.");
                }
            }
        }

        public void PlayMusic(string musicKey, float fadeDuration = 1f)
        {
            AudioClip clipToPlay = null;
            if (!musicClips.TryGetValue(musicKey, out clipToPlay))
            {
                clipToPlay = Resources.Load<AudioClip>($"Audio/Music/{musicKey}");
                if (clipToPlay != null)
                {
                    musicClips[musicKey] = clipToPlay;
                }
            }

            if (clipToPlay != null)
            {
                StartCoroutine(FadeMusicCoroutine(clipToPlay, fadeDuration));
            }
            else
            {
                Debug.LogWarning($"Music clip '{musicKey}' not found.");
            }
        }

        public void StopMusic(float fadeDuration = 1f)
        {
            StartCoroutine(FadeOutMusicCoroutine(fadeDuration));
        }

        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        private void UpdateVolumes()
        {
            musicSource.volume = musicVolume * masterVolume;
        }

        private IEnumerator FadeMusicCoroutine(AudioClip newClip, float duration)
        {
            if (musicSource.isPlaying && duration > 0f)
            {
                float startVol = musicSource.volume;
                for (float t = 0; t < duration; t += Time.deltaTime)
                {
                    musicSource.volume = Mathf.Lerp(startVol, 0f, t / duration);
                    yield return null;
                }
            }

            musicSource.clip = newClip;
            musicSource.Play();

            float targetVol = musicVolume * masterVolume;
            if (duration > 0f)
            {
                for (float t = 0; t < duration; t += Time.deltaTime)
                {
                    musicSource.volume = Mathf.Lerp(0f, targetVol, t / duration);
                    yield return null;
                }
            }
            musicSource.volume = targetVol;
        }

        private IEnumerator FadeOutMusicCoroutine(float duration)
        {
            if (musicSource.isPlaying && duration > 0f)
            {
                float startVol = musicSource.volume;
                for (float t = 0; t < duration; t += Time.deltaTime)
                {
                    musicSource.volume = Mathf.Lerp(startVol, 0f, t / duration);
                    yield return null;
                }
            }
            musicSource.Stop();
        }
    }
}
