using System.Collections;
using UnityEngine;

public class WaveEffect : MonoBehaviour
{
    [SerializeField, Range(0, 5)]
    private float duration = 2;

    private IEnumerator Start() {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = new Vector3(4, 4, 4);

        float t = 0;

        while(t < 1)
        {
            t += Time.smoothDeltaTime / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        transform.localScale = endScale;
    }
}
