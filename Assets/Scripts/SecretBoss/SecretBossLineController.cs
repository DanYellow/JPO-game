using UnityEngine;

public class SecretBossLineController : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField]
    private Texture[] listTextures;

    private int animationStep;

    [SerializeField]
    private float fps = 30f;
    private float fpsCounter = 0;

    public bool hideOnAwake = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if(hideOnAwake) {
            lineRenderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 0));
        }

    }

    void Update()
    {
        fpsCounter += Time.deltaTime;
        if (fpsCounter >= 1f / fps)
        {
            animationStep++;
            if(animationStep == listTextures.Length) {
                animationStep = 0;
            }
            lineRenderer.material.SetTexture("_MainTex", listTextures[animationStep]);
            fpsCounter = 0f;
        }
    }
}
