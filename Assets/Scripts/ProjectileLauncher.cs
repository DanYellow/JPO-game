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
    private SpriteRenderer sr;

    private Vector2 firePoint;

    private RaycastHit2D hitInfo;

    public float angle = 236.5f;

    void Awake()
    {
        pool = new ObjectPool<Projectile>(
                () => CreateFunc(),
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy,
                false
            );
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        firePoint = new Vector2(sr.bounds.min.x, sr.bounds.center.y);
    }


    private void Start()
    {
        print(transform.right.normalized);
        // StartCoroutine(Shoot());
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // if (bc2d == null)
        // {
        //     bc2d = GetComponent<BoxCollider2D>();
        // } 
        
        Gizmos.DrawLine(new Vector2(sr.bounds.min.x, sr.bounds.center.y), new Vector2(sr.bounds.min.x + (10 * (shootDirection == ShootDirection.Right ? 1 : -1)), sr.bounds.center.y));
    }

    private void FixedUpdate() {
        print(transform.right.normalized);
        hitInfo = Physics2D.Raycast(firePoint, Vector3.left, 10, collisionLayers);

        if(hitInfo) {
            print(hitInfo.transform.name);
            Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, angle) * hitInfo.point, Color.white);
        } else {
            Debug.DrawRay(transform.position, Vector3.left * 5, Color.cyan);
        }
    }


    private IEnumerator Shoot()
    {
        yield return Helpers.GetWait(projectileLauncherData.startDelay);

        while (true)
        {
            // print("Shoot");
            animator.SetTrigger(AnimationStrings.shoot);
            pool.Get();
            // yield return Helpers.GetWait(10);
            yield return Helpers.GetWait(projectileLauncherData.cadency);
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

    private void OnDestroy() {
       StopAllCoroutines();
    }
}
