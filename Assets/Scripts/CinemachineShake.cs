using System.Collections;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField]
    private CinemachineShakeEventChannel onCinemachineShake;

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        onCinemachineShake.OnEventRaised += ShakeCameraProxy;
    }

    private void ShakeCameraProxy(CameraShakeType value)
    {
        StartCoroutine(ShakeCamera(value.intensity, value.duration));
    }

    private IEnumerator ShakeCamera(float intensity = 3f, float time = 2f)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = time;
        yield return new WaitForSecondsRealtime(time);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }

    private void OnDisable()
    {
        onCinemachineShake.OnEventRaised -= ShakeCameraProxy;
    }
}