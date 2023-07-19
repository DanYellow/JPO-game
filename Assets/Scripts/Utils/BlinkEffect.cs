using UnityEngine;
using UnityEngine.UI;

public class BlinkEffect : MonoBehaviour
{
    private Color startColor;

    [SerializeField]
    private Color endColor = Color.white;

    [SerializeField]
    private float speed;

    private Image image;
    private SpriteRenderer sr;

    private void Awake()
    {
        image = GetComponent<Image>();
        sr = GetComponent<SpriteRenderer>();

        if (image != null)
        {
            startColor = image.color;
        }

        if (sr != null)
        {
            startColor = sr.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (image != null)
        {
            image.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
        }

        if (sr != null)
        {
            sr.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
        }
    }
}
