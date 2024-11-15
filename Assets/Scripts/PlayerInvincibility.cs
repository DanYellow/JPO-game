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

    [SerializeField]
    private Player player;

    private void Awake()
    {
        damageLayer = playerData.damageLayer;
        damageLayer &= ~(1 << LayerMask.NameToLayer($"WaveEffect{player.ToString()}"));

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

    public IEnumerator Invincible()
    {
        if (isInvincible)
        {
            yield break;
        }

        isInvincible = true;

        ToggleCollisions(gameObject.layer, isInvincible);

        float timeElapsed = 0;
        while (timeElapsed < playerData.invincibilityTime)
        {
            timeElapsed += Time.deltaTime;
            if (Time.frameCount % 8 == 0)
            {
                if (spriteRenderer.color.a == 1)
                {
                    spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
                }
                else
                {
                    spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                }
            }

            yield return null;
        }

        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        isInvincible = false;
        ToggleCollisions(gameObject.layer, isInvincible);
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
