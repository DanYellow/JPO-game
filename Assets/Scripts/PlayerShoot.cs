using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    BoolEventChannel isShootingEventChannel;

    public LineRenderer lineRenderer;
    public GameObject impactEffect;

    public Transform firePoint;

    private Vector3 moveInput;

    private void Awake() {
        // lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            isShootingEventChannel.Raise(ctx.phase == InputActionPhase.Performed);
            StartCoroutine(DetectHit());

        }
    }

    IEnumerator DetectHit()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);

        if (hitInfo)
        {
            if(hitInfo.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
                iDamageable.TakeDamage(0.1f);
            }

            GameObject impact = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            Destroy(impact, impact.GetComponent<Animator>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); 

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.02f);

        lineRenderer.enabled = false;
    }
}
