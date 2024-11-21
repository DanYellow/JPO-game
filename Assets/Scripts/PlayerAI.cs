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
    private float lastGroundPoundCooldown = 0;
    private float delayGroundPound = 2.75f;

    private void Awake()
    {
        playerControls = GetComponent<PlayerControls>();

        if (playerData.isCPU == false)
        {
            enabled = false;
        }
        liveFraction = (float)playerData.nbLives / playerData.root.maxNbLives;
    }

    IEnumerator Start()
    {
        yield return Helpers.GetWait(15);
        while (true)
        {
            if (playerControls.isGrounded && Random.value < Mathf.Lerp(0.8f, 0.5f, liveFraction) && Time.time - lastGroundPoundCooldown > delayGroundPound)
            {
                playerControls.Jump();
                StartCoroutine(DelayGroundPound());
            }
            yield return Helpers.GetWait(1.5f);

            yield return null;
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
        // for (int i = 0; i < hitColliders.Length; i++)
        // {
        if (hitColliders.Length != 0 && Random.value < Mathf.Lerp(0.45f, 0.15f, liveFraction))
        {
            playerControls.Jump();
            bool isCPU = hitColliders[0].transform.GetComponent<WaveEffectCollision>().playerData.isCPU;
            float highestProbability = isCPU ? 0.35f : 0.42f;
            float lowestProbability = isCPU ? 0.175f : 0.2f;

            if (Time.time - lastGroundPoundCooldown > delayGroundPound && Random.value < Mathf.Lerp(highestProbability, lowestProbability, liveFraction))
            {
                StartCoroutine(DelayGroundPound());
            }
        }
        // Debug.Log(hitColliders[i].transform.name);
        // Debug.Log();
        // }
    }

    private IEnumerator DelayGroundPound()
    {
        lastGroundPoundCooldown = Time.time;
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
