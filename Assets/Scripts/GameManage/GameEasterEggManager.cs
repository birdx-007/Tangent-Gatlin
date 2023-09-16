using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEasterEggManager : MonoBehaviour
{
    public Transform playerTransform;
    public Transform groundTransform;
    private bool _hasEasterEggTriggered = false;
    private PlayerController _player;

    private void Awake()
    {
        _player = playerTransform.gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!_hasEasterEggTriggered && playerTransform.position.y < groundTransform.position.y - 1f)
        {
            _hasEasterEggTriggered = true;
            _player.DieOfEasterEgg();
        }
    }
}
