using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class GameLogicManager : MonoBehaviour
{
    public static GameLogicManager instance;
    [SerializeField] private CMVirtualCameraController PlayerVirtualCamera;
    private EnemySpawner _enemySpawner;
    private GameUIManager _uiManager;
    private bool _enablePauseGame = true;
    private bool _enableRestartGame = false;
    private float _originTimeScale;
    private bool _isPausing = false;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        _enemySpawner = GetComponent<EnemySpawner>();
        _uiManager = GetComponent<GameUIManager>();
    }

    private void Update()
    {
        if (_enableRestartGame && Input.anyKeyDown)
        {
            _enableRestartGame = false;
            PersistentManager.instance.RestartGame();
        }

        if (_enablePauseGame && Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPausing)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    #region 全局游戏进程相关

    public void DisablePauseGame()
    {
        _enablePauseGame = false;
    }
    public void EnableRestartGame()
    {
        _enableRestartGame = true;
    }

    private void PauseGame()
    {
        AudioManager.instance.PlaySoundEffect(SoundEffectType.UIPause);
        _isPausing = true;
        DOTween.PauseAll();
        _originTimeScale = Time.timeScale;
        Time.timeScale = 0;
        _uiManager.ShowMaskPanelOnPause();
    }

    private void ResumeGame()
    {
        AudioManager.instance.PlaySoundEffect(SoundEffectType.UIResume);
        _uiManager.HideMaskPanelOnResume();
    }

    public void OnPausePanelHide()
    {
        _isPausing = false;
        DOTween.PlayAll();
        Time.timeScale = _originTimeScale;
    }

    #endregion

    #region 玩家敌人相关

    public void PlayerDeathDetected()
    {
        AudioManager.instance.StopPlayingBGM();
        DisablePauseGame();
        ShakeCamera(10f,1f,6f,3f);
        ChangeTimeScale(0f, 3f, 0.5f);
        _enemySpawner.enableSpawn = false;
        _enemySpawner.CancelAllEnemiesProvoked();
        _uiManager.ShowMaskPanelOnGameOver(_enemySpawner.totalDeadEnemyNumber);
    }

    public void PlayerEasterEggDetected()
    {
        DisablePauseGame();
        ChangeTimeScale(0f, 5.5f, 5f);
        _enemySpawner.enableSpawn = false;
        _enemySpawner.CancelAllEnemiesProvoked();
        _uiManager.ShowMaskPanelOnEasterEgg();
    }
    
    public void EnemyDeathDetected(Enemy enemy)
    {
        _enemySpawner.OnAliveEnemyDeath(enemy);
    }

    #endregion

    #region 辅助增强打击感

    public void ShakeCamera(float amplitude, float frequency, float time, float keepTime = 0f)
    {
        PlayerVirtualCamera.CameraShake(amplitude, frequency, time, keepTime);
    }

    public void ChangeTimeScale(float timeScale, float realTime, float realKeepTime = 0f)
    {
        float originTimeScale = Time.timeScale;
        if (Time.timeScale == 0 && timeScale == 0) //防止卡死的权宜之计
        {
            return;
        }
        Time.timeScale = timeScale;
        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(true);
        sequence.AppendInterval(realKeepTime);
        sequence.Append(DOTween.To(() => Time.timeScale, value => Time.timeScale = value, originTimeScale,
            realTime - realKeepTime));
    }

    #endregion

}
