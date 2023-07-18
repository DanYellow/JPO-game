using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterialManager : MonoBehaviour
{
    private Material originalMaterial;
    private SpriteRenderer sr;

    [SerializeField]
    private MaterialEventChannel onMaterialChange;

    private float currentTime;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
    }
    
    private void OnEnable() {
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
        while (currentTime < materialChange.duration)
        {
            sr.material = materialChange.material;
            yield return new WaitForSeconds(materialChange.interval);
            sr.material = originalMaterial;
            yield return new WaitForSeconds(materialChange.interval);
        }
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
