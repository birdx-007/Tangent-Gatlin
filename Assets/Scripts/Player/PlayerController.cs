using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PlayerInputSystem))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerWeaponSystem))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement _movement;
    public PlayerMovement Movement
    {
        get {return _movement;}
    }
    private PlayerWeaponSystem _weapon;
    public PlayerWeaponSystem WeaponSystem
    {
        get { return _weapon; }
    }
    private PlayerInputSystem _input;
    private Transform _transform;
    private Animator _animator;
    [SerializeField] private ParticleSystem groundParticle;
    private float _inputHorizontal;
    private float _inputVertical;
    private bool _isFacingRight;
    private float _elapsedTimeTurningAround;
    private Quaternion _turningAroundTargetRotation;
    public bool enableInput = true;
    public bool isAlive = true;
    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _weapon = GetComponent<PlayerWeaponSystem>();
        _input = GetComponent<PlayerInputSystem>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        groundParticle.Stop();
        _inputHorizontal = 0f;
        _inputVertical = 0f;
        _isFacingRight = true;
        _elapsedTimeTurningAround = 0f;
        _turningAroundTargetRotation = transform.rotation;
    }

    private void Update()
    {
        if (enableInput)
        {
            _input.UpdateInputState();
            _inputHorizontal = Input.GetAxis("Horizontal");
            _animator.SetBool("isWalking", !Mathf.Approximately(_inputHorizontal, 0f));
            _isFacingRight = (_inputHorizontal > 0f) ? true : ((_inputHorizontal < 0f) ? false : _isFacingRight);

            _inputVertical = Input.GetAxis("Vertical");
            if (!Mathf.Approximately(_inputVertical, 0f))
            {
                _weapon.AdjustRotation(_inputVertical);
            }

            //_spriteRenderer.flipX = !_isFacingRight;
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, (_isFacingRight ? 1f : -1f) * Vector3.right);
            if (_turningAroundTargetRotation != targetRotation)
            {
                float totalTurningTime = 0.15f;
                AudioManager.instance.PlaySoundEffect(SoundEffectType.PlayerTurnAround);
                _transform.DORotateQuaternion(targetRotation, totalTurningTime - _elapsedTimeTurningAround);
                _turningAroundTargetRotation = targetRotation;
            }
            else
            {
                _elapsedTimeTurningAround = 0f;
            }

            if (_input.PlayerShootKeyDown)
            {
                _weapon.Shoot();
                _input.UnlockPlayerShootKeyDownUpdate();
            }

            if (_input.PlayerDefendKeyDown)
            {
                _input.UnlockPlayerDefendKeyDownUpdate();
            }
        }
        else
        {
            _inputHorizontal = 0f;
            _animator.SetBool("isWalking", false);
        }
        _movement.UpdateMovementState();
        UpdateParticleState();
    }

    private void FixedUpdate()
    {
        _movement.Walk(_inputHorizontal);
        if (_input.PlayerJumpKey)
        {
            if (_input.PlayerJumpKeyDown)
            {
                if (_movement.Jump())
                {
                    _animator.SetTrigger("jump");
                }
                _input.UnlockPlayerJumpKeyDownUpdate();
            }
            else
            {
                _movement.JumpHold();
            }
        }
        else
        {
            _movement.StopEnableJumpHold();
        }
    }

    private void UpdateParticleState()
    {
        if (!isAlive)
        {
            if (groundParticle.isPlaying)
            {
                groundParticle.Stop(false,ParticleSystemStopBehavior.StopEmitting);
            }
            return;
        }
        if (_movement.isMovingHorizontal && _movement.isOnGround)
        {
            if (!groundParticle.isPlaying)
            {
                groundParticle.Play();
            }
        }
        else if (groundParticle.isPlaying)
        {
            groundParticle.Stop(false,ParticleSystemStopBehavior.StopEmitting);
        }

        var emission = groundParticle.emission;
        emission.rateOverTime = _movement.currentHorizontalSpeed * 2;
    }

    public void Die()
    {
        if (isAlive)
        {
            isAlive = false;
            FreezeInput();
            //_movement.DisableCollision();
            _weapon.CancelEquipWeapon();
            GameLogicManager.instance.PlayerDeathDetected();
        }
    }

    public void DieOfEasterEgg()
    {
        if (isAlive)
        {
            isAlive = false;
            FreezeInput();
            //_weapon.CancelEquipWeapon();
            GameLogicManager.instance.PlayerEasterEggDetected();
        }
    }

    private float GetAnimationLength(string name)
    {
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name.Equals(name))
            {
                return clip.length;
            }
        }
        throw new ArgumentException($"No such clip name: {name}");
    }

    public void FreezeInput()
    {
        enableInput = false;
    }

    public void UnfreezeInput()
    {
        enableInput = true;
    }
}
