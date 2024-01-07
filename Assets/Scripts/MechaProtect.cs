using System.Collections;
using UnityEngine;

public class MechaProtect : MonoBehaviour, IGuardable
{
    public bool isGuarding { get; set; } = false;
    public bool hasTotalGuard { get; } = true;

    private RaycastHit2D hitObstacle;
    private BoxCollider2D bc2d;
    private SpriteRenderer sr;

    [HideInInspector]
    public GameObject shield;

    [SerializeField]
    private LayerMask targetLayerMask;

    private void Awake()
    {
        bc2d = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        isGuarding = true;
    }

    private void OnDisable() {
        isGuarding = false;
    }

    private void Reflect()
    {
        if (hitObstacle.transform.TryGetComponent(out Knockback knockback))
        {
            knockback.Apply(gameObject, KnockbackValues.lightAttack);
            StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash() {
        sr.color = new Color(0.3f, 0.4f, 0.6f, 1f);
        yield return new WaitForSeconds(0.35f);
        sr.color = Color.white;
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
