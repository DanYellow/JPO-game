using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    private UnityAction onBipedalBossActivationEvent;

    [SerializeField]
    private VoidEventChannel onBipedalBossActivation;

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private void Awake() {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

        onBipedalBossActivationEvent = () => { StartCoroutine(ShakeCamera()); };
        onBipedalBossActivation.OnEventRaised += onBipedalBossActivationEvent;
    }

    // [ContextMenu("ShakeCamera")]
    IEnumerator ShakeCamera(float intensity = 3f, float time = 2f)
    {
        
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        // cinemachineBasicMultiChannelPerlin.m_FrequencyGain = time;
    }

     private void OnDestroy()
    {
        onBipedalBossActivation.OnEventRaised -= onBipedalBossActivationEvent;
    }
}
