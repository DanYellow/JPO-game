using System.Collections;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    private Material originalMaterial;
    private SpriteRenderer sr;

    private float currentTime;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    public void ChangeMaterialProxy(MaterialChangeValue materialChange)
    {
        StartCoroutine(ChangeMaterial(materialChange));
    }

    IEnumerator ChangeMaterial(MaterialChangeValue materialChange)
    {
        currentTime = 0;
        StartCoroutine(StartTimer(materialChange.duration));
        sr.material = originalMaterial;

        Color targetColor = sr.color;
        targetColor.a = materialChange.opacity;

        Color originalColor = sr.color;

        while (currentTime <= materialChange.duration)
        {
            sr.material = originalMaterial;
            sr.color = originalColor;
            yield return Helpers.GetWait(materialChange.interval);
            sr.material = materialChange.material;
            sr.color = targetColor;
     
            yield return Helpers.GetWait(materialChange.interval);
        }

        // originalColor.a = 1;
        sr.color = originalColor;
        sr.material = originalMaterial;
        StopAllCoroutines();
    }

    IEnumerator StartTimer(float duration)
    {
        while (true)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
