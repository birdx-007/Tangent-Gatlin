using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public MaskableGraphic pausePanel;
    private List<MaskableGraphic> _pausePanelChildren;
    public MaskableGraphic gameOverPanel;
    private List<MaskableGraphic> _gameOverPanelChildren;
    public MaskableGraphic easterEggPanel;
    private List<MaskableGraphic> _easterEggPanelChildren;

    private void Awake()
    {
        //pausePanel
        pausePanel.gameObject.SetActive(true);
        pausePanel.raycastTarget = false;
        _pausePanelChildren = pausePanel.transform.GetComponentsInChildren<MaskableGraphic>().ToList();
        _pausePanelChildren.RemoveAt(0);
        foreach (var graphic in _pausePanelChildren)
        {
            graphic.gameObject.SetActive(false);
            graphic.raycastTarget = false;
        }
        //gameOverPanel
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.raycastTarget = false;
        gameOverPanel.color = Color.clear;
        _gameOverPanelChildren = gameOverPanel.transform.GetComponentsInChildren<MaskableGraphic>().ToList();
        _gameOverPanelChildren.RemoveAt(0);
        foreach (var graphic in _gameOverPanelChildren)
        {
            graphic.gameObject.SetActive(true);
            graphic.raycastTarget = false;
            graphic.color = Color.clear;
        }
        //easterEggPanel
        easterEggPanel.gameObject.SetActive(true);
        easterEggPanel.raycastTarget = false;
        easterEggPanel.rectTransform.pivot = new Vector2(0.5f, 0);
        easterEggPanel.rectTransform.localScale = new Vector3(0, 0, 0);
        _easterEggPanelChildren = easterEggPanel.transform.GetComponentsInChildren<MaskableGraphic>().ToList();
        _easterEggPanelChildren.RemoveAt(0);
        foreach (var graphic in _easterEggPanelChildren)
        {
            graphic.gameObject.SetActive(false);
            graphic.raycastTarget = false;
        }
    }

    public void ShowMaskPanelOnPause()
    {
        pausePanel.raycastTarget = true;
        foreach (var graphic in _pausePanelChildren)
        {
            graphic.gameObject.SetActive(true);
            if (graphic is Image operationTip)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.SetUpdate(true);
                sequence.Append(graphic.rectTransform.DOAnchorPosY(-710, 1f));
                sequence.Join(graphic.rectTransform.DOLocalRotate(new Vector3(0, 0, 8f), 1f));
            }
            else
            {
                graphic.DOColor(Color.white, 1f).SetUpdate(true);
            }
        }
    }

    public void HideMaskPanelOnResume()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(true);
        foreach (var graphic in _pausePanelChildren)
        {
            if (graphic is Image operationTip)
            {
                sequence.Join(graphic.rectTransform.DOAnchorPosY(-1500, 1f));
                sequence.Join(graphic.rectTransform.DOLocalRotate(new Vector3(0, 0, 40f), 1f));
            }
            else
            {
                sequence.Join(graphic.DOColor(Color.clear, 1f).OnComplete(() => graphic.gameObject.SetActive(false)));
            }
        }
        sequence.OnComplete(() =>
        {
            pausePanel.raycastTarget = false;
            GameLogicManager.instance.OnPausePanelHide();
        });
    }

    public void ShowMaskPanelOnGameOver(int totalKillNumber)
    {
        gameOverPanel.raycastTarget = true;
        gameOverPanel.DOColor(Color.white, 3f)
            .OnComplete(() =>
                {
                    AudioManager.instance.PlayBGM(BackgroundMusicType.MainSceneBGM);
                    Sequence sequence = DOTween.Sequence();
                    foreach (var graphic in _gameOverPanelChildren)
                    {
                        if (graphic is Text text && graphic.gameObject.CompareTag("UINumberCounter"))
                        {
                            sequence.Append(graphic.DOColor(Color.black, 1f));
                            int counter = 0;
                            sequence.Append(DOTween.To(() => counter, value => counter = value, totalKillNumber, 1.5f)
                                .SetEase(Ease.OutQuad).OnUpdate(() => text.text = counter.ToString()));
                            sequence.AppendInterval(0.5f);
                        }
                        else if (graphic.gameObject.CompareTag("UIPressKeyTip"))
                        {
                            sequence.Append(graphic.DOColor(Color.black, 0.1f));
                        }
                        else
                        {
                            sequence.Append(graphic.DOColor(Color.black, 1f));
                            sequence.AppendInterval(1f);
                        }
                    }
                    sequence.AppendCallback(() => GameLogicManager.instance.EnableRestartGame());
                }
            );
    }

    public void ShowMaskPanelOnEasterEgg()
    {
        gameOverPanel.raycastTarget = true;
        easterEggPanel.raycastTarget = true;
        gameOverPanel.DOColor(new Color(1, 1, 1, 0.5f), 1f).SetUpdate(true)
            .OnComplete(() =>
            {
                Sequence sequence = DOTween.Sequence();
                sequence.SetUpdate(true);
                sequence.AppendInterval(1f);
                sequence.Append(easterEggPanel.rectTransform.DOScale(new Vector3(1, 1, 1), 0.5f));
                sequence.AppendInterval(1f);
                foreach (var graphic in _easterEggPanelChildren)
                {
                    sequence.AppendCallback(() => graphic.gameObject.SetActive(true));
                    if (graphic is Text text)
                    {
                        if (graphic.gameObject.CompareTag("UIPressKeyTip"))
                        {
                            sequence.Append(text.DOText("", text.text.Length * 0.1f).From().SetEase(Ease.Linear));
                        }
                        else
                        {
                            sequence.Append(text.DOText("", 2f).From().SetEase(Ease.Linear));
                            sequence.AppendInterval(1f);
                        }
                    }
                }
                sequence.AppendCallback(() => GameLogicManager.instance.EnableRestartGame());
            });
    }

    public void FadeOutMaskPanel()
    {
        gameOverPanel.DOColor(Color.clear, 3f)
            .OnComplete(() => gameOverPanel.raycastTarget = false);
    }

    public void FadeInMaskPanel()
    {
        gameOverPanel.raycastTarget = true;
        gameOverPanel.DOColor(Color.black, 3f);
    }
}
