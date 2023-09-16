using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangentGatlinController : Weapon
{
    public override void Shoot()
    {
        base.Shoot();
        AudioManager.instance.PlaySoundEffect(AudioSource,SoundEffectType.PlayerShoot);
        Animator.SetTrigger("fire");
        GameLogicManager.instance.ShakeCamera(1,1,0.1f);
    }
}
