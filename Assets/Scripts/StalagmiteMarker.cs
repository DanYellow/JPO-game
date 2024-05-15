using System.Collections;
using UnityEngine;

public class StalagmiteMarker : MonoBehaviour
{
    [SerializeField]
    private Transform ground;

    // IEnumerator Start()
    // {
    //     transform.localScale = Vector3.zero;

    //     Vector3 startScale = transform.localScale;

    //     float current = 0;
    //     float duration = 1.55f;

    //     transform.LookAt(ground);
    //     transform.RotateAround(transform.position, transform.right, -90);

    //     while (current <= 1)
    //     {
    //         transform.localScale = Vector3.Lerp(startScale, Vector3.one, current);
    //         current += Time.deltaTime / duration;

    //         yield return null;
    //     }
    // }

    private void OnEnable()
    {
        StartCoroutine(Appear());
    }

    public IEnumerator Appear()
    {
        transform.localScale = Vector3.zero;

        Vector3 startScale = transform.localScale;

        float current = 0;
        float duration = 0.85f;

        transform.LookAt(ground);
        transform.RotateAround(transform.position, transform.right, -90);

        while (current <= 1)
        {
            transform.localScale = Vector3.Lerp(startScale, Vector3.one, current);
            current += Time.deltaTime / duration;

            yield return null;
        }
        yield return Helpers.GetWait(0.5f);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
    }
}
