using UnityEngine;
using System.Collections;
using Cinemachine.PostFX;
using UnityEngine.Rendering.Universal;

public class CameraEffects : MonoBehaviour
{
    [SerializeField]
    private float chromaticAberrationStep = 0.025f;

    private ChromaticAberration chromaticAberrationClone;

    [SerializeField]
    private Material backgroundRender;

    [SerializeField, ColorUsage(true, true)]
    private Color startColor;
    [SerializeField, ColorUsage(true, true)]
    private Color damageColor;

    private float initialVignettePower;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onScoreThresholdReached;

    [SerializeField]
    private FloatValue distanceTravelled;

    [SerializeField]
    private VoidEventChannel onCarDamage;

    [SerializeField]
    private BoolValue isCarTakingDamage;


    private void OnEnable()
    {
        onScoreThresholdReached.OnEventRaised += ScoreEffect;
        onCarDamage.OnEventRaised += DamageIndicator;
    }

    void Start()
    {
        backgroundRender.SetColor("_VignetteColor", startColor);
        initialVignettePower = backgroundRender.GetFloat("_VignettePower");
        CinemachineVolumeSettings cinemachineVolumeSettings = GetComponent<CinemachineVolumeSettings>();

        if (cinemachineVolumeSettings.m_Profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberrationClone = chromaticAberration;
        }
        chromaticAberrationClone.intensity.value = 0;
        chromaticAberrationClone.intensity.overrideState = true;
    }

    private void ScoreEffect()
    {
        chromaticAberrationClone.intensity.value += chromaticAberrationStep;
        StartCoroutine(NextThresholdReached(chromaticAberrationClone.intensity.value));
    }

    private void DamageIndicator()
    {
        StartCoroutine(DamageIndicatorCoroutine());
    }

    private IEnumerator DamageIndicatorCoroutine()
    {
        if (isCarTakingDamage.CurrentValue)
        {
            yield break;
        }
        isCarTakingDamage.CurrentValue = true;
        backgroundRender.SetColor("_VignetteColor", damageColor);
        yield return Helpers.GetWait(0.5f);
        backgroundRender.SetColor("_VignetteColor", startColor);
        yield return Helpers.GetWait(0.65f);
        isCarTakingDamage.CurrentValue = false;
    }

    private IEnumerator NextThresholdReached(float startValue)
    {
        float current = 0;
        float duration = 0.45f;

        float factor = 0.8f;

        yield return null;
        backgroundRender.SetInt("_HideGradient", 1);
        while (current <= 1)
        {
            chromaticAberrationClone.intensity.value = Mathf.Lerp(startValue, 1, Spike(current));
            backgroundRender.SetFloat("_VignettePower", Mathf.Lerp(initialVignettePower, initialVignettePower * factor, Spike(current)));
            current += Time.deltaTime / duration;

            yield return null;
        }

        current = 0;
        while (current <= 1)
        {
            chromaticAberrationClone.intensity.value = Mathf.Lerp(1, startValue, Spike(current));
            backgroundRender.SetFloat("_VignettePower", Mathf.Lerp(initialVignettePower * factor, initialVignettePower, Spike(current)));
            current += Time.deltaTime / duration;

            yield return null;
        }
        backgroundRender.SetInt("_HideGradient", 0);
        chromaticAberrationClone.intensity.value = startValue;
        backgroundRender.SetFloat("_VignettePower", initialVignettePower);
    }

    private float EaseIn(float t)
    {
        return t * t;
    }

    private float Spike(float t)
    {
        if (t <= .5f)
            return EaseIn(t / .5f);
        return EaseIn(Flip(t) / .5f);
    }

    public float Flip(float x)
    {
        return 1 - x;
    }

    private void OnDisable()
    {
        onScoreThresholdReached.OnEventRaised -= ScoreEffect;
        onCarDamage.OnEventRaised -= DamageIndicator;
    }

}
