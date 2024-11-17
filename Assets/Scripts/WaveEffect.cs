using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void SetTrackersPosition(PlayerID playerID)
    {
        int nbColliders = 4;
        List<Vector3> pos = new List<Vector3>();

        for (int i = 0; i <= nbColliders; i += 2)
        {
            float val = Mathf.Lerp(0, -Mathf.PI / 2, (float)i / nbColliders);

            switch (playerID)
            {
                case PlayerID.Player2:
                    val = Mathf.Lerp(Mathf.PI, 3 * Mathf.PI / 2, (float)i / nbColliders);
                    break;
                case PlayerID.Player3:
                    val = Mathf.Lerp(Mathf.PI / 2, Mathf.PI, (float)i / nbColliders);
                    break;
                case PlayerID.Player4:
                    val = Mathf.Lerp(0, Mathf.PI / 2, (float)i / nbColliders);
                    break;
                default:
                    break;
            }

            var vertical = Mathf.Sin(val);
            var horizontal = Mathf.Cos(val);

            var spawnDir = new Vector3(horizontal, 0, vertical);
            pos.Add(spawnDir);
        }

        for (var i = 0; i < pos.Count; i++)
        {
            listTrackers[i].transform.position = transform.position + pos[i] * (meshRenderer.bounds.size.x / 2);
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

    public Transform[] GetTrakers(PlayerID playerID)
    {
        SetTrackersPosition(playerID);

        return listTrackers;
    }
}
