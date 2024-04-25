using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class CarEffects : MonoBehaviour
{
    [SerializeField]
    private GameObject skidMarks;

    private Vector3 moveInput = Vector3.zero;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onCarSlowdown;

    private void OnEnable()
    {
        onCarSlowdown.OnEventRaised += ShowSkidMarks;
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

    private void OnDisable()
    {
        onCarSlowdown.OnEventRaised -= ShowSkidMarks;
    }
}
