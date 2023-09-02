using System.Collections;

using UnityEngine;
using UnityEngine.Pool;


public enum ShootDirection
{
    Right,
    Left
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

    private Vector2 firePoint;
    private BoxCollider2D bc2d;

    private RaycastHit2D hitInfo;

    private bool targetInSight;

    [SerializeField]
    private int lengthDetection = 10;

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

        firePoint = new Vector2(bc2d.bounds.min.x, bc2d.bounds.center.y);
    }


    private void Start()
    {
        print(transform.right.normalized);
        StartCoroutine(Shoot());
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // if (bc2d == null)
        // {
        //     bc2d = GetComponent<BoxCollider2D>();
        // } 

        // Gizmos.DrawLine(new Vector2(sr.bounds.min.x, sr.bounds.center.y), new Vector2(sr.bounds.min.x + (10 * (shootDirection == ShootDirection.Right ? 1 : -1)), sr.bounds.center.y));
    }

    private void FixedUpdate()
    {
        targetInSight = Physics2D.Raycast(firePoint, Vector3.left, lengthDetection, collisionLayers);

        // if(hitInfo) {
        //     print(hitInfo.transform.name);
        //     Debug.DrawRay(firePoint, Vector3.left * lengthDetection, Color.white);
        // } else {
            Debug.DrawRay(firePoint, Vector3.left * lengthDetection, Color.cyan);
        // }
    }


    private IEnumerator Shoot()
    {
        // yield return Helpers.GetWait(projectileLauncherData.startDelay);

        while (true)
        {
            if (targetInSight)
            {
                animator.SetTrigger(AnimationStrings.shoot);
                pool.Get();
                yield return Helpers.GetWait(projectileLauncherData.cadency);
            }
            yield return null;
        }
    }

    Projectile CreateFunc()
    {
        Projectile _projectile = Instantiate(projectileLauncherData.projectile);
        _projectile.projectileData.shootDirection = shootDirection;
        _projectile.pool = pool;

        return _projectile;
    }

    void ActionOnGet(Projectile _projectile)
    {
        int rotationAngle = 0;
        if (shootDirection == ShootDirection.Left && _projectile.projectileData.isFacingRight)
        {
            rotationAngle = 180;
        }

        Quaternion quaternion = Quaternion.Euler(0, rotationAngle, 0);
        _projectile.transform.position = transform.position;
        _projectile.transform.rotation = quaternion;
        _projectile.gameObject.SetActive(true);
    }

    void ActionOnRelease(Projectile _projectile)
    {
        _projectile.gameObject.SetActive(false);
    }

    void ActionOnDestroy(Projectile _projectile)
    {
        Destroy(_projectile.gameObject);
    }

    private void OnBecameVisible()
    {
        // StartCoroutine(Shoot());
    }

    private void OnBecameInvisible()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
