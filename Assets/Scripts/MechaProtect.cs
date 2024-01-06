using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MechaProtect : MonoBehaviour, IGuardable
{
    public bool isGuarding { get; set; } = false;

    private RaycastHit2D hitObstacle;
    private BoxCollider2D bc2d;

    [SerializeField]
    private LayerMask targetLayerMask;

    private void Awake()
    {
        bc2d = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isGuarding = true;
    }

    private void Reflect()
    {
        if (hitObstacle.transform.TryGetComponent(out Knockback knockback))
        {
            knockback.Apply(gameObject, 5);
        }
    }

    private void FixedUpdate()
    {
        var origin = new Vector2(bc2d.bounds.min.x - 2.5f, bc2d.bounds.center.y);

        hitObstacle = Physics2D.Linecast(
            origin,
            new Vector2(bc2d.bounds.max.x + 2.5f, bc2d.bounds.center.y),
            targetLayerMask
        );

        if (hitObstacle)
        {
            Reflect();
        }
    }
}
