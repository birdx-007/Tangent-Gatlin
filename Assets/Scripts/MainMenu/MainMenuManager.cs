using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private bool _hasGameStarted = false;
    public MaskableGraphic menuPanel;
    private List<MaskableGraphic> mainPanelChildren;
    private Tweener menuPanelTweener;
    private void Awake()
    {
        Cursor.visible = false;
        //menuPanel.gameObject.SetActive(true);
        mainPanelChildren = menuPanel.transform.GetComponentsInChildren<MaskableGraphic>().ToList();
        //mainPanelChildren.RemoveAt(0);
        foreach (var graphic in mainPanelChildren)
        {
            graphic.gameObject.SetActive(true);
        }
        SetMenuPanelAnimation();
    }

    private void Update()
    {
        if (!_hasGameStarted && Input.anyKeyDown)
        {
            _hasGameStarted = true;
            StartGame();
        }
    }

    private void SetMenuPanelAnimation()
    {
        menuPanelTweener = menuPanel.transform.DOLocalMoveY(-30, 5);
        menuPanelTweener.SetLoops(-1, LoopType.Yoyo);
    }
    
    private async UniTask HideMenu()
    {
        float hideTime = 1.5f;
        foreach (var graphic in mainPanelChildren)
        {
            graphic.DOColor(Color.clear, hideTime);
        }
        await UniTask.WaitUntil(() => menuPanel.color == Color.clear);
        menuPanelTweener.Kill();
    }
    public async void StartGame()
    {
        await HideMenu();
        await new WaitForSecondsRealtime(0.1f);
        Camera.main.transform.DOMoveY(7.5f, PersistentManager.instance.curtainFadeDuration * 0.95f).SetEase(Ease.InOutSine);
        PersistentManager.instance.StartGame();
    }
}
