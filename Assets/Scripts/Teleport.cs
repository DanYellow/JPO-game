using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Teleport : MonoBehaviour
{
    private Animator animator;

    private Vector2 startingPosition;
    private bool isTeleporting = false;
    [SerializeField]
    private int distanceBeforeTeleport = 15;

    [SerializeField]
    private UnityEvent OnBegin, OnDone;


    private void Awake()
    {
        animator = GetComponent<Animator>();

        startingPosition = transform.position;
    }

    private void Update() {
        if (Vector2.Distance(startingPosition, transform.position) > distanceBeforeTeleport && !isTeleporting)
        {
            StartCoroutine(ReturnToStartPoint());
        }
    }

    IEnumerator ReturnToStartPoint()
    {
        OnBegin?.Invoke();
        isTeleporting = true;
        animator.SetTrigger(AnimationStrings.teleportIn);
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        transform.position = startingPosition;
        animator.SetTrigger(AnimationStrings.teleportOut);
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        isTeleporting = false;
        OnDone?.Invoke();
    }
}
