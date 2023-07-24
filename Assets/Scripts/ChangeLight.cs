using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class ChangeLight : MonoBehaviour
{

    private new Light2D light;

    [SerializeField]
    private float minIntensity = 0.1f;

    [SerializeField]
    private float intensityThreshold = 0.3f;

    [SerializeField]
    private float intensityDecreaseStep = 0.001f;

    [SerializeField]
    private float delayBeforeDecreasing = 10f;

    [SerializeField]
    private VoidEventChannel onGlobalLightThresholdReached;

    private void Awake()
    {
        light = GetComponent<Light2D>();
        light.intensity = 1f;
    }

    public void StartGameProxy()
    {   
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {   
        yield return new WaitForSeconds(delayBeforeDecreasing);
        StartCoroutine(DecreaseLight());

        yield return new WaitUntil(() => light.intensity <= intensityThreshold);
        onGlobalLightThresholdReached.Raise();
    }

    IEnumerator DecreaseLight()
    {
        while (light.intensity > minIntensity) {
            light.intensity -= intensityDecreaseStep;
            yield return null;
        }
    }
}
