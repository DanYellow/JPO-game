using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invulnerable : MonoBehaviour
{
    private bool isInvulnerable = false;
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

    private void DisableCollisions(bool enabled)
    {
        foreach (var layerIndex in listLayers)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, layerIndex, enabled);
        }
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

    public void Trigger()
    {
        if (!isInvulnerable)
        {
            StartCoroutine(HandleInvunlnerableDelay());
            materialManager.ChangeMaterialProxy(materialChange);
        }
    }

    public IEnumerator HandleInvunlnerableDelay()
    {
        isInvulnerable = true;
        DisableCollisions(true);
        yield return Helpers.GetWait(invulnerableDataValue.duration);
        isInvulnerable = false;
        DisableCollisions(false);
    }
}
