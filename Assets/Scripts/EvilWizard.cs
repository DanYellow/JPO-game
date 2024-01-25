using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://game.courses/c-events-vs-unityevents/
// https://forum.unity.com/threads/what-is-the-best-way-to-delay-my-attack-method.999343/
public class EvilWizard : MonoBehaviour
{
    [SerializeField]
    private float invokeTimer;
    [SerializeField]
    private float invokeDelay = 17.5f;

    public bool canInvoke { get; private set; } = true;

    [SerializeField]
    private float attackTimer;
    [SerializeField]
    private float attackDelay = 6.25f;
    public bool canAttack { get; private set; } = true;
    public bool invoking { get; private set; } = false;
    public bool isOperating { get; private set; } = false;

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


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();

        animator.SetBool(AnimationStrings.invoke, false);
        enabled = false;
        originalGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        if(!invoking) {
            invokeTimer += Time.deltaTime;
        }
        canInvoke = invokeTimer > invokeDelay;
        if (canInvoke)
        {
            invokeTimer = 0;
        }

        attackTimer += Time.deltaTime;
        canAttack = attackTimer > attackDelay;
        if (canAttack)
        {
            attackTimer = 0;
        }
    }

    public void Invoke()
    {
        invoking = true;
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
