using System.Collections;
using UnityEngine;

public class GateManager : MonoBehaviour, IOpenable
{
    private Collider2D collider2d;
    private Animator[] listGatesAnimator;
    private SpriteRenderer[] listGatesSpriteRenderer;
    private IEnumerator autocloseRef;
    private IEnumerator closeRef;

    public bool isDisabled = false;

    private Color colorDisabled;

    private void Awake()
    {
        collider2d = GetComponent<BoxCollider2D>();
        listGatesAnimator = GetComponentsInChildren<Animator>();
        listGatesSpriteRenderer = GetComponentsInChildren<SpriteRenderer>();

        ColorUtility.TryParseHtmlString("#8E8E8E", out colorDisabled);
        foreach (SpriteRenderer sr in listGatesSpriteRenderer)
            sr.color = isDisabled ? colorDisabled : Color.white;
        closeRef = ToggleOpening(false);
        ToggleDisable(isDisabled);

    }

    public void Open()
    {
        if (!isDisabled)
        {
            StartCoroutine(ToggleOpening(true));
            autocloseRef = Autoclose();
            StartCoroutine(autocloseRef);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopCoroutine(closeRef);
            StopCoroutine(autocloseRef);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            StopCoroutine(closeRef);
            StopCoroutine(autocloseRef);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            closeRef = ToggleOpening(false);
            StartCoroutine(closeRef);
        }
    }

    IEnumerator ToggleOpening(bool isOpening)
    {
        if (isOpening)
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }

        yield return new WaitForSeconds(0.5f);

        foreach (Animator animator in listGatesAnimator)
            animator.SetBool("IsOpen", isOpening);
        collider2d.isTrigger = isOpening;
    }

    IEnumerator Autoclose()
    {
        yield return new WaitForSeconds(3f);

        StartCoroutine(ToggleOpening(false));
    }

    private void ToggleDisable(bool _isDisabled)
    {
        isDisabled = _isDisabled;

        foreach (SpriteRenderer sr in listGatesSpriteRenderer)
            sr.color = new Color(1, 1, 1, isDisabled ? 0.5f : 1f);
    }
}
