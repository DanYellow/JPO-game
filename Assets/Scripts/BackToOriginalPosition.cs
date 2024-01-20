using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BackToOriginalPosition : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;

    private Vector2 startingPosition;
    private bool isTeleporting = false;
    [SerializeField]
    private int distanceBeforeTeleport = 15;

    [SerializeField]
    private float delay = 7;
    private float timer;

    [SerializeField]
    private UnityEvent OnBegin, OnDone;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        startingPosition = transform.position;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (Vector2.Distance(startingPosition, transform.position) > distanceBeforeTeleport && !isTeleporting && timer > delay)
        {
            StartCoroutine(Return());
        }
    }

    IEnumerator Return()
    {
        timer = 0;
        OnBegin?.Invoke();
        isTeleporting = true;
        yield return Helpers.GetWait(timer * 1.5f);
        animator.SetTrigger(AnimationStrings.teleportIn);
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        sr.color = Color.clear;
        transform.position = startingPosition;
        yield return Helpers.GetWait(0.85f);
        sr.color = Color.white;
        animator.SetTrigger(AnimationStrings.teleportOut);
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        isTeleporting = false;
        OnDone?.Invoke();
    }
}
