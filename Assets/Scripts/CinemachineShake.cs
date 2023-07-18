using System.Collections;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    [SerializeField]
    private CinemachineShakeEventChannel onCinemachineShake;

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private void Awake() {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

        onCinemachineShake.OnEventRaised += ShakeCameraProxy;
    }

    public void ShakeCameraProxy(CameraShakeTypeValue value) {
        StartCoroutine(ShakeCamera(value.intensity, value.duration));
    }

    IEnumerator ShakeCamera(float intensity = 3f, float time = 2f)    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = time;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }

     private void OnDisable()
    {
        onCinemachineShake.OnEventRaised -= ShakeCameraProxy;
    }
}
