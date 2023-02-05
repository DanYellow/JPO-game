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

        if (hitInfo)
        {
            List<Vector3> listPositions = new List<Vector3>();
            listPositions.Add(firePoint.position);
            listPositions.Add(hitInfo.point);

            if (hitInfo.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
            {
                iDamageable.TakeDamage(playerStatsValue.damage);
                if(iDamageable.isInvulnerable == true) {
                    // Debug.Log("fffe");
                    // listPositions.Add(Vector3.zero);
                    // listPositions.Add(Vector3.Reflect(hitInfo.point, hitInfo.normal));
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

            GameObject impact = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            Destroy(impact, impact.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            
            lineRenderer.positionCount = listPositions.Count;
            lineRenderer.SetPositions(listPositions.ToArray());
            listPositions.Clear();
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * playerStatsValue.beamLength);
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.02f);

        lineRenderer.enabled = false;
    }
}
