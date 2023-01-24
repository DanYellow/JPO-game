using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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

    private void Awake() {
        lineRenderer = GetComponentInChildren<LineRenderer>();
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
            if(hitInfo.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
                iDamageable.TakeDamage(playerStatsValue.damage);
            }

            if(hitInfo.transform.TryGetComponent<IOpenable>(out IOpenable iOpenable)) {
                iOpenable.Open();
            }

            GameObject impact = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            Destroy(impact, impact.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); 

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
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
