using UnityEngine;

public class PlayerIA : MonoBehaviour
{
    [SerializeField, Header("Scriptable Objects")]
    PlayerData playerData;

    private PlayerControls playerControls;

    Collider[] hitColliders;

    private void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
    }
    private void FixedUpdate()
    {
        // float radiusDetection = Mathf.Lerp(0.85f, 1.85f, Random.value);
        hitColliders = Physics.OverlapSphere(transform.position, 1.55f, playerData.damageLayer);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (Random.value < 1.2f)
            {
                playerControls.Jump();
                if (Random.value < 1.2f)
                {
                    playerControls.GroundPound();
                }
            }
            // Debug.Log(hitColliders[i].transform.name);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.85f);
    }
}
