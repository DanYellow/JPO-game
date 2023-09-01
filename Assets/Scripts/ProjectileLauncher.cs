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

    public ShootDirection shootDirection;

    void Awake()
    {
        pool = new ObjectPool<Projectile>(
                () => CreateFunc(),
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy,
                true
            );
    }

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        yield return Helpers.GetWait(projectileLauncherData.startDelay);

        while (true)
        {
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
        StartCoroutine(Shoot());
    }

    private void OnBecameInvisible()
    {
        StopAllCoroutines();
    }
}
