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
        light = GetComponentInChildren<Light2D>(true);
        light.gameObject.SetActive(false);
    }

    IEnumerator LightVariation()
    {
        while (light.intensity <= defaultLightIntensity)
        {
            light.intensity += 0.005f;
            yield return null;
        }

        yield return null;

        while (true)
        {
            light.intensity = 0.95f - Mathf.PingPong(Time.time, Random.Range(0.01f, 0.10f));
            yield return null;
        }
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
        
        light.intensity = 0;
        light.gameObject.SetActive(true);

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
