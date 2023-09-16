using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class EulerBomberController : Enemy
{
    public float recoilForce = 1f;
    public float frictionForce = 1f;
    public float bounceUpForceOnDeath = 20f;
    public Transform bomberHead;
    public Transform bomberBody;
    public Transform bomberTail;
    public TextMesh bomberCount;
    public ParticleSystem sleepParticle;
    private int _count = 0;
    private float _bomberTailHorizontalOffset = 2;

    private void Start()
    {
        base.BaseOnStart();
        SetCount(0);
        bomberTail.localPosition = new Vector3(_bomberTailHorizontalOffset, 0, 0);
    }

    private void Update()
    {
        base.BaseOnUpdate();
        if (isAlive)
        {
            if (isProvoked)
            {
                if (playerTransform)
                {
                    Aim();
                }
                isPlayerInAttackRange = (playerTransform.position - transform.position).magnitude < attackRangeRadius;
            }
            //float v = Input.GetAxis("Vertical");
            //float h = Input.GetAxis("Horizontal");
            //Rigidbody2D.AddForce(new Vector2(h, v) * 10f, ForceMode2D.Force);
            Rigidbody2D.AddForce(Rigidbody2D.velocity * (-frictionForce), ForceMode2D.Force); // 阻力
        }
        UpdateParticleState();
    }

    private void UpdateParticleState()
    {
        if (sleepParticle)
        {
            if (isAlive && !isProvoked)
            {
                if (!sleepParticle.isPlaying)
                {
                    sleepParticle.Play();
                }
            }
            else if (sleepParticle.isPlaying)
            {
                sleepParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    private void SetCount(int count)
    {
        _count = count;
        bomberCount.text = _count.ToString();
    }

    private void Aim()
    {
        Vector3 vectorToTarget = playerTransform.position - bomberHead.position;
        bomberHead.rotation = Quaternion.FromToRotation(Vector3.right, vectorToTarget);
        if (vectorToTarget.x * bomberTail.localPosition.x > 0)
        {
            bomberTail.DOLocalMoveX(vectorToTarget.x < 0 ? _bomberTailHorizontalOffset : -_bomberTailHorizontalOffset, 0.2f);
        }
    }

    public override void Shoot()
    {
        AudioManager.instance.PlaySoundEffect(AudioSource,SoundEffectType.EnemyShoot);
        Animator.SetTrigger("fire");
        GameObject bullet =
            Instantiate(this.bulletPrefab, bomberHead.position + 0f * bomberHead.right,
                Quaternion.Euler(0, 0, 90) * bomberHead.rotation);
        bullet.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
        var bulletController = bullet.GetComponent<BomberBulletController>();
        if (bulletController != null)
        {
            bulletController.SetCount(_count);
        }
        var bulletBody = bullet.GetComponent<Rigidbody2D>();
        if (bulletBody != null)
        {
            bulletBody.AddForce(bomberHead.right * (fireForce * bulletBody.mass), ForceMode2D.Impulse);
        }
        Rigidbody2D.AddForce(bomberHead.right * (-recoilForce * Rigidbody2D.mass), ForceMode2D.Impulse);//后坐力
        SetCount(_count + 1);
    }

    public override void BeHitAway(Vector3 hitForce)
    {
        base.BeHitAway(hitForce);
        Rigidbody2D.constraints = RigidbodyConstraints2D.None;
        Rigidbody2D.AddTorque(-hitForce.normalized.x * hitForce.magnitude,ForceMode2D.Impulse);
    }

    public override void Die()
    {
        base.Die();
        Rigidbody2D.AddForce(Vector2.up * (bounceUpForceOnDeath * Rigidbody2D.mass), ForceMode2D.Impulse);
    }
}
