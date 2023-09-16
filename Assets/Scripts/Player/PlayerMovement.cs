using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 15f;
    public float accelerate = 3f;
    public float jump = 10f;
    public float jumpHold = 0.85f;
    public float maxJumpHoldTime = 0.2f;
    public float gravity = 6f;
    [NonSerialized] public bool isOnGround = false;
    [NonSerialized] public bool isMovingHorizontal = false;
    [NonSerialized] public float currentHorizontalSpeed = 0;
    private bool _isJumping;
    private bool _enableJumpHold;
    private Rigidbody2D _rigidbody2D;
    private float _mass;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.gravityScale = gravity;
        _mass = _rigidbody2D.mass;
        _isJumping = false;
        _enableJumpHold = false;
    }

    public void UpdateMovementState()
    {
        currentHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x);
        isMovingHorizontal = !Mathf.Approximately(currentHorizontalSpeed,0);
    }

    public void Walk(float horizontal)
    {
        float velocityX = Mathf.Clamp(_rigidbody2D.velocity.x, -speed, speed);
        _rigidbody2D.velocity.Set(velocityX,_rigidbody2D.velocity.y);
        
        //_rigidbody2D.velocity = new Vector2(horizontal * speed, _rigidbody2D.velocity.y);
        float speedDiff = horizontal * speed - velocityX;
        _rigidbody2D.AddForce(accelerate * speedDiff * Vector2.right);
    }
    public bool Jump()
    {
        if (!_isJumping)
        {
            _rigidbody2D.AddForce(Vector2.up * (jump * _mass), ForceMode2D.Impulse);
            _isJumping = true;
            _enableJumpHold = true;
            StartCoroutine(TimerForStopEnableJumpHold());
            return true;
        }
        return false;
    }

    public bool JumpHold() // 跳跃时长按调用
    {
        if (_isJumping && _enableJumpHold)
        {
            _rigidbody2D.AddForce(Vector2.up * (jumpHold * _mass), ForceMode2D.Impulse);
            return true;
        }
        return false;
    }

    public bool StopEnableJumpHold()
    {
        if (_isJumping && _enableJumpHold)
        {
            _enableJumpHold = false;
            return true;
        }
        return false;
    }

    IEnumerator TimerForStopEnableJumpHold()
    {
        yield return new WaitForSeconds(maxJumpHoldTime);
        StopEnableJumpHold();
    }

    private void StopJump()
    {
        _isJumping = false;
        _enableJumpHold = false;
    }
    
    public void BeHitAway(Vector3 hitForce)
    {
        _rigidbody2D.AddForce(hitForce,ForceMode2D.Impulse);
        _rigidbody2D.AddForce(Vector2.up * jump * 1.5f, ForceMode2D.Impulse);
        //_rigidbody2D.AddTorque(-hitForce.normalized.x * hitForce.magnitude,ForceMode2D.Impulse);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") &&
            other.gameObject.transform.position.y <= gameObject.transform.position.y)
        {
            StopJump();
            isOnGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") &&
            other.gameObject.transform.position.y <= gameObject.transform.position.y)
        {
            isOnGround = false;
        }
    }

    public void DisableCollision()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }
}
