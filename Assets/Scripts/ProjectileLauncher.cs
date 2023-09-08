using System.Collections;

using UnityEngine;
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

        // if(hitInfo) {
        //     print(hitInfo.transform.name);
        //     // Debug.DrawRay(firePoint, Vector3.left * lengthDetection, Color.white);
        // } else {
        // //     // Debug.DrawRay(firePoint, Vector3.left * lengthDetection, Color.cyan);
        // }

        targetInSight = hitInfo.collider != null;
        if(transform.right.normalized.x == -1) {
            Debug.DrawRay(new Vector2(bc2d.bounds.min.x - 0.15f, bc2d.bounds.min.y), fireDirection * lengthDetection * (isMoving ? transform.right.normalized : Vector3.one), Color.cyan);
            Debug.DrawRay(new Vector2(bc2d.bounds.min.x - 0.15f, bc2d.bounds.max.y), fireDirection * lengthDetection * (isMoving ? transform.right.normalized : Vector3.one), Color.cyan);
        } else {
            Debug.DrawRay(new Vector2(bc2d.bounds.max.x + 0.15f, bc2d.bounds.min.y), fireDirection * lengthDetection * (isMoving ? transform.right.normalized : Vector3.one), Color.cyan);
            Debug.DrawRay(new Vector2(bc2d.bounds.max.x + 0.15f, bc2d.bounds.max.y), fireDirection * lengthDetection * (isMoving ? transform.right.normalized : Vector3.one), Color.cyan);

        }
    }


    private IEnumerator Shoot()
    {
        // yield return Helpers.GetWait(projectileLauncherData.startDelay);

        while (true)
        {
            if (targetInSight)
            {
                animator.SetTrigger(AnimationStrings.shoot);
                yield return null;
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                pool.Get();
                yield return Helpers.GetWait(projectileLauncherData.cadency);
                // yield return Helpers.GetWait(5);
            }
            yield return null;
        }
    }

    void CancelAllProjectiles()
    {
        // pool.Clear();
    }

    Projectile CreateFunc()
    {
        Projectile _projectile = Instantiate(projectileLauncherData.projectile);
        _projectile.pool = pool;

        return _projectile;
    }

    void ActionOnGet(Projectile _projectile)
    {
        int rotationAngle = 0;
        if (!isMoving && shootDirection == ShootDirection.Left && _projectile.projectileData.isFacingRight)
        {
            rotationAngle = 180;
        }

        if(isMoving && transform.right.normalized.x == -1) {
            rotationAngle = 180;
        }

        Quaternion quaternion = Quaternion.Euler(0, rotationAngle, 0);
        // _projectile.transform.position = transform.position;
        // print(firePoint != null);
        _projectile.transform.position = firePoint != null ? firePoint.position : transform.position;
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
