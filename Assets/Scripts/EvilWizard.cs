using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://game.courses/c-events-vs-unityevents/
public class EvilWizard : MonoBehaviour
{
    private float invokeTimer;
    [SerializeField]
    private float invokeDelay = 20;

    public bool canInvoke { get; private set; } = true;

    [SerializeField]
    private float attackTimer;
    [SerializeField]
    private float attackDelay = 5;
    public bool canAttack { get; private set; } = true;
    public bool invoking { get; private set; } = false;

    [SerializeField]
    private Transform invocationPoint;

    private Animator animator;

    private Rigidbody2D rb;

    [SerializeField]
    List<Transform> listSpawnPoints = new List<Transform>();
    [SerializeField]
    List<GameObject> listMobsInvocables = new List<GameObject>();

    [SerializeField]
    List<GameObject> listMobsInvocated = new List<GameObject>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animator.SetBool(AnimationStrings.invoke, false);
    }

    private void Update()
    {
        invokeTimer += Time.deltaTime;
        canInvoke = invokeTimer > invokeDelay;
        if (canInvoke && !invoking) //  && !invoking
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
        rb.bodyType = RigidbodyType2D.Static;
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

        transform.position = invocationPoint.position;
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

        invoking = false;
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
            EndInvocation();
        }
    }

    IEnumerator EndInvocation() {
        yield return Helpers.GetWait(0.75f);
        rb.bodyType = RigidbodyType2D.Dynamic;
        invoking = false;
    }
}
