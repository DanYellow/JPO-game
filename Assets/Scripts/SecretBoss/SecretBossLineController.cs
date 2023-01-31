using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
