using System.Collections;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    [SerializeField, Header("Scriptable Objects")]
    PlayerData playerData;

    [SerializeField]
    private PlayerIDEventChannel onPlayerHitEvent;

    private PlayerControls playerControls;

    private Collider[] hitColliders;

    private float liveFraction = 0;

    private void Awake()
    {
        playerControls = GetComponent<PlayerControls>();

        if (playerData.isCPU == false)
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        onPlayerHitEvent.OnEventRaised += OnPlayerHit;
    }

    private void OnPlayerHit(PlayerID playerID)
    {
        if (playerID == playerData.id)
        {
            liveFraction = (float)playerData.nbLives / playerData.root.maxNbLives;
        }
    }

    private void FixedUpdate()
    {
        if (!playerControls.isGrounded)
        {
            return;
        }

        hitColliders = Physics.OverlapSphere(transform.position, playerData.root.incomingAttackRadius, playerData.damageLayer);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (Random.value < Mathf.Lerp(0.95f, 0.2f, liveFraction))
            {
                playerControls.Jump();
                if (Random.value < Mathf.Lerp(0.6f, 0.33f, liveFraction))
                {
                    StartCoroutine(DelayGroundPound());
                }
            }
            // Debug.Log(hitColliders[i].transform.name);
        }
    }

    private IEnumerator DelayGroundPound()
    {
        float duration = Mathf.Lerp(
            0.35f,
            Mathf.Lerp(0.85f, 1.1f, liveFraction),
        Random.value);
        yield return new WaitForSeconds(duration);
        playerControls.GroundPound();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerData.root.incomingAttackRadius);
    }

    private void OnDisable()
    {
        onPlayerHitEvent.OnEventRaised -= OnPlayerHit;
    }
}
