using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MechaGolemBoss : MonoBehaviour
{
    private Coroutine expulseSpikesCo;
    private Coroutine throwAllSpikesCo;
    private Coroutine throwSpikeCo;
    private Coroutine checkExpulsingSpikesAttackCo;

    private bool areSpikesReady = false;
    [SerializeField]
    public bool isExpulsingSpikes = false;
    private bool isThrowingSpike = false;
    public bool canMove = true;
    public bool isPlayerDead = false;
    private LookAtTarget lookAtTarget;
    private MechaProtect mechaProtect;

    [SerializeField]
    private Transform pivotPointSpike;

    private Transform target;
    private Invulnerable targetInvulnerable;

    [SerializeField]
    private GameObject mechaBossSpikeSpawnPrefab;

    [HideInInspector]
    public GameObject mechaBossSpikeSpawn;

    public float delayBetweenThrows = 3.75f;
    // Start is called before the first frame update

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    public List<Transform> listSpikes = new List<Transform>();
    private List<Transform> listSpikesToThrow = new List<Transform>();

    private Enemy boss;

    [HideInInspector]
    public Bounds spikeSpawnBounds;

    private void Awake()
    {
        lookAtTarget = GetComponent<LookAtTarget>();
        mechaProtect = GetComponent<MechaProtect>();

        target = GameObject.Find("Player").transform;

        targetInvulnerable = target.GetComponent<Invulnerable>();
        boss = GetComponent<Enemy>();

        listSpikes.ForEach((item) =>
        {
            item.gameObject.SetActive(false);
            item.GetComponent<RotateAround>().enabled = false;
        });

        spikeSpawnBounds = mechaBossSpikeSpawnPrefab.GetComponentInChildren<SpriteMask>().bounds;
        mechaBossSpikeSpawn = Instantiate(
            mechaBossSpikeSpawnPrefab,
            new Vector3(
                0,
                target.GetComponent<BoxCollider2D>().bounds.min.y - spikeSpawnBounds.size.y / 2,
                0
            ),
            mechaBossSpikeSpawnPrefab.transform.rotation
        );
        mechaBossSpikeSpawn.SetActive(false);
    }

    private void OnEnable()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        isPlayerDead = true;
        StopExpulseSpikesChecking();
        StopExpulseSpikes();
        StopThrowAllSpikes();
        StopThrowSpike();
    }

    private void OnDisable()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised -= OnPlayerDeath;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.N))
        {
            // StartCoroutine(ExpulseSpikes());
            // StartCoroutine(ThrowSpike());
        }

