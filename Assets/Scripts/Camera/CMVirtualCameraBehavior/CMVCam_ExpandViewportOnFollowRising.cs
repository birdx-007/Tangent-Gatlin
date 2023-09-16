using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CMVCam_ExpandViewportOnFollowRising : MonoBehaviour
{
    public float maxOrthoSize = 15f;
    public float maxSoftZoneHeight = 1.5f;
    public float followHeightOnMaxExpand = 15f;
    public float followHeightThreshold = 4f;
    private float _originOrthoSize;
    private float _originSoftZoneHeight;
    private CinemachineVirtualCamera _virtualCamera;
    private Transform _follow;
    private Rigidbody2D _followBody;
    private CinemachineFramingTransposer _framingTransposer;
    void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _originOrthoSize = _virtualCamera.m_Lens.OrthographicSize;
        _follow = _virtualCamera.m_Follow;
        _followBody = _follow.GetComponent<Rigidbody2D>();
        _framingTransposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _originSoftZoneHeight = _framingTransposer.m_SoftZoneHeight;
    }

    private void Update()
    {
        float followY = _follow.position.y;
        float ratioOfDeltaOrthoSizeToDeltaHeight =
            (maxOrthoSize - _originOrthoSize) / (followHeightOnMaxExpand - followHeightThreshold);
        _virtualCamera.m_Lens.OrthographicSize =
            Mathf.Clamp(_originOrthoSize + (followY - followHeightThreshold) * ratioOfDeltaOrthoSizeToDeltaHeight,
                _originOrthoSize,
                maxOrthoSize);
        float ratioOfDeltaSoftZoneHeightToDeltaHeight = (maxSoftZoneHeight - _originSoftZoneHeight) /
                                                        (followHeightOnMaxExpand - followHeightThreshold);
        _framingTransposer.m_SoftZoneHeight =
            Mathf.Clamp(
                _originSoftZoneHeight + (followY - followHeightThreshold) * ratioOfDeltaSoftZoneHeightToDeltaHeight,
                _originSoftZoneHeight, maxSoftZoneHeight);
    }
}
