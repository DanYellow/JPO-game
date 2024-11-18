using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class VFX : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField]
    private VectorEventChannel onPlayerExitEvent;

    private ChromaticAberration chromaticAberrationClone;

    private void Awake()
    {
        Volume volume = gameObject.GetComponent<Volume>();

        if (volume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberrationClone = chromaticAberration;
            chromaticAberrationClone.intensity.value = 0;
        }
    }

    private void OnEnable()
    {
        onPlayerExitEvent.OnEventRaised += OnPlayerDeath;
    }

    private void OnPlayerDeath(Vector3 exitPoint)
    {
        chromaticAberrationClone.intensity.value = 0.65f;
        chromaticAberrationClone.intensity.overrideState = true;
        StartCoroutine(DecreaseDeathEffect());
    }

    IEnumerator DecreaseDeathEffect()
    {
        float current = 0;
        float duration = 0.09f;
        float startValue = chromaticAberrationClone.intensity.value;

        while (current <= 1)
        {
            chromaticAberrationClone.intensity.value = Mathf.Lerp(startValue, 0, current);
            current += Time.deltaTime / duration;

            yield return null;
        }

        chromaticAberrationClone.intensity.value = 0;
    }

    private void OnDisable()
    {
        onPlayerExitEvent.OnEventRaised -= OnPlayerDeath;
    }
}
