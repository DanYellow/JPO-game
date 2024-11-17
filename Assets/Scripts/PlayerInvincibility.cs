using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvincibility : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    private List<int> listLayersIndexes = new List<int>();

    private LayerMask damageLayer;

    private bool isInvincible = false;

    private void Awake()
    {
        damageLayer = playerData.damageLayer;
        damageLayer &= ~(1 << LayerMask.NameToLayer($"WaveEffect{playerData.id.ToString()}"));

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        CreateListLayers();
        ToggleCollisions(gameObject.layer, isInvincible);
    }

    private void CreateListLayers()
    {
        for (int i = 0; i < 32; i++)
        {
            if (damageLayer == (damageLayer | (1 << i)))
            {
                listLayersIndexes.Add(i);
            }
        }
    }

    public IEnumerator Invincible(float? duration)
    {
        if (isInvincible)
        {
            yield break;
        }

        isInvincible = true;
        ToggleCollisions(gameObject.layer, isInvincible);

        float invincibilityTime = duration ?? playerData.root.invincibilityTime;
        float timeElapsed = 0;

        while (timeElapsed < invincibilityTime)
        {
            timeElapsed += Time.deltaTime;
            if (Time.frameCount % 8 == 0)
            {
                if (spriteRenderer.material.GetFloat("_Alpha") == 1)
                {
                    spriteRenderer.material.SetFloat("_Alpha", 0);
                }
                else
                {
                    spriteRenderer.material.SetFloat("_Alpha", 1);
                }
            }

            yield return null;
        }

        spriteRenderer.material.SetFloat("_Alpha", 1);
        isInvincible = false;
        ToggleCollisions(gameObject.layer, isInvincible);
    }

    public void Winner()
    {
        ToggleCollisions(gameObject.layer, true);
    }

    private void ToggleCollisions(int gameObjectLayer, bool enabled)
    {
        foreach (var layerIndex in listLayersIndexes)
        {
            Physics.IgnoreLayerCollision(
                gameObjectLayer,
                layerIndex,
                enabled
            );
        }
    }
}
