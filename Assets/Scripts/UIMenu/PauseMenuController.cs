using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public Image pauseTip;
    public Sprite tipBeforeFight;
    public Sprite tipAfterFight;
    private void OnEnable()
    {
        EventCenter.AddListener(EventType.FightStart, ChangeTipOnFight);
    }
    private void OnDisable()
    {
        EventCenter.RemoveListener(EventType.FightStart, ChangeTipOnFight);
    }
    private void Awake()
    {
        pauseTip.sprite = tipBeforeFight;
    }
    void ChangeTipOnFight()
    {
        pauseTip.sprite = tipAfterFight;
    }
}
