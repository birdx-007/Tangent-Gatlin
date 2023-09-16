using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEffectType
{
    UIStart,
    UIPause,
    UIResume,
    UIRestart,
    PlayerShoot,
    PlayerShootHit,
    EnemyShoot,
    EnemyShootHit,
    PlayerTurnAround
}

public enum BackgroundMusicType
{
    MainSceneBGM,
    QuietBGM,
    TenseBGM
}

[CreateAssetMenu(fileName = "AudioCategory",menuName = "CategoryData/AudioCategory")]
public class AudioCategory : ScriptableObject
{
    public SerializableDictionary<SoundEffectType, AudioClip> soundEffects;
    public SerializableDictionary<BackgroundMusicType, AudioClip> backgroundMusics;
}
