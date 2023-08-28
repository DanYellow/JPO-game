using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField]
    private Transform destination;

    [SerializeField]
    private VectorEventChannel onPlayerMoveEvent;

    [SerializeField]
    private BoolEventChannel onToggleCinemachineEvent;

    [SerializeField]
    private Animator animatorFX;
    
    private void OnEnable() {
        onPlayerMoveEvent.OnEventRaised += CheckPlayerPosition;
        animatorFX = GetComponentInChildren<Animator>(true);
    }

    private void CheckPlayerPosition(Vector3 pos) {
        if(Vector3.Distance(pos, destination.position) < 0.1f) {
            StartCoroutine(RenableVCams());
            // onToggleCinemachineEvent.Raise(true);
        }
    }

    IEnumerator RenableVCams() {
        yield return null;
        onToggleCinemachineEvent.Raise(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animatorFX.SetTrigger(AnimationStrings.disabled);
            onToggleCinemachineEvent.Raise(false);
            other.transform.position = destination.position;
        }
    }

    private void OnDisable() {
        onPlayerMoveEvent.OnEventRaised -= CheckPlayerPosition;
    }
}
