using System;
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

// public static class ShootDirection
// {
//     public static readonly Vector3 left = Vector3.left;
//     public static readonly Vector3 right = Vector3.right;
//     public static readonly Vector3 up = Vector3.up;
//     public static readonly Vector3 down = Vector3.down;
// }

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
    private int lengthDetection = 10;

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

        fireDirection = listDirection[(int) shootDirection]; //shootDirection == ShootDirection.Left ? Vector3.left : Vector3.right;
        print(fireDirection);
    }


    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private void FixedUpdate()
    {
        hitInfo = Physics2D.BoxCast(
            new Vector2(bc2d.bounds.center.x - 0.5f, bc2d.bounds.center.y),
            bc2d.bounds.size,
            0,
            fireDirection,
            lengthDetection,
            collisionLayers
        );

        // if(hitInfo) {
        //     print(hitInfo.transform.name);
        //     // Debug.DrawRay(firePoint, Vector3.left * lengthDetection, Color.white);
        // } else {
        //     // Debug.DrawRay(firePoint, Vector3.left * lengthDetection, Color.cyan);
        // }

        targetInSight = hitInfo.collider != null;
        Debug.DrawRay(new Vector2(bc2d.bounds.min.x - 0.25f, bc2d.bounds.min.y), fireDirection * lengthDetection, Color.cyan);
        Debug.DrawRay(new Vector2(bc2d.bounds.min.x - 0.25f, bc2d.bounds.max.y), fireDirection * lengthDetection, Color.cyan);
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

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
