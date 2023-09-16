using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    public Transform parent;
    public GameObject bulletPrefab;
    public bool enableInfiniteBullets = true;
    public int maxBulletNumber = 10;
    public float recoverTimeForOneBullet = 0.5f;
    [HideInInspector] public int leftBulletNumber;
    public float fireForce = 15f;
    public float recoilForce = 10f;
    public float fallGravity = 4f;
    public Transform firePoint;
    protected Animator Animator;
    protected Rigidbody2D Rigidbody2D;
    protected AudioSource AudioSource;

    private void OnEnable()
    {
        EventCenter.AddListener(EventType.FightStart,DisableInfiniteBullets);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener(EventType.FightStart,DisableInfiniteBullets);
    }

    private void Awake()
    {
        leftBulletNumber = maxBulletNumber;
        
        Animator = GetComponent<Animator>();
        Rigidbody2D = null;
        AudioSource = GetComponent<AudioSource>();
    }

    public virtual void Aim(Vector3 targetPosition)
    {
        Vector3 vectorToTarget = targetPosition - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, vectorToTarget);
    }

    public void TryShoot()
    {
        if (!enableInfiniteBullets)
        {
            EventCenter.Broadcast(EventType.PlayerWeaponTryShoot, leftBulletNumber, maxBulletNumber);
            if (leftBulletNumber == 0)
            {
                return;
            }

            leftBulletNumber--;
        }
        Shoot();
    }

    public virtual void Shoot()
    {
        GameObject bullet = Instantiate(this.bulletPrefab, firePoint.position, firePoint.rotation);
        var bulletBody = bullet.GetComponent<Rigidbody2D>();
        if (bulletBody)
        {
            bulletBody.AddForce(firePoint.right * (fireForce * bulletBody.mass), ForceMode2D.Impulse);
        }
        Rigidbody2D parentBody = parent.GetComponent<Rigidbody2D>();
        if (parentBody)
        {
            parentBody.AddForce(-firePoint.right * (recoilForce * parentBody.mass), ForceMode2D.Impulse);
        }
    }

    public void BeEquipped(Transform parent)
    {
        this.parent = parent;
        Rigidbody2D = null;
    }

    public void BeCanceledEquipped()
    {
        Rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        Rigidbody2D.gravityScale = fallGravity;
        Rigidbody2D.AddForce(Vector2.up * (fallGravity * Rigidbody2D.mass * 2), ForceMode2D.Impulse);
        Rigidbody2D.AddTorque(-2.5f * (parent.rotation.eulerAngles.y > 90 ? 1 : -1), ForceMode2D.Impulse);
        parent = null;
    }

    public void DisableInfiniteBullets()
    {
        enableInfiniteBullets = false;
        StartCoroutine(RecoverBullet());
    }

    IEnumerator RecoverBullet()
    {
        float timer = 0;
        while (true)
        {
            if (leftBulletNumber == maxBulletNumber)
            {
                yield return new WaitUntil(() =>  leftBulletNumber < maxBulletNumber);
                timer = 0;
            }

            if (timer == 0)
            {
                EventCenter.Broadcast(EventType.PlayerWeaponStartRecoverOneBullet, recoverTimeForOneBullet);
            }

            yield return null;
            timer += Time.deltaTime;
            if (timer >= recoverTimeForOneBullet)
            {
                leftBulletNumber++;
                timer = 0;
                EventCenter.Broadcast(EventType.PlayerWeaponEndRecoverOneBullet,leftBulletNumber,maxBulletNumber);
            }
        }
    }
}
