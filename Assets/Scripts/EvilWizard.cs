using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://game.courses/c-events-vs-unityevents/
// https://forum.unity.com/threads/what-is-the-best-way-to-delay-my-attack-method.999343/
public class EvilWizard : MonoBehaviour
{
    public float invokeCountdownMax = 17.5f;
    public float invokeCountdown = 0;

    public float attackCountdownMax = 4.25f;
    public float attackCountdown = 0;

    [HideInInspector]
    public bool isAttacking = false;

    public bool invoking { get; private set; } = false;

    [SerializeField]
    private Transform invocationPoint;

    private Animator animator;

    private Rigidbody2D rb;

    [SerializeField]
    List<Transform> listSpawnPoints = new List<Transform>();
    [SerializeField]
    List<GameObject> listMobsInvocables = new List<GameObject>();

    List<GameObject> listMobsInvocated = new List<GameObject>();

    [SerializeField]
    private BlastEffectData blastEffectData;

    private new Collider2D collider;

    private float originalGravityScale;

    public bool hasTouchedWall = false;

    [SerializeField]
    private LayerMask wallLayersMask;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();

        animator.SetBool(AnimationStrings.invoke, false);
        enabled = false;
        originalGravityScale = rb.gravityScale;

        invokeCountdown = invokeCountdownMax;
        attackCountdown = 0;
    }

    private void FixedUpdate()
    {
        hasTouchedWall = HasTouchedWall();
    }

    private bool HasTouchedWall()
    {
        return Physics2D.Linecast(
            new Vector2(collider.bounds.max.x, collider.bounds.center.y),
            new Vector2(collider.bounds.max.x + 0.5f, collider.bounds.center.y),
            wallLayersMask
        );
    }

    void OnDrawGizmos()
    {
        if (collider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                new Vector2(collider.bounds.max.x, collider.bounds.center.y),
                new Vector2(collider.bounds.max.x + 0.5f, collider.bounds.center.y)
            );
        }
    }

    public void Invoke()
    {
        invoking = true;
        invokeCountdown = invokeCountdownMax;
        StartCoroutine(InvokeCoroutine());
    }

    IEnumerator InvokeCoroutine()
    {
        // rb.bodyType = RigidbodyType2D.Static;
        rb.gravityScale = 0;
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

        transform.position = invocationPoint.position;
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

        foreach (var item in listSpawnPoints)
        {
            GameObject summoned = Instantiate(
                listMobsInvocables[Random.Range(0, listMobsInvocables.Count)],
                item.position,
                Quaternion.identity
            );
            Enemy enemy = summoned.GetComponentInChildren<Enemy>();
            if (enemy != null)
            {
                enemy.deathNotify += OnMobDeath;
            }
            listMobsInvocated.Add(summoned);
        }
    }

    void OnMobDeath(GameObject go)
    {
        listMobsInvocated.Remove(go);

        if (listMobsInvocated.Count == 0)
        {
            StartCoroutine(EndInvocation());
        }
    }

    IEnumerator EndInvocation()
    {
        yield return Helpers.GetWait(0.75f);
        rb.gravityScale = originalGravityScale;
        yield return Helpers.GetWait(1.75f);
        invoking = false;
    }

    public void InvokeBlast()
    {
        GameObject blast = Instantiate(blastEffectData.effect, new Vector2(collider.bounds.center.x, collider.bounds.min.y), Quaternion.identity);
        blast.GetComponent<BlastEffect>().SetEffectData(blastEffectData);
        blast.GetComponent<SpriteRenderer>().color = blastEffectData.color;
        blast.transform.localScale *= blastEffectData.scale;
    }
}
