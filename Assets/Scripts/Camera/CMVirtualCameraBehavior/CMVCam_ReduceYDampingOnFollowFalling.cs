using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 当Follow的Transform下落时将YDamping变小以紧跟（参考空洞骑士相机移动模式）
/// </summary>
public class CMVCam_ReduceYDampingOnFollowFalling : MonoBehaviour
{
    public float followSpeedThreshold = -25f;
    public float followFallingYDamping = 0f;
    public float yDampingChangeTime = 0.5f;
    private float _originYDamping;
    private CinemachineVirtualCamera _virtualCamera;
    private Transform _follow;
    private Rigidbody2D _followBody;
    private CinemachineFramingTransposer _framingTransposer;
    //private bool _isChangingYDamping = false;

    void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _follow = _virtualCamera.m_Follow;
        _followBody = _follow.GetComponent<Rigidbody2D>();
        _framingTransposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _originYDamping = _framingTransposer.m_YDamping;
    }

    void Update()
    {
        float targetYDamping;
        if (_followBody.velocity.y < followSpeedThreshold)
        {
            targetYDamping = followFallingYDamping;
        }
        else
        {
            targetYDamping = _originYDamping;
        }

        if (!Mathf.Approximately(targetYDamping, _framingTransposer.m_YDamping))
        {
            _framingTransposer.m_YDamping = Mathf.MoveTowards(_framingTransposer.m_YDamping, targetYDamping,
                Time.deltaTime / yDampingChangeTime * (_originYDamping - followFallingYDamping));
        }
    }
}
