using System.Collections;
using UnityEngine;

public class PlayerMaterialManager : MonoBehaviour
{
    private Material originalMaterial;
    private float originalAlpha;
    private SpriteRenderer sr;

    [SerializeField]
    private MaterialEventChannel onMaterialChange;

    private float currentTime;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
        originalAlpha = sr.color.a;
    }

    private void OnEnable()
    {
        onMaterialChange.OnEventRaised += ChangeMaterialProxy;
    }

    private void ChangeMaterialProxy(MaterialChangeValue materialChange)
    {
        StartCoroutine(ChangeMaterial(materialChange));
    }

    IEnumerator ChangeMaterial(MaterialChangeValue materialChange)
    {
        currentTime = 0;
        StartCoroutine(StartTimer());
        sr.material = originalMaterial;

        Color targetColor = sr.color;
        targetColor.a = materialChange.opacity;

        Color originalColor = sr.color;

        WaitForSeconds intervalMaterialChange = new WaitForSeconds(materialChange.interval);
        while (currentTime < materialChange.duration)
        {
            sr.material = materialChange.material;
            sr.color = targetColor;
            yield return intervalMaterialChange;
            sr.material = originalMaterial;
            sr.color = originalColor;
            yield return intervalMaterialChange;
        }
        originalColor.a = 1;
        sr.color = originalColor;
        sr.material = originalMaterial;
        StopAllCoroutines();
    }

    IEnumerator StartTimer()
    {
        while (true)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDisable()
    {
        onMaterialChange.OnEventRaised -= ChangeMaterialProxy;
    }
}
