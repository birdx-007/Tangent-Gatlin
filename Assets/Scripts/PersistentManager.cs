using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        Initialize();
    }

    public Camera tempCameraForCurtain;
    public Image curtain;
    public float curtainFadeDuration = 1f;
    public string menuSceneName = "MenuScene";
    public string gameSceneName = "GameScene";

    async void Initialize()
    {
        Cursor.visible = false;
        await SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);
        tempCameraForCurtain.gameObject.SetActive(false);
        AudioManager.instance.PlayBGM(BackgroundMusicType.MainSceneBGM);
        await FadeOutCurtain();
    }

    private async UniTask FadeInCurtain()
    {
        await curtain.DOFade(1, curtainFadeDuration).AsyncWaitForCompletion().AsUniTask();
    }

    private async UniTask FadeOutCurtain()
    {
        await curtain.DOFade(0, curtainFadeDuration).AsyncWaitForCompletion().AsUniTask();
    }

    public async void StartGame()
    {
        await FadeInCurtain();
        AudioManager.instance.PlaySoundEffect(SoundEffectType.UIStart);

        await SceneManager.UnloadSceneAsync(menuSceneName);
        tempCameraForCurtain.gameObject.SetActive(true);

        if (GameLogicManager.instance == null)
        {
            AsyncOperation gameSceneHandle = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);
            gameSceneHandle.allowSceneActivation = true;
        }

        await UniTask.WaitUntil(() => GameLogicManager.instance != null);
        tempCameraForCurtain.gameObject.SetActive(false);

        AudioManager.instance.PlayBGM(BackgroundMusicType.QuietBGM);
        await FadeOutCurtain();
    }

    public async void RestartGame()
    {
        AudioManager.instance.PlaySoundEffect(SoundEffectType.UIRestart);
        await FadeInCurtain();
        
        await SceneManager.UnloadSceneAsync(gameSceneName);
        tempCameraForCurtain.gameObject.SetActive(true);

        AsyncOperation gameSceneHandle = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);
        gameSceneHandle.allowSceneActivation = true;
        
        await UniTask.WaitUntil(() => GameLogicManager.instance != null);
        tempCameraForCurtain.gameObject.SetActive(false);

        await FadeOutCurtain();
    }
}
