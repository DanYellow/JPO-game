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

    void ShakeCameraProxy(CameraShakeTypeValue value) {
        Debug.Log("ShakeCameraProxy");
        StartCoroutine(ShakeCamera(value.intensity, value.time));
    }

    IEnumerator ShakeCamera(float intensity = 3f, float time = 2f)    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }

     private void OnDestroy()
    {
        onCinemachineShake.OnEventRaised -= ShakeCameraProxy;
    }
}
