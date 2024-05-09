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

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onCarSlowdown;

    [SerializeField]
    private VoidEventChannel onGameOver;

    private void Awake() {
        crater.SetActive(false);
        carMesh.SetActive(true);
    }

    private void OnEnable()
    {
        onCarSlowdown.OnEventRaised += ShowSkidMarks;
        onGameOver.OnEventRaised += DisplayCrater;
    }

    private void ShowSkidMarks()
    {
        StartCoroutine(ShowSkidMarksCoroutine());
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

    private void DisplayCrater() {
        crater.SetActive(true);
        carMesh.SetActive(false);
    }

    private void OnDisable()
    {
        onCarSlowdown.OnEventRaised -= ShowSkidMarks;
        onGameOver.OnEventRaised -= DisplayCrater;
    }
}
