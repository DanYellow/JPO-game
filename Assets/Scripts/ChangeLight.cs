using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class ChangeLight : MonoBehaviour
{

    private new Light2D light;

    [SerializeField]
    private float minIntensity = 0.1f;

    [SerializeField]
    private float intensityDecreaseStep = 0.001f;

    [SerializeField]
    private float delayBeforeDecrease = 0.1f;

    [SerializeField]
    private float intensityDecreaseTime = 1f;

    private void Awake()
    {
        light = GetComponent<Light2D>();
        light.intensity = 1;
        Debug.Log("ffeee");
    }

    private IEnumerator Start()
    {   
        yield return new WaitForSeconds(delayBeforeDecrease);
        StartCoroutine(DecreaseLight());
    }

    IEnumerator DecreaseLight()
    {
        while (light.intensity > minIntensity) {
            yield return new WaitForSeconds(intensityDecreaseTime);
            light.intensity -= intensityDecreaseStep;
        }
    }
}
