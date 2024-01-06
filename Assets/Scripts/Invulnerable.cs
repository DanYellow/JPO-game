using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invulnerable : MonoBehaviour
{
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
        materialManager.ChangeMaterialProxy(materialChange);
    }

    private IEnumerator HandleInvunlnerableDelay()
    {
        yield return Helpers.GetWait(invulnerableDataValue.delay);
        isInvulnerable = true;
        Helpers.DisableCollisions(LayerMask.LayerToName(gameObject.layer), listLayers, isInvulnerable);
        yield return Helpers.GetWait(invulnerableDataValue.duration);
        isInvulnerable = false;
        Helpers.DisableCollisions(LayerMask.LayerToName(gameObject.layer), listLayers, isInvulnerable);
    }
}
