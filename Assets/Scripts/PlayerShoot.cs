using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private BoolEventChannel isShootingEventChannel;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    private LineRenderer lineRenderer;
    public GameObject impactEffect;

    private float nextShootTime = 0f;

    public LayerMask collisionLayers;

    public Transform firePoint;

    private Vector3 moveInput;

    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.useWorldSpace = true;
    }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed && Time.time >= nextShootTime)
        {
            isShootingEventChannel.Raise(ctx.phase == InputActionPhase.Performed);
            StartCoroutine(DetectHit());
            nextShootTime = Time.time + playerStatsValue.shootingRate;
        }
    }

    IEnumerator DetectHit()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, playerStatsValue.beamLength, collisionLayers);
        List<Vector3> listPositions = new List<Vector3>();

        if (hitInfo)
        {
            listPositions.Add(firePoint.position);
            listPositions.Add(hitInfo.point);

            if (hitInfo.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
            {
                if (iDamageable.isInvulnerable == true)
                {
                    // listPositions.Add(Vector3.Reflect(transform.right.normalized, hitInfo.normal));
                }
                else
                {
                    iDamageable.TakeDamage(playerStatsValue.damage);
                }
            }

            if (hitInfo.transform.TryGetComponent<IOpenable>(out IOpenable iOpenable))
            {
                iOpenable.Open();
            }

            if (hitInfo.transform.TryGetComponent<IPushable>(out IPushable iPushable))
            {
                iPushable.HitDirection(hitInfo.normal);
            }

            foreach (var item in listPositions)
            {
                GameObject impact = Instantiate(impactEffect, item, Quaternion.identity);
                Destroy(impact, impact.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            }

            lineRenderer.positionCount = listPositions.Count;
            lineRenderer.SetPositions(listPositions.ToArray());
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * playerStatsValue.beamLength);
        }

        listPositions.Clear();

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.02f);

        lineRenderer.enabled = false;
    }
}
