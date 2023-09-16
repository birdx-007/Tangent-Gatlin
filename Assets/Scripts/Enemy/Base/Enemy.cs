using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    public bool isAlive = true;
    public bool isProvoked = false; // 被激怒
    public bool isPlayerInAttackRange = false;
    public float attackRangeRadius = 8f;
    public float slowChaseRangeRadius = 11f;
    public float moveForce = 10f;
    public float fireForce = 10f;
    [NonSerialized] public GameObject bulletPrefab;
    [NonSerialized] public Transform playerTransform;
    protected Rigidbody2D Rigidbody2D;
    protected Animator Animator;
    protected AudioSource AudioSource;
    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
    }

    protected void BaseOnStart()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    protected void BaseOnUpdate()
    {
        if (isAlive)
        {
            
        }
    }

    public virtual void Die()
    {
        isAlive = false;
        playerTransform = null;
        GameLogicManager.instance.EnemyDeathDetected(this);
    }

    public void Move(Vector2 direction, float targetSpeed)
    {
        Vector2 velocityDiff = direction.normalized * targetSpeed - Rigidbody2D.velocity;
        Rigidbody2D.AddForce(velocityDiff * (moveForce * Rigidbody2D.mass));
    }

    public virtual void Shoot()
    {
        GameObject bullet = Instantiate(this.bulletPrefab, transform.position, Quaternion.identity);
        //bullet.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
        var bulletBody = bullet.GetComponent<Rigidbody2D>();
        if (bulletBody != null)
        {
            bulletBody.AddForce(transform.right * (fireForce * bulletBody.mass), ForceMode2D.Impulse);
        }
    }
    
    public virtual void BeHitAway(Vector3 hitForce)
    {
        Rigidbody2D.AddForce(hitForce,ForceMode2D.Impulse);
    }
}
