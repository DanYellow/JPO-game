using System.Collections;

using UnityEngine;
using UnityEngine.Pool;

public class ProjectileLauncher : MonoBehaviour
{
    public IObjectPool<Projectile> pool;

    [SerializeField]
    ProjectileLauncherData projectileLauncherData;

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
            
            yield return Helpers.GetWait(10);
            // yield return Helpers.GetWait(projectileLauncherData.cadency);
        }
    }

    Projectile CreateFunc()
    {
        Projectile _projectile = Instantiate(projectileLauncherData.projectile, transform.position, Quaternion.identity);
        _projectile.projectileData.shootDirection = projectileLauncherData.shootDirection;
        _projectile.pool = pool;

        return _projectile;
    }

    void ActionOnGet(Projectile _projectile)
    {
        _projectile.transform.position = transform.position;
        _projectile.transform.rotation = transform.rotation;
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
        // print("He");
        StartCoroutine(Shoot());
    }

    private void OnBecameInvisible()
    {
        StopAllCoroutines();
    }
}
