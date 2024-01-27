using UnityEngine.Pool;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public Projectile projectile;
    public IObjectPool<Projectile> pool;

    #nullable enable
        [SerializeField]
        private Transform? firePoint = null;
    #nullable disable

    private void Awake()
    {
        pool = new ObjectPool<Projectile>(
                () => CreateFunc(),
                ActionOnGet,
                ActionOnRelease,
                ActionOnDestroy,
                false
            );
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Shoot();
    //     }
    // }

    public void Shoot()
    {
        pool.Get();
    }

    Projectile CreateFunc()
    {
        Projectile _projectile = Instantiate(projectile);
        _projectile.pool = pool;

        return _projectile;
    }

    void ActionOnGet(Projectile _projectile)
    {
        Vector3 nextPosition = firePoint != null ? firePoint.position : transform.position;
        nextPosition.z = 0;

        _projectile.transform.position = nextPosition;
        _projectile.transform.rotation = transform.rotation;
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
}
