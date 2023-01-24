using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharge : MonoBehaviour
{
    private Vector3 offset;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    public EnemyStatsValue enemyData;

    [SerializeField]
    private bool isFacingRight = false;

    public LayerMask obstacleLayersMask;

    private void Awake()
    {
        // We don't want the script to be enabled by default
        enabled = false;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        offset = new Vector3(sr.bounds.extents.x * (isFacingRight ? -1 : 1), sr.bounds.extents.y, 0);
    }

    private void FixedUpdate() {
        Vector3 startCast = transform.position - new Vector3(offset.x, 0, 0);
        Vector3 endCast = transform.position + (isFacingRight ? Vector3.right : Vector3.left) * enemyData.sightLength;

        Debug.DrawLine(startCast, endCast, Color.white);

        RaycastHit2D hitObstacle = Physics2D.Linecast(startCast, endCast, obstacleLayersMask);

        if (hitObstacle.collider != null && hitObstacle.collider.gameObject.CompareTag("Player"))
        {
            rb.velocity += -hitObstacle.normal * 0.01f;
        }
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }
}
