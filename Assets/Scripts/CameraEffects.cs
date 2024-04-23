using UnityEngine;
using System.Collections;
using Cinemachine.PostFX;
using UnityEngine.Rendering.Universal;

public class CameraEffects : MonoBehaviour
{
    [SerializeField]
    private FloatValue distanceTravelled;

    [SerializeField]
    private float chromaticAberrationStep = 0.025f;

    private float lastThousandth = 0;

    [SerializeField]
    private int scoreStepThreshold = 850;

    private ChromaticAberration chromaticAberrationClone;

    void Start()
    {
        CinemachineVolumeSettings cinemachineVolumeSettings = GetComponent<CinemachineVolumeSettings>();

        if (cinemachineVolumeSettings.m_Profile.TryGet(out ChromaticAberration chromaticAberration))
        {

            chromaticAberrationClone = chromaticAberration;
        }
        chromaticAberrationClone.intensity.value = 0;
        chromaticAberrationClone.intensity.overrideState = true;
    }

    // Update is called once per frame
    void Update()
    {
        float thousandth = Mathf.Floor(distanceTravelled.CurrentValue / scoreStepThreshold);
        if (thousandth >= 1 && thousandth > lastThousandth)
        {
            print($"Increased ! {thousandth}");
            lastThousandth = thousandth;
            chromaticAberrationClone.intensity.value += chromaticAberrationStep;
            StartCoroutine(NextThresholdReached(chromaticAberrationClone.intensity.value));
        }
    }

    private IEnumerator NextThresholdReached(float startValue)
    {
        float current = 0;
        float duration = 0.25f;

        yield return null;
        while (current <= 1)
        {
            chromaticAberrationClone.intensity.value = Mathf.Lerp(startValue, 1,  Spike(current));
            current += Time.deltaTime / duration;

            yield return null;
        }

        current = 0;
        while (current <= 1)
        {
            chromaticAberrationClone.intensity.value = Mathf.Lerp(1, startValue, Spike(current));

            current += Time.deltaTime / duration;

            yield return null;
        }

        chromaticAberrationClone.intensity.value = startValue;
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
}
