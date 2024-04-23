using UnityEngine;
using Cinemachine.PostFX;
using UnityEngine.Rendering.Universal;

public class CameraEffects : MonoBehaviour
{
    [SerializeField]
    private FloatValue distanceTravelled;

    [SerializeField]
    private float chromaticAberrationStep = 0.025f;

    private float lastThousandth = 0;

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
        float thousandth = Mathf.Floor(distanceTravelled.CurrentValue / 850);
        if (thousandth >= 1 && thousandth > lastThousandth)
        {
            print($"Increased ! {thousandth}");
            lastThousandth = thousandth;
            chromaticAberrationClone.intensity.value += chromaticAberrationStep;
        }
    }
}
