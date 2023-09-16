using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        commonBackgroundMusicAudioSource = transform.Find("BGMSource").GetComponent<AudioSource>();
        if (!commonBackgroundMusicAudioSource)
        {
            Debug.LogError("Common Audio Source NOT Found.");
        }
        commonSoundEffectAudioSource = transform.Find("SoundEffectSource").GetComponent<AudioSource>();
        if (!commonSoundEffectAudioSource)
        {
            Debug.LogError("Common Audio Source NOT Found.");
        }
    }
    public AudioCategory audioCategory;
    [Range(0f, 1f)] public float backgroundMusicVolume = 1f;
    [Range(0f, 1f)] public float soundEffectVolume = 1f;
    [HideInInspector] public AudioSource commonBackgroundMusicAudioSource;
    [HideInInspector] public AudioSource commonSoundEffectAudioSource;

    public void PlaySoundEffect(AudioSource source,SoundEffectType type)
    {
        if (source)
        {
            if (audioCategory.soundEffects.TryGetValue(type, out AudioClip clip))
            {
                source.volume = soundEffectVolume;
                source.PlayOneShot(clip);
            }
        }
    }

    public void PlaySoundEffect(SoundEffectType type)
    {
        if (audioCategory.soundEffects.TryGetValue(type, out AudioClip clip))
        {
            commonSoundEffectAudioSource.volume = soundEffectVolume;
            commonSoundEffectAudioSource.PlayOneShot(clip);
        }
    }

    public void PlayBGM(BackgroundMusicType type)
    {
        if (audioCategory.backgroundMusics.TryGetValue(type, out AudioClip clip))
        {
            commonBackgroundMusicAudioSource.volume = backgroundMusicVolume;
            commonBackgroundMusicAudioSource.clip = clip;
            commonBackgroundMusicAudioSource.Play();
        }
    }

    public void StopPlayingBGM()
    {
        if (commonBackgroundMusicAudioSource.isPlaying)
        {
            commonBackgroundMusicAudioSource.Stop();
        }
    }
}
