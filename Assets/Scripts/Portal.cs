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

    private void OnEnable()
    {
        onPlayerMoveEvent.OnEventRaised += CheckPlayerPosition;
        animatorFX = GetComponentInChildren<Animator>(true);
    }

    private void CheckPlayerPosition(Vector3 pos)
    {
        if (Vector3.Distance(pos, destination.position) < 0.1f)
        {
            StartCoroutine(RenableVCams());
        }
    }

    IEnumerator RenableVCams()
    {
        yield return 0.3f;
        onToggleCinemachineEvent.Raise(true);
        animatorFX.ResetTrigger(AnimationStrings.disabled);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animatorFX.SetTrigger(AnimationStrings.disabled);
            other.transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
            print(other.gameObject.name);
            StartCoroutine(Teleport(other.gameObject));
        }
    }

    IEnumerator Teleport(GameObject target)
    {
        yield return null;
        yield return new WaitForSeconds(animatorFX.GetCurrentAnimatorStateInfo(0).length);
        onToggleCinemachineEvent.Raise(false);
        target.transform.position = destination.position;
        target.transform.GetComponentInChildren<SpriteRenderer>().enabled = true;
        StartCoroutine(RenableVCams());
    }

    private void OnDisable()
    {
        onPlayerMoveEvent.OnEventRaised -= CheckPlayerPosition;
    }
}
