using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invulnerable : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool isInvulnerable = false;
    [SerializeField]
    private InvulnerableDataValue invulnerableDataValue;
    [SerializeField]
    private LayerMask layersToIgnoreAfterHit;
    private List<int> listLayers = new List<int>();

    [SerializeField]
    private BoolEventChannel onHealthUpdated;


    [SerializeField]
    private MaterialEventChannel onMaterialChange;

    [SerializeField]
    private MaterialChangeValue materialChange;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        CheckMasks();

        onHealthUpdated.OnEventRaised += OnCollision;

        
    }

    private void DisableCollisions(bool enabled)
    {
        foreach (var layerIndex in listLayers)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, layerIndex, enabled);
        }
    }


    private void CheckMasks()
    {
        for (int i = 0; i < 32; i++)
        {
            if (layersToIgnoreAfterHit == (layersToIgnoreAfterHit | (1 << i)))
            {
                listLayers.Add(i);
            }
        }
    }

    private void OnCollision(bool isTakingDamage)
    {
        if (isTakingDamage && !isInvulnerable)
        {
            if (!isInvulnerable)
            {
                StartCoroutine(HandleInvunlnerableDelay());
                onMaterialChange.Raise(materialChange);
            }
        }
    }


    public IEnumerator HandleInvunlnerableDelay()
    {
        DisableCollisions(true);
        // Physics2D.IgnoreLayerCollision(gameObject.layer, layerId, true);
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerableDataValue.duration);
        isInvulnerable = false;
        DisableCollisions(false);
        // Physics2D.IgnoreLayerCollision(gameObject.layer, layerId, false);
    }

    private void OnDisable()
    {
        onHealthUpdated.OnEventRaised -= OnCollision;
    }
}
