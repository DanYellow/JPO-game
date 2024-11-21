using System.Collections;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    [SerializeField, Header("Scriptable Objects")]
    PlayerData playerData;

    [SerializeField]
    private PlayerIDEventChannel onPlayerHitEvent;

    [SerializeField]
    private PlayerIDEventChannel onPlayerDeathEvent;

    [SerializeField]
    private PlayerIDEventChannel onPlayerWinsEvent;

    private PlayerControls playerControls;

    private Collider[] hitColliders;

    private float liveFraction = 0;
    private float lastGroundPoundCooldown = 0;
    private float delayGroundPound = 5.1f;
    private float delayGroundPoundAggressityFactor = 1f;

    private float highestAttackProbability = 0;
    private float lowestAttackProbability = 0;
    private bool isGroundPounding = false;

    private void Awake()
    {
        playerControls = GetComponent<PlayerControls>();

        if (playerData.isCPU == false)
        {
            enabled = false;
        }
        liveFraction = (float)playerData.nbLives / playerData.root.maxNbLives;

        switch (playerData.agressivity)
        {
            case PlayerAgressivity.Medium:
                highestAttackProbability = 0.55f;
                lowestAttackProbability = 0.45f;
                delayGroundPoundAggressityFactor = 1f;
                break;
            case PlayerAgressivity.High:
                highestAttackProbability = 0.5f;
                lowestAttackProbability = 0.45f;
                delayGroundPoundAggressityFactor = 0.9f;
                break;
            default:
                highestAttackProbability = 0.15f;
                lowestAttackProbability = 0.35f;
                delayGroundPoundAggressityFactor = 1.1f;
                break;
        }

        delayGroundPound *= delayGroundPoundAggressityFactor;
    }

    IEnumerator Start()
    {
        yield return Helpers.GetWait(6.5f * delayGroundPoundAggressityFactor);
        while (true)
        {
            if (
                !isGroundPounding &&
                playerControls.isGrounded &&
                Random.value < Mathf.Lerp(highestAttackProbability, lowestAttackProbability, liveFraction)
            )
            {
                playerControls.Jump();
                yield return Helpers.GetWait(0.35f);
                playerControls.GroundPound();
                // StartCoroutine(DelayGroundPound());
            }
            yield return Helpers.GetWait(delayGroundPound);
            yield return null;
        }
    }

    private void OnEnable()
    {
        onPlayerHitEvent.OnEventRaised += OnPlayerHit;
        onPlayerDeathEvent.OnEventRaised += OnPlayerDeath;
        onPlayerWinsEvent.OnEventRaised += OnDisplayWinner;
    }

    private void OnPlayerHit(PlayerID playerID)
    {
        if (playerID == playerData.id)
        {
            liveFraction = (float)playerData.nbLives / playerData.root.maxNbLives;
        }
    }

    private void OnDisplayWinner(PlayerID playerID)
    {
        enabled = false;
    }

    private void OnPlayerDeath(PlayerID playerID)
    {
        if (playerID == playerData.id)
        {
            enabled = false;
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
        if (hitColliders.Length != 0 && Random.value < Mathf.Lerp(0.2f, 0.15f, liveFraction))
        {
            playerControls.Jump();
            bool isCPU = hitColliders[0].transform.GetComponent<WaveEffectCollision>().playerData.isCPU;
            float highestProbability = isCPU ? 0.10f : 0.13f;
            float lowestProbability = isCPU ? 0.05f : 0.09f;

            if (
                !isGroundPounding &&
                Time.time > delayGroundPound + lastGroundPoundCooldown &&
                Random.value < Mathf.Lerp(highestProbability, lowestProbability, liveFraction)
            )
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
        isGroundPounding = true;
        float duration = Mathf.Lerp(
            0.35f,
            Mathf.Lerp(0.85f, 1, liveFraction),
            Random.value
        );
        yield return new WaitForSeconds(duration);
        playerControls.GroundPound();
        lastGroundPoundCooldown = Time.time;
        isGroundPounding = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerData.root.incomingAttackRadius);
    }

    private void OnDisable()
    {
        onPlayerHitEvent.OnEventRaised -= OnPlayerHit;
        onPlayerDeathEvent.OnEventRaised -= OnPlayerDeath;
        onPlayerWinsEvent.OnEventRaised -= OnDisplayWinner;
    }
}
