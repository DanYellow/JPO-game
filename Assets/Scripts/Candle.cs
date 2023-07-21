using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class Candle : MonoBehaviour
{
    private new Light2D light;
    [SerializeField]
    private VoidEventChannel onLightOn;

    [SerializeField]
    private float defaultLightIntensity = 0.95f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (light != null)
        {
            // float newY = (Mathf.Sin(Time.time * speed) * height) + _originPosition.y;
            // light.intensity = 0.95f - Mathf.PingPong(Time.time, 0.10f);
        }
    }

    IEnumerator LightVariation()
    {
        while (light.intensity <= defaultLightIntensity)
        {
            light.intensity += 0.01f;
            yield return null;
        }
    
        // while (true)
        // {
        //     light.intensity = 0.95f - Mathf.PingPong(Time.time, 0.10f);
        //     yield return null;
        // }
    }

    private void OnEnable()
    {
        onLightOn.OnEventRaised += LightOnProxy;
    }

    IEnumerator LightOn()
    {
        animator.SetTrigger("LightOn");

        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        light = GetComponentInChildren<Light2D>();
        light.intensity = 0;

        StartCoroutine(LightVariation());
    }

    private void LightOnProxy()
    {
        StartCoroutine(LightOn());
    }

    private void OnDisable()
    {
        onLightOn.OnEventRaised -= LightOnProxy;
    }
}
