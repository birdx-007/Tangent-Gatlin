using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{
    [NonSerialized] public bool PlayerJumpKey = false;
    [NonSerialized] private bool _lockForPlayerJumpKeyDown = false;
    [NonSerialized] public bool PlayerJumpKeyDown = false;
    
    [NonSerialized] public bool PlayerShootKey = false;
    [NonSerialized] private bool _lockForPlayerShootKeyDown = false;
    [NonSerialized] public bool PlayerShootKeyDown = false;
    
    [NonSerialized] public bool PlayerDefendKey = false;
    [NonSerialized] private bool _lockForPlayerDefendKeyDown = false;
    [NonSerialized] public bool PlayerDefendKeyDown = false;

    public void UpdateInputState()
    {
        PlayerJumpKey = Input.GetKey(GlobalSettings.KeyPlayerJump);
        if (!_lockForPlayerJumpKeyDown)
        {
            PlayerJumpKeyDown = Input.GetKeyDown(GlobalSettings.KeyPlayerJump);
            if (PlayerJumpKeyDown) //Update检测到按键按下时锁住变量 直到FixedUpdate使用完解锁
            {
                _lockForPlayerJumpKeyDown = true;
            }
        }

        PlayerShootKey = Input.GetKey(GlobalSettings.KeyPlayerShoot);
        if (!_lockForPlayerShootKeyDown)
        {
            PlayerShootKeyDown = Input.GetKeyDown(GlobalSettings.KeyPlayerShoot);
            if (PlayerShootKeyDown)
            {
                _lockForPlayerShootKeyDown = true;
            }
        }
        
        PlayerDefendKey = Input.GetKey(GlobalSettings.KeyPlayerDefend);
        if (!_lockForPlayerDefendKeyDown)
        {
            PlayerDefendKeyDown = Input.GetKeyDown(GlobalSettings.KeyPlayerDefend);
            if (PlayerDefendKeyDown)
            {
                _lockForPlayerDefendKeyDown = true;
            }
        }
    }

    public void UnlockPlayerJumpKeyDownUpdate()
    {
        _lockForPlayerJumpKeyDown = false;
    }

    public void UnlockPlayerShootKeyDownUpdate()
    {
        _lockForPlayerShootKeyDown = false;
    }
    
    public void UnlockPlayerDefendKeyDownUpdate()
    {
        _lockForPlayerDefendKeyDown = false;
    }
}
