using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invulnerable : MonoBehaviour
{
    private bool isInvulnerable = false;
    [SerializeField]
    private InvulnerableDataValue invulnerableDataValue;
    [SerializeField]
    private LayerMask listLayerToIgnoreAfterHit;
    private List<int> listLayers = new List<int>();

    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    [SerializeField]
    private MaterialEventChannel onMaterialChange;

    [SerializeField]
    private MaterialChangeValue materialChange;


    // Start is called before the first frame update
    private void Start()
    {
        CheckMasks();

        isHurtVoidEventChannel.OnEventRaised += OnCollision;

        foreach (var layerIndex in listLayers)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, layerIndex, false);
        }
    }

    private void CheckMasks()
    {
        for (int i = 0; i < 32; i++)
        {
            if (listLayerToIgnoreAfterHit == (listLayerToIgnoreAfterHit | (1 << i)))
            {
                listLayers.Add(i);
            }
        }
    }

    private void OnCollision()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        LayerMask otherLayer = other.gameObject.layer;
        bool isInLayer = ((listLayerToIgnoreAfterHit & (1 << otherLayer)) != 0);

        if (!isInvulnerable && isInLayer)
        {
            StartCoroutine(HandleInvunlnerableDelay(otherLayer.value));
            onMaterialChange.Raise(materialChange);
        }
    }

    public IEnumerator HandleInvunlnerableDelay(int layerId)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, layerId, true);
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerableDataValue.time);
        isInvulnerable = false;
        Physics2D.IgnoreLayerCollision(gameObject.layer, layerId, false);
    }

    private void OnDestroy()
    {
        isHurtVoidEventChannel.OnEventRaised -= OnCollision;
    }
}
