using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        AudioManager.instance.PlaySoundEffect(_audioSource,SoundEffectType.EnemyShootHit);
        PlayerController player = other.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            OnHitPlayer(player);
            return;
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            OnHitGround(other.collider.gameObject);
            return;
        }

        WeaponBullet bullet = other.collider.GetComponent<WeaponBullet>();
        if (bullet != null)
        {
            OnHitPlayerBullet(bullet);
            return;
        }
    }
    protected virtual void OnHitPlayer(PlayerController player)
    {
        Destroy(gameObject);
    }

    protected virtual void OnHitPlayerBullet(WeaponBullet bullet)
    {
        Destroy(gameObject);
    }
    
    protected override void OnHitGround(GameObject ground)
    {
        Destroy(gameObject);
    }
}
