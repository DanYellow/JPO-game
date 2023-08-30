using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField]
    private Transform destination;

    [SerializeField]
    private VectorEventChannel onPlayerMoveEvent;

    private bool isPlayerInRange = false;

    [SerializeField]
    private GameObject portalFX;

    [SerializeField]
    private float fxMaxScale = 2.2f;
    private float fxMinScale = 0.5f;

    private float holdDuration = 0.3f;

    private Animator animatorFX;

    [SerializeField, TextArea]
    private string interactionText;
// Appuyez sur <sprite="touches-spritesheet" anim="0, 1, 2" tint=1> pour interagir
    [Header("Events")]
    [SerializeField]
    private BoolEventChannel onToggleCinemachineEvent;


    [SerializeField]
    private VoidEventChannel onPlayerStartInteractEvent;

    [SerializeField]
    private VoidEventChannel onCancelInteractEventChannel;

    [SerializeField]
    private BoolEventChannel onInteractRangeEvent;

    [SerializeField]
    private StringEventChannel onInteract;


    private void Awake()
    {
        animatorFX = portalFX.GetComponent<Animator>();
        portalFX.transform.localScale = new Vector3(fxMinScale, fxMinScale, fxMinScale);
    }

    private void OnEnable()
    {
        onPlayerMoveEvent.OnEventRaised += CheckPlayerPosition;
        onPlayerStartInteractEvent.OnEventRaised += StartTeleport;
        onCancelInteractEventChannel.OnEventRaised += CancelTeleport;
    }

    private void CancelTeleport()
    {
        if (isPlayerInRange)
        {
            StopAllCoroutines();
            StartCoroutine(DecreasePortal());
        }
    }

    private void StartTeleport()
    {
        if (isPlayerInRange)
        {
            StopAllCoroutines();
            StartCoroutine(IncreasePortal());
        }
    }

    IEnumerator IncreasePortal()
    {
        float currentTime = 0;
        while (portalFX.transform.localScale.x < fxMaxScale)
        {
            currentTime += Time.deltaTime;
            portalFX.transform.localScale = new Vector3(
                Mathf.Clamp(currentTime / holdDuration, fxMinScale, fxMaxScale), 
                Mathf.Clamp(currentTime / holdDuration, fxMinScale, fxMaxScale), 
                Mathf.Clamp(currentTime / holdDuration, fxMinScale, fxMaxScale)
            );

            yield return null;
        }

        print("teleport");
    }

    IEnumerator DecreasePortal()
    {
        float currentTime = portalFX.transform.localScale.x / fxMaxScale;

        while (portalFX.transform.localScale.x > 0.1f)
        {
            currentTime -= Time.deltaTime * 2.25f;
            portalFX.transform.localScale = new Vector3(
                Mathf.Clamp(currentTime / holdDuration, fxMinScale, fxMaxScale), 
                Mathf.Clamp(currentTime / holdDuration, fxMinScale, fxMaxScale), 
                Mathf.Clamp(currentTime / holdDuration, fxMinScale, fxMaxScale)
            );

            yield return null;
        }

        print("Cancel teleport");
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
            isPlayerInRange = true;
            onInteractRangeEvent.Raise(isPlayerInRange);
            onInteract.Raise(interactionText);
            // animatorFX.SetTrigger(AnimationStrings.disabled);
            // other.transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
            // StartCoroutine(Teleport(other.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            onInteractRangeEvent.Raise(isPlayerInRange);

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
        onPlayerStartInteractEvent.OnEventRaised -= StartTeleport;
        onCancelInteractEventChannel.OnEventRaised -= CancelTeleport;
    }
}
