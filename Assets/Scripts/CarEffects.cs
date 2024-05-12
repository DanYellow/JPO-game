using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class CarEffects : MonoBehaviour
{
    [SerializeField]
    private GameObject skidMarks;

    private Vector3 moveInput = Vector3.zero;

    [SerializeField]
    private GameObject crater;

    [SerializeField]
    private GameObject carMesh;

    [SerializeField]
    private ParticleSystem driftSmokeParticles;

    private bool isGameStarted = false;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onCarSlowdown;

    [SerializeField]
    private VoidEventChannel onGameOver;

    [SerializeField]
    private BoolValue isCarDrifting;

    [SerializeField]
    private VoidEventChannel onGameStart;

    [SerializeField]
    private VoidEventChannel onGameCameraBlendFinished;

    private void Awake()
    {
        crater.SetActive(false);
        carMesh.SetActive(true);

        driftSmokeParticles.Stop();
    }

    private void OnEnable()
    {
        onCarSlowdown.OnEventRaised += ShowSkidMarks;
        onGameOver.OnEventRaised += DisplayCrater;
        onGameStart.OnEventRaised += StartDriftSmokes;
        onGameCameraBlendFinished.OnEventRaised += StopDriftSmokes;
    }

    private void Update()
    {
        if(!isGameStarted) {
            return;
        }

        if (isCarDrifting.CurrentValue)
        {
            driftSmokeParticles.Play();
        }
        else
        {
            driftSmokeParticles.Stop();
        }
    }

    private void ShowSkidMarks()
    {
        StartCoroutine(ShowSkidMarksCoroutine());
    }

    private void StartDriftSmokes()
    {
        driftSmokeParticles.Play();
    }

    private void StopDriftSmokes()
    {
        isGameStarted = true;
        driftSmokeParticles.Stop();
    }
    

    private IEnumerator ShowSkidMarksCoroutine()
    {
        ToggleSkidMarks(true);
        yield return Helpers.GetWait(1.05f);
        ToggleSkidMarks(false);
    }

    private void ToggleSkidMarks(bool isEmitting)
    {
        foreach (Transform item in skidMarks.transform)
        {
            item.GetComponent<TrailRenderer>().emitting = isEmitting;
        }
    }

    public void OnDrive(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }

    private void DisplayCrater()
    {
        crater.SetActive(true);
        carMesh.SetActive(false);
    }

    private void OnDisable()
    {
        onCarSlowdown.OnEventRaised -= ShowSkidMarks;
        onGameOver.OnEventRaised -= DisplayCrater;
        onGameStart.OnEventRaised -= StartDriftSmokes;
        onGameStart.OnEventRaised -= StopDriftSmokes;
    }
}
