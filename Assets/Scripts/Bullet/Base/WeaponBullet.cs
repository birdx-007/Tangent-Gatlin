using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponBullet : Bullet
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        AudioManager.instance.PlaySoundEffect(_audioSource,SoundEffectType.PlayerShootHit);
        Enemy enemy = other.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            OnHitEnemy(enemy);
            return;
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            OnHitGround(other.collider.gameObject);
            return;
        }

        EnemyBullet bullet = other.collider.GetComponent<EnemyBullet>();
        if (bullet != null)
        {
            OnHitEnemyBullet(bullet);
            return;
        }
    }

    protected virtual void OnHitEnemy(Enemy enemy)
    {
        Destroy(gameObject);
    }

    protected virtual void OnHitEnemyBullet(EnemyBullet bullet)
    {
        Destroy(gameObject);
    }

    protected override void OnHitGround(GameObject ground)
    {
        Destroy(gameObject);
    }
}
