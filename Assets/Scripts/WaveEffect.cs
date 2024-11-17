using UnityEngine;

public class WaveEffect : MonoBehaviour
{
    [SerializeField, Range(0, 10)]
    private float speed = 7;

    // Update is called once per frame
    void Update()
    {
        transform.localScale += Vector3.one * Time.deltaTime * speed;
    }
}
