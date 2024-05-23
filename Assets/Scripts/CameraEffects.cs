using UnityEngine;
using System.Collections;
using Cinemachine.PostFX;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField]
    private float chromaticAberrationStep = 0.025f;

    private ChromaticAberration chromaticAberrationClone;

    private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField]
    private Material backgroundRender;

    [SerializeField, ColorUsage(true, true)]
    private Color startColor;
    [SerializeField, ColorUsage(true, true)]
    private Color damageColor;

    private Bloom bloomClone;

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

    [SerializeField]
    private VoidEventChannel onCarBoost;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        onScoreThresholdReached.OnEventRaised += ScoreEffect;
        onCarDamage.OnEventRaised += DamageIndicator;
        onCarBoost.OnEventRaised += BoostEffect;
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

        if (cinemachineVolumeSettings.m_Profile.TryGet(out Bloom bloom))
        {
            bloomClone = bloom;
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
        float duration = 0.3f;

        float factor = 0.95f;
        float startBloomIntensity = bloomClone.intensity.value;
        float endBloomIntensity = 1.5f;

        yield return null;
        backgroundRender.SetInt("_HideGradient", 1);
        while (current <= 1)
        {
            chromaticAberrationClone.intensity.value = Mathf.Lerp(startValue, 1, current);
            bloomClone.intensity.value = Mathf.Lerp(startBloomIntensity, endBloomIntensity, current);
            backgroundRender.SetFloat("_VignettePower", Mathf.Lerp(initialVignettePower, initialVignettePower * factor, current));
            current += Time.deltaTime / duration;

            yield return null;
        }

        current = 0;
        while (current <= 1)
        {
            chromaticAberrationClone.intensity.value = Mathf.Lerp(1, startValue, current);
            bloomClone.intensity.value = Mathf.Lerp(endBloomIntensity, startBloomIntensity, current);
            backgroundRender.SetFloat("_VignettePower", Mathf.Lerp(initialVignettePower * factor, initialVignettePower, current));
            current += Time.deltaTime / duration;

            yield return null;
        }
        backgroundRender.SetInt("_HideGradient", 0);
        bloomClone.intensity.value = startBloomIntensity;
        chromaticAberrationClone.intensity.value = startValue;
        backgroundRender.SetFloat("_VignettePower", initialVignettePower);
    }

    private void BoostEffect() {
        StartCoroutine(BoostEffectProxy());
    }

    private IEnumerator BoostEffectProxy()
    {
        float current = 0;
        float duration = 0.95f;

        CinemachineFramingTransposer cinemachineFramingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        yield return null;

        while (current <= 1)
        {
            cinemachineFramingTransposer.m_XDamping = Mathf.SmoothStep(1, 0, current);
            cinemachineFramingTransposer.m_YDamping = Mathf.SmoothStep(1, 0, current);
            cinemachineFramingTransposer.m_ZDamping = Mathf.SmoothStep(1, 0, current);
            
            current += Time.deltaTime / duration;

            yield return null;
        }
    }
    private void OnDisable()
    {
        onScoreThresholdReached.OnEventRaised -= ScoreEffect;
        onCarDamage.OnEventRaised -= DamageIndicator;
        onCarBoost.OnEventRaised -= BoostEffect;
    }
}