#endif
    }

    private IEnumerator CheckExpulsingSpikesAttack()
    {
        while (mechaProtect.isGuarding == true)
        {
            yield return Helpers.GetWait(3.05f);
            if (Random.value < 0.5f && isExpulsingSpikes == false)
            {
                // expulseSpikesCo = StartCoroutine(ExpulseSpikes());
                yield return StartCoroutine(ExpulseSpikes());
            }
        }
    }

    private IEnumerator PrepareSpikes()
    {
        if (listSpikesToThrow.Count > 0)
        {
            yield break;
        }
        listSpikesToThrow = new List<Transform>(listSpikes);
        var length = listSpikes.Count;
        var radius = 20;
        areSpikesReady = false;
        canMove = false;

        for (var i = 0; i < length; i++)
        {
            var spike = listSpikes[i];
            spike.parent = pivotPointSpike;
            spike.gameObject.SetActive(true);
            var val = Mathf.Lerp(0, 2 * Mathf.PI, (float)i / length);
            var pos = spike.localPosition;

            pos.x = radius * Mathf.Cos(val);
            pos.y = radius * Mathf.Sin(val);
            pos.z = 0;

            spike.GetComponent<MechaBossSpike>().Reset();

            spike.localPosition = pos;

            yield return Helpers.GetWait(0.15f);
        }

        for (var i = 0; i < length; i++)
        {
            var spike = listSpikes[i];

            RotateAround rotateAround = spike.GetComponent<RotateAround>();
            float rotateAroundBaseSpeed = rotateAround.GetBaseSpeed();
            rotateAround.enabled = true;
            rotateAround.SetSpeed(
                Mathf.Lerp(
                    rotateAroundBaseSpeed * 1.25f,
                    rotateAroundBaseSpeed,
                    (float)boss.GetHealth() / boss.GetMaxHealth()
                )
            );
        }
        canMove = true;

        yield return Helpers.GetWait(1.15f);
        areSpikesReady = true;
    }

    private IEnumerator ExpulseSpikes()
    {
        if (listSpikesToThrow.Count == 0)
        {
            yield return StartCoroutine(PrepareSpikes());
        }
        else
        {
            yield return Helpers.GetWait(2.5f);
        }

        RotateSpikes(false);

        for (var i = 0; i < listSpikesToThrow.Count; i++)
        {
            var item = listSpikesToThrow[i];

            Vector3 delta = (transform.position - item.position).normalized;
            Vector3 cross = Vector3.Cross(delta, transform.up);
            Vector3 rotateDir = cross.z > 0 ? Vector3.up : Vector3.down;

            Quaternion rotation = Quaternion.LookRotation(transform.position - item.position, transform.TransformDirection(rotateDir));
            item.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

            yield return Helpers.GetWait(0.15f);
        }

        for (var i = 0; i < listSpikesToThrow.Count; i++)
        {
            var item = listSpikesToThrow[i];

            item.parent = null;
            Vector3 throwDir = -item.transform.right;
            item.GetComponent<MechaBossSpike>().Throw(throwDir);
        }

        var lastSpike = listSpikesToThrow?.Last();
        listSpikesToThrow.Clear();

        if (lastSpike)
        {
            yield return new WaitUntil(() => Vector3.Distance(transform.position, lastSpike.position) >= 150);
        }
        yield return Helpers.GetWait(2.85f);

        isExpulsingSpikes = false;
    }

    private IEnumerator ThrowSpike()
    {
        isThrowingSpike = true;
        canMove = false;

        int[] anglesLimit = { -130, -50 };
        if (lookAtTarget.isFacingRight)
        {
            anglesLimit[0] = 0;
            anglesLimit[1] = 90;
        }

        Transform spike = null;
        int indexSpike = -1;

        yield return new WaitUntil(() =>
        {
            indexSpike = listSpikesToThrow.FindIndex(item =>
            {
                Vector2 distance = item.position - transform.position;
                float angle = Vector2.SignedAngle(transform.right, distance);

                return angle >= anglesLimit[0] && angle <= anglesLimit[1];
            });

            return indexSpike > -1 && targetInvulnerable.isInvulnerable == false;
        });
        spike = listSpikesToThrow[indexSpike];

        if (spike)
        {
            spike.parent = null;
            RotateSpikes(false);

            spike.transform.rotation = GetSpikeOrientation(spike.transform.position);

            spike.GetComponent<RotateAround>().enabled = false;

            Vector3 throwDir = (target.position - spike.transform.position).normalized;
            listSpikesToThrow.Remove(spike);
            yield return Helpers.GetWait(0.65f);
            spike.GetComponent<MechaBossSpike>().Throw(throwDir);

            RotateSpikes(true);
            yield return Helpers.GetWait(0.55f);
        }
        canMove = true;
        
        yield return Helpers.GetWait(delayBetweenThrows);

        if (listSpikesToThrow.Count == 0)
        {
            if (spike)
            {
                yield return new WaitUntil(() => Vector3.Distance(spike.position, transform.position) >= 20);
            }
            yield return StartCoroutine(PrepareSpikes());
        }

        isThrowingSpike = false;
    }

    public IEnumerator ThrowAllSpikes()
    {
        if (listSpikesToThrow.Count > 0)
        {
            isThrowingSpike = true;

            listSpikesToThrow.ForEach((spike) =>
            {
                spike.GetComponent<RotateAround>().enabled = false;
                spike.parent = null;

                spike.transform.rotation = GetSpikeOrientation(spike.transform.position);

                Vector3 throwDir = (target.position - spike.transform.position).normalized;
                spike.GetComponent<MechaBossSpike>().Throw(throwDir);
            });

            var lastSpike = listSpikesToThrow.Last();

            listSpikesToThrow.Clear();

            yield return new WaitUntil(() => Vector3.Distance(lastSpike.position, transform.position) >= 20);
            yield return Helpers.GetWait(3.15f);
        }

        yield return StartCoroutine(PrepareSpikes());
        isThrowingSpike = false;
    }

    public void DestroyAllSpikes()
    {
        listSpikesToThrow.ForEach((spike) =>
        {
            spike.GetComponent<RotateAround>().enabled = false;
            spike.GetComponent<Collider2D>().enabled = false;
            spike.GetComponent<MechaBossSpike>().Destroy();
            spike.parent = null;
        });
        StopAllCoroutines();
    }

    public void RotateSpikes(bool rotating = true)
    {
        listSpikesToThrow.ForEach((item) =>
        {
            item.GetComponent<RotateAround>().enabled = rotating;
        });
    }

    private Quaternion GetSpikeOrientation(Vector3 position)
    {
        Vector3 rotateDir = lookAtTarget.isFacingRight ? Vector3.down : Vector3.up;
        Quaternion rotation = Quaternion.LookRotation(target.position - position, transform.TransformDirection(rotateDir));

        return new Quaternion(0, 0, rotation.z, rotation.w);
    }

    public void PrepareSpikesProxy()
    {
        StartCoroutine(PrepareSpikes());
    }

    public void ThrowSpikeProxy()
    {
        if (!areSpikesReady || isThrowingSpike) return;
        throwSpikeCo = StartCoroutine(ThrowSpike());
    }

    public void ThrowAllSpikesProxy()
    {
        if (!areSpikesReady || isThrowingSpike) return;
        throwAllSpikesCo = StartCoroutine(ThrowAllSpikes());
    }

    public void StartExpulseSpikesChecking()
    {
        checkExpulsingSpikesAttackCo = StartCoroutine(CheckExpulsingSpikesAttack());
    }

    public void StopExpulseSpikesChecking()
    {
        if (checkExpulsingSpikesAttackCo != null)
        {
            StopCoroutine(checkExpulsingSpikesAttackCo);
        }
    }

    public void StopExpulseSpikes()
    {
        isExpulsingSpikes = false;
        if (expulseSpikesCo != null)
        {
            StopCoroutine(expulseSpikesCo);
        }
    }

    public void StopThrowAllSpikes()
    {
        isExpulsingSpikes = false;
        if (throwAllSpikesCo != null)
        {
            StopCoroutine(throwAllSpikesCo);
        }
    }

    public void StopThrowSpike()
    {
        isThrowingSpike = false;
        if (throwSpikeCo != null)
        {
            StopCoroutine(throwSpikeCo);
        }
    }
}
