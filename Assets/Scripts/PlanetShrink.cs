using System.Collections;
using UnityEngine;

public class PlanetShrink : MonoBehaviour
{
    public float shrinkSpeed = .05f;
    private float initialScale = 0;

    [Header("Scriptable Objects")]
    [SerializeField]
    private BoolValue hasReachMinimumTravelDistance;

    IEnumerator Start()
    {
        initialScale = transform.localScale.x;
        yield return new WaitUntil(() => hasReachMinimumTravelDistance.CurrentValue == true);
        while (true)
        {
            Vector3 newScale = new Vector3(
            Mathf.Clamp(transform.localScale.x - (shrinkSpeed * Time.deltaTime), initialScale / 3, initialScale),
            Mathf.Clamp(transform.localScale.y - (shrinkSpeed * Time.deltaTime), initialScale / 3, initialScale),
            Mathf.Clamp(transform.localScale.z - (shrinkSpeed * Time.deltaTime), initialScale / 3, initialScale)
        );

            transform.localScale = newScale;

            yield return null;
        }
    }
}
