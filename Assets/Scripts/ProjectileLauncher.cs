using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public enum ShootDirection
{
    Left,
    Right,
    Up,
    Down,
}

public class ProjectileLauncher : MonoBehaviour
{
    public IObjectPool<Projectile> pool;

    [SerializeField]
    ProjectileLauncherData projectileLauncherData;

    [SerializeField]
    private LayerMask collisionLayers;

    [SerializeField]
    private ShootDirection shootDirection;

    private Animator animator;

    private Vector2 fireDirection;
    private BoxCollider2D bc2d;

    private RaycastHit2D hitInfo;

    private bool targetInSight;

    [SerializeField]
    private UnityEvent onShootStart;

    [SerializeField]
    private UnityEvent onShootEnd;

#nullable enable
    [SerializeField]
    private Transform? firePoint = null;
#nullable disable

    [SerializeField]
    private int lengthDetection = 10;

    [SerializeField]
    private bool isMoving = false;

    private Vector3[] listDirection = new Vector3[] { Vector3.left, Vector3.right, Vector3.up, Vector3.down };

    void Awake()
    {
        pool = new ObjectPool<Projectile>(
                () => CreateFunc(),
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy,
                false
            );
        bc2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        fireDirection = listDirection[(int)shootDirection];
    }

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private void FixedUpdate()
    {
        hitInfo = Physics2D.BoxCast(
            bc2d.bounds.center,
            bc2d.bounds.size,
            0,
            fireDirection * (isMoving ? transform.right.normalized : Vector3.one),
            lengthDetection,
            collisionLayers
        );

        targetInSight = hitInfo.collider != null;

        float xBounds = bc2d.bounds.min.x;
        if (shootDirection == ShootDirection.Right)
        {
            xBounds = bc2d.bounds.max.x;
        }

        Debug.DrawRay(new Vector2(xBounds, bc2d.bounds.min.y), fireDirection * lengthDetection * (isMoving ? transform.right.normalized : Vector3.one), Color.cyan);
        Debug.DrawRay(new Vector2(xBounds, bc2d.bounds.max.y), fireDirection * lengthDetection * (isMoving ? transform.right.normalized : Vector3.one), Color.cyan);

    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            if (targetInSight)
            {
                animator.SetTrigger(AnimationStrings.shoot);
                yield return null;
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                onShootStart?.Invoke();
                pool.Get();
                yield return Helpers.GetWait(projectileLauncherData.cadency);
                onShootEnd?.Invoke();
            }
            yield return null;
        }
    }

    Projectile CreateFunc()
    {
        Projectile _projectile = Instantiate(projectileLauncherData.projectile);
        _projectile.pool = pool;

        return _projectile;
    }

    void ActionOnGet(Projectile _projectile)
    {
        int yRotation = 0;
        int zRotation = 0;
        if (
            (!isMoving && shootDirection == ShootDirection.Left && _projectile.projectileData.isFacingRight) ||
            (isMoving && transform.right.normalized.x == -1)
        )
        {
            yRotation = 180;
        }

        if (shootDirection == ShootDirection.Down)
        {
            zRotation = -90;
        }
        else if (shootDirection == ShootDirection.Up)
        {
            zRotation = 90;
        }

        Quaternion quaternion = Quaternion.Euler(0, yRotation, zRotation);
        Vector3 nextPosition = firePoint != null ? firePoint.position : transform.position;
        nextPosition.z = 0;

        _projectile.transform.position = nextPosition;
        _projectile.transform.rotation = quaternion;
        _projectile.gameObject.SetActive(true);
        _projectile.ResetThyself();
    }

    void ActionOnRelease(Projectile _projectile)
    {
        _projectile.gameObject.SetActive(false);
    }

    void ActionOnDestroy(Projectile _projectile)
    {
        Destroy(_projectile.gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
