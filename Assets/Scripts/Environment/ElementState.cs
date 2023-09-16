using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public interface IElementState
{
    public void OnStateEnter(BackgroundElement element);
    public void OnStateUpdate(BackgroundElement element);
    public void OnStateExit(BackgroundElement element);
}

public class ElementIdleState : IElementState
{
    private Vector2 _moveDirection;
    private const float MinIdleSpeed = 2f;
    private const float MaxIdleSpeed = 10f;
    private const float MaxLocalX = 18f;
    private const float MinLocalX = -18f;
    private const float MaxLocalY = 10f;
    private const float MinLocalY = 2f;

    public void OnStateEnter(BackgroundElement element)
    {
        element.moveSpeed = Random.Range(MinIdleSpeed, MaxIdleSpeed);
    }

    public void OnStateUpdate(BackgroundElement element)
    {
        Transform transform = element.transform;
        if (transform.localPosition.x < MinLocalX || transform.localPosition.x > MaxLocalX)
        {
            transform.localPosition = new Vector2(Mathf.Clamp(transform.localPosition.x, MinLocalX, MaxLocalX),
                Random.Range(MinLocalY, MaxLocalY));
            element.TurnAround();
            element.moveSpeed = Random.Range(MinIdleSpeed, MaxIdleSpeed);
        }
        _moveDirection = new Vector2(element.isFacingRight ? 1 : -1, 0);
        element.Move(_moveDirection);
    }

    public void OnStateExit(BackgroundElement element)
    {
        
    }
}

public class ElementTenseState : IElementState
{
    private float _originMoveSpeed;
    private Vector2 _moveDirection;
    private float _stopTimer;
    private bool _isStopping;
    private const float stopTime = 1.25f;
    private const float tenseSpeed = 20f;
    public void OnStateEnter(BackgroundElement element)
    {
        _originMoveSpeed = element.moveSpeed;
        _stopTimer = stopTime;
        _isStopping = true;
    }

    public void OnStateUpdate(BackgroundElement element)
    {
        if (_isStopping)
        {
            _stopTimer -= Time.deltaTime;
        }
        if (_stopTimer <= 0)
        {
            if (_isStopping)
            {
                _isStopping = false;
                element.moveSpeed = tenseSpeed;
                bool willMoveRight = element.transform.transform.localPosition.x > 0;
                if (willMoveRight ^ element.isFacingRight)
                {
                    element.TurnAround();
                }
                _moveDirection = new Vector2(element.isFacingRight ? 1 : -1, 0);
            }
        }
        else
        {
            element.moveSpeed = Mathf.Lerp(_originMoveSpeed, 0, _stopTimer / stopTime);
        }
        element.Move(_moveDirection);
    }

    public void OnStateExit(BackgroundElement element)
    {
        
    }
}