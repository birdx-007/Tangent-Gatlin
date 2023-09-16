using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundElement : MonoBehaviour
{
    [NonSerialized] public bool isFacingRight;
    [NonSerialized] public float moveSpeed;
    private IElementState _currentState = null;
    private void Awake()
    {
        float eulerY = transform.localRotation.eulerAngles.y;
        isFacingRight = Mathf.Approximately(eulerY,180); // 约束:localRotation的Y必须是0或180
        ChangeState(new ElementIdleState());
    }

    private void Update()
    {
        _currentState.OnStateUpdate(this);
    }

    public void ChangeState(IElementState newState)
    {
        _currentState?.OnStateExit(this);
        _currentState = newState;
        _currentState?.OnStateEnter(this);
    }

    public void Move(Vector2 direction)
    {
        transform.localPosition += (Vector3)direction * (moveSpeed * Time.deltaTime);
    }

    public void TurnAround()
    {
        transform.localRotation = Quaternion.FromToRotation(Vector3.right, (isFacingRight ? 1f : -1f) * Vector3.right);
        isFacingRight = !isFacingRight;
    }
}
