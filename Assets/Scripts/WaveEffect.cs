using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=CVsZ98TSEwI
public class WaveEffect : MonoBehaviour
{
    [SerializeField, Range(0, 5)]
    private float duration = 2;

    private Vector3 startScale;

    [SerializeField]
    private Transform[] listTrackers;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(IncreaseSize());
    }

    private void Start()
    {
        startScale = transform.localScale;
    }

    private void SetTrackersPosition(List<(Vector3 Position, float RadiansAngle)> listDirections)
    {
        for (var i = 0; i < listDirections.Count; i++)
        {
            listTrackers[i].transform.position = transform.position + listDirections[i].Position * (meshRenderer.bounds.size.x / 2);
        }
    }

    private IEnumerator IncreaseSize()
    {
        Vector3 endScale = new Vector3(4, 4, 4);

        float t = 0;

        while (t < 1)
        {
            t += Time.smoothDeltaTime / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        transform.localScale = endScale;
        gameObject.SetActive(false);
    }

    public Transform[] GetTrakers(List<(Vector3 Position, float RadiansAngle)> listDirections)
    {
        SetTrackersPosition(listDirections);

        return listTrackers;
    }
}
