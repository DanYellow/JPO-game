using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.aleksandrhovhannisyan.com/blog/invulnerability-frames-in-unity/

public class Invulnerable : MonoBehaviour
{
    [field: SerializeField]
    public bool isInvulnerable { private set; get; } = false;
    [SerializeField]
    private InvulnerableDataValue invulnerableDataValue;
    [SerializeField]
    private LayerMask layersToIgnoreAfterHit;
    private List<int> listLayers = new List<int>();

    private MaterialChangeValue materialChange;

    private MaterialManager materialManager;

    private void Awake()
    {
        materialManager = GetComponent<MaterialManager>();

        materialChange = ScriptableObject.CreateInstance<MaterialChangeValue>();
        materialChange.material = invulnerableDataValue.material;
        materialChange.interval = invulnerableDataValue.flashDelay;
        materialChange.duration = invulnerableDataValue.duration;
    }


    // Start is called before the first frame update
    private void Start()
    {
        CreateListLayers();
    }

    private void CreateListLayers()
    {
        for (int i = 0; i < 32; i++)
        {
            if (layersToIgnoreAfterHit == (layersToIgnoreAfterHit | (1 << i)))
            {
                listLayers.Add(i);
            }
        }
    }

    public void Enable()
    {
        Helpers.DisableCollisions(LayerMask.LayerToName(gameObject.layer), listLayers, true);
    }

    public void Disable()
    {
        Helpers.DisableCollisions(LayerMask.LayerToName(gameObject.layer), listLayers, false);
    }

    public void Trigger()
    {
        StopAllCoroutines();
        StartCoroutine(HandleInvunlnerableDelay());
    }

    private IEnumerator HandleInvunlnerableDelay()
    {
        isInvulnerable = true;

        Helpers.DisableCollisions(LayerMask.LayerToName(gameObject.layer), listLayers, isInvulnerable);
        float invincibilityDeltaTime = 0.15f;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Material originalMaterial = sr.material;
        Color targetColor = sr.color;
        targetColor.a = materialChange.opacity;

        Color originalColor = sr.color;

        for (float i = 0; i < invulnerableDataValue.duration; i += invincibilityDeltaTime)
        {
            if (sr.material == originalMaterial)
            {
                sr.material = materialChange.material;
                sr.color = originalColor;
            }
            else
            {
                sr.material = originalMaterial;
                sr.color = targetColor;
            }

            yield return Helpers.GetWait(invincibilityDeltaTime);
        }

        sr.color = originalColor;
        sr.material = originalMaterial;
        isInvulnerable = false;
        Helpers.DisableCollisions(LayerMask.LayerToName(gameObject.layer), listLayers, isInvulnerable);
    }
}
