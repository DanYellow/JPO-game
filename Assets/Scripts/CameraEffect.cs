using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraEffect : MonoBehaviour
{
    [SerializeField]
    private Material material;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Debug.Log("Graphics 2");
        if (material == null) {
            Graphics.Blit(src, dest);
            return;
        }
        Graphics.Blit(src, dest, material);
    }
}