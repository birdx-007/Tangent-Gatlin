using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.Serialization;

public class CMVirtualCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _noise;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void CameraShake(float amplitudeGain, float frequencyGain, float time, float keepTime)
    {
        List<Tween> shakeTweens = DOTween.TweensById("CameraShake", true);
        if (shakeTweens is not null && shakeTweens.Count > 0)
        {
            foreach (var tween in shakeTweens)
            {
                tween.Complete();
            }
        }
        _noise.m_AmplitudeGain = amplitudeGain;
        _noise.m_FrequencyGain = frequencyGain;
        Sequence sequence = DOTween.Sequence();
        sequence.SetId("CameraShake");
        sequence.SetUpdate(false);
        sequence.AppendInterval(keepTime);
        sequence.Append(DOTween.To(() => _noise.m_AmplitudeGain, value => _noise.m_AmplitudeGain = value, 0,
            time - keepTime));
    }
}
