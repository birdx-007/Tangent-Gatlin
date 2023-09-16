using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHUDController : MonoBehaviour
{
    [SerializeField] private Image weaponMagazineBackground;
    [SerializeField] private Image weaponBulletBar;
    [SerializeField] private Image weaponBulletCD;
    private Animator _animator;

    private void OnEnable()
    {
        EventCenter.AddListener<int,int>(EventType.PlayerWeaponTryShoot, TryMinusOneBullet);
        EventCenter.AddListener<float>(EventType.PlayerWeaponStartRecoverOneBullet, StartRecoverOneBullet);
        EventCenter.AddListener<int,int>(EventType.PlayerWeaponEndRecoverOneBullet, EndRecoverOneBullet);
        EventCenter.AddListener(EventType.FightStart,ShowHUD);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<int,int>(EventType.PlayerWeaponTryShoot, TryMinusOneBullet);
        EventCenter.RemoveListener<float>(EventType.PlayerWeaponStartRecoverOneBullet, StartRecoverOneBullet);
        EventCenter.RemoveListener<int,int>(EventType.PlayerWeaponEndRecoverOneBullet, EndRecoverOneBullet);
        EventCenter.RemoveListener(EventType.FightStart,ShowHUD);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void ShowHUD()
    {
        _animator.SetTrigger("show");
    }

    public void DisableHUDAnimator()
    {
        _animator.enabled = false;
    }

    private void TryMinusOneBullet(int leftBulletNumber,int maxBulletNumber)
    {
        DOTween.To(() => weaponBulletBar.fillAmount, value => weaponBulletBar.fillAmount = value,
            Mathf.Clamp((float)(leftBulletNumber - 1) / maxBulletNumber, 0, 1), 0.1f);
        if (leftBulletNumber == 0)
        {
            weaponMagazineBackground.DOColor(new Color(1,0,0,0.75f), 0.15f).OnComplete(() =>
                weaponMagazineBackground.DOColor(Color.black, 0.35f));
        }
    }

    private void StartRecoverOneBullet(float recoverTime)
    {
        StartCoroutine(RecoverOneBullet(recoverTime));
    }

    IEnumerator RecoverOneBullet(float recoverTime)
    {
        float timer = 0;
        while (timer < recoverTime)
        {
            yield return null;
            timer += Time.deltaTime;
            weaponBulletCD.fillAmount = Mathf.Clamp(timer / recoverTime, 0, 1);
        }
    }

    private void EndRecoverOneBullet(int leftBulletNumber,int maxBulletNumber)
    {
        DOTween.To(() => weaponBulletBar.fillAmount, value => weaponBulletBar.fillAmount = value,
            Mathf.Clamp((float)leftBulletNumber / maxBulletNumber, 0, 1), 0.1f);
        weaponBulletCD.fillAmount = (leftBulletNumber == maxBulletNumber) ? 1 : 0;
    }
}
