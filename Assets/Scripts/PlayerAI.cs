using System.Collections;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    [SerializeField, Header("Scriptable Objects")]
    PlayerData playerData;

    private PlayerControls playerControls;

    private Collider[] hitColliders;
    private bool isGroundPounding = false;

    private void Awake()
    {
        playerControls = GetComponent<PlayerControls>();

        if (playerData.isCPU == false)
        {
            enabled = false;
        }
    }
    private void FixedUpdate()
    {
        if (isGroundPounding)
        {
            return;
        }
        // float radiusDetection = Mathf.Lerp(0.85f, 1.85f, Random.value);
        hitColliders = Physics.OverlapSphere(transform.position, playerData.root.incomingAttackRadius, playerData.damageLayer);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (Random.value < 0.2f)
            {
                playerControls.Jump();
                if (Random.value < 1.2f && !isGroundPounding)
                {
                    StartCoroutine(DelayGroundPound());
                }
            }
            // Debug.Log(hitColliders[i].transform.name);
        }
    }

    private IEnumerator DelayGroundPound()
    {
        isGroundPounding = true;
        float duration = Mathf.Lerp(0.15f, 1.05f, Random.value);
        yield return new WaitForSeconds(duration);
        playerControls.GroundPound();
        isGroundPounding = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerData.root.incomingAttackRadius);
    }
}
