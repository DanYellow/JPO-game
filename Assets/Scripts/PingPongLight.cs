using UnityEngine;

public class PingPongLight : MonoBehaviour
{
    private Light lighting;

    [SerializeField, ColorUsage(true, true)]
    private Color endColor;

    [SerializeField, Range(1f, 5f)]
    private float speed;

    [SerializeField, ColorUsage(true, true)]
    private Color startColor;

    private void Awake()
    {
        lighting = GetComponent<Light>();
    }

    void Update()
    {
        float pingPong = Mathf.PingPong(Time.time * (1f / speed), 1f);

        Color color = Color.Lerp(startColor, endColor, pingPong);
        lighting.color = color;
    }

    private void OnDisable()
    {
        lighting.color = startColor;
    }
}
