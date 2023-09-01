using System.Collections;

using UnityEngine;
using UnityEngine.Pool;

public class ProjectileLauncher : MonoBehaviour
{
    private IObjectPool<Projectile> pool;

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

    private void Start() {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        yield return Helpers.GetWait(projectileLauncherData.startDelay);

        while (true)
        {
            pool.Get();
            yield return Helpers.GetWait(projectileLauncherData.cadency);
        }
    }

    Projectile CreateFunc()
    {
        return Instantiate(projectileLauncherData.projectile);
    }

    void ActionOnGet(Projectile _projectile)
    {
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

    private void OnBecameVisible() {
        print("He");
        StartCoroutine(Shoot());
    }

    private void OnBecameInvisible() {
        StopAllCoroutines();
    }
}
