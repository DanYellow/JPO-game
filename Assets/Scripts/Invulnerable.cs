using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invulnerable : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator animator;
    private bool isInvulnerable = false;
    [SerializeField]
    private InvulnerableDataValue invulnerableDataValue;
    [SerializeField]
    private LayerMask listLayerToIgnoreAfterHit;
    private List<int> listLayers = new List<int>();

    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

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

    private void OnCollision() {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        LayerMask otherLayer = other.gameObject.layer;
        bool isInLayer = ((listLayerToIgnoreAfterHit & (1 << otherLayer)) != 0);

        if (!isInvulnerable && isInLayer)
        {
            Debug.Log("ggggz");
            StartCoroutine(HandleInvunlnerableDelay(otherLayer.value));
            StartCoroutine(InvunlnerableFlash());
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

    public IEnumerator InvunlnerableFlash()
    {
        while (isInvulnerable)
        {
            sr.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invulnerableDataValue.flashDelay);
            sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invulnerableDataValue.flashDelay);
        }

        // Hack to reenable OnTriggerEnter/Stay methods
        gameObject.transform.position = new Vector3(
            gameObject.transform.position.x + 0.001f,
            gameObject.transform.position.y
        );
    }

    private void OnDestroy()
    {
        isHurtVoidEventChannel.OnEventRaised -= OnCollision;
    }
}
