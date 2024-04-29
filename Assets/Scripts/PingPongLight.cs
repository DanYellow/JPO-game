using UnityEngine;

public class PingPongLight : MonoBehaviour
{
    private Light lighting;

    [SerializeField, ColorUsage(true, true)]
    private Color endColor;

    [SerializeField, Range(1f, 5f)]
    private float speed;

    private Color startColor;

    private void Awake()
    {
        lighting = GetComponent<Light>();
        startColor = lighting.color;
    }

    void Update()
    {
        float pingPong = Mathf.PingPong(Time.time * (1 / speed), 1);
        Color color = Color.Lerp(startColor, endColor, pingPong);
        lighting.color = color;
    }

    private void OnDisable()
    {
        lighting.color = startColor;
    }
}
