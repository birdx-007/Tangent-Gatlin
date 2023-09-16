using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GatlinBulletController : WeaponBullet
{
    [SerializeField] private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer explosion;
    [SerializeField] private ParticleSystem trailParticle;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnHitEnemy(Enemy enemy)
    {
        enemy.BeHitAway(_rigidbody2D.velocity.normalized * hitForce);
        enemy.Die();
        DisplayExplosionThenDestroy();
        GameLogicManager.instance.ChangeTimeScale(0f,0.05f,0.05f);
        GameLogicManager.instance.ShakeCamera(5, 1, 1f);
    }

    protected override void OnHitEnemyBullet(EnemyBullet bullet)
    {
        DisplayExplosionThenDestroy();
        GameLogicManager.instance.ShakeCamera(2.5f, 1, 1f);
    }

    protected override void OnHitGround(GameObject ground)
    {
        DisplayExplosionThenDestroy();
        GameLogicManager.instance.ShakeCamera(2.5f, 1, 1f);
    }

    private void DisplayExplosionThenDestroy()
    {
        _rigidbody2D.velocity = new Vector2(0, 0);
        _collider2D.isTrigger = true;
        _spriteRenderer.sprite = null;
        trailParticle.Stop(true,ParticleSystemStopBehavior.StopEmitting);
        // explosion animation
        explosion.color = Color.white;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(0.05f);
        sequence.Append(DOTween.To(() => explosion.color, value => explosion.color = value, new Color(1, 1, 1, 0),
            0.3f));
        sequence.AppendCallback(() => Destroy(gameObject));
    }
}
