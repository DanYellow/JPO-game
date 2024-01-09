using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MechaGolemBoss : MonoBehaviour
{
    private Coroutine checkShieldGenerationCo;
    private Coroutine expulseSpikesCo;
    private Coroutine throwAllSpikesCo;
    private Coroutine throwSpikeCo;

    private bool isCheckingShieldGeneration = false;

    public bool needsToActivateShield = false;
    private bool areSpikesReady = false;
    private bool areSpikesPreparing = false;
    [SerializeField]
    private bool isExpulsingSpikes = false;
    private bool isThrowingSpike = false;
    private LookAtTarget lookAtTarget;
    private MechaProtect mechaProtect;

    private Transform target;

    public float delayBetweenThrows = 3.75f;
    // Start is called before the first frame update

    public List<Transform> listSpikes = new List<Transform>();
    private List<Transform> listSpikesToThrow = new List<Transform>();

    private void Awake()
    {
        lookAtTarget = GetComponent<LookAtTarget>();
        mechaProtect = GetComponent<MechaProtect>();

        target = GameObject.Find("Player").transform;

        listSpikes.ForEach((item) =>
        {
            item.gameObject.SetActive(false);
            item.GetComponent<RotateAround>().enabled = false;
        });
    }

    void Start()
    {
        // StartCoroutine(PrepareSpikes());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            // StartCoroutine(ExpulseSpikes());
            // StartCoroutine(ThrowSpike());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            needsToActivateShield = true;
        }
    }

    public bool IsPerformingAction()
    {
        return !isExpulsingSpikes && !isThrowingSpike;
    }

    private IEnumerator CheckShieldGeneration()
    {
        yield return null;
        yield return Helpers.GetWait(0.85f);
        while (mechaProtect.isGuarding == false)
        {
            // yield return Helpers.GetWait(1f); // For tests
            yield return Helpers.GetWait(4.15f);
            // bool randVal = Random.value < 0.5f;
            bool randVal = Random.value < 0.2f;
            print("shield " + randVal);
            needsToActivateShield = randVal;
        }
    }

    private IEnumerator PrepareSpikes()
    {
        if(listSpikesToThrow.Count > 0) {
            yield break;
        }
        listSpikesToThrow = new List<Transform>(listSpikes);
        var length = listSpikes.Count;
        var radius = 7;
        areSpikesReady = false;
        areSpikesPreparing = true;

        for (var i = 0; i < length; i++)
        {
            var spike = listSpikes[i];
            spike.parent = transform;
            spike.gameObject.SetActive(true);
            var val = Mathf.Lerp(0, 2 * Mathf.PI, (float)i / length);
            var pos = spike.localPosition;

            pos.x = radius * Mathf.Cos(val);
            pos.y = radius * Mathf.Sin(val);

            spike.GetComponent<MechaBossSpike>().Reset();

            spike.localPosition = pos;

            yield return Helpers.GetWait(0.15f);
        }

        for (var i = 0; i < length; i++)
        {
            var spike = listSpikes[i];
            spike.GetComponent<RotateAround>().enabled = true;
        }

        yield return Helpers.GetWait(1.15f);
        areSpikesReady = true;
    }

    private IEnumerator ExpulseSpikes()
    {
        isExpulsingSpikes = true;

        yield return Helpers.GetWait(3.5f);

        listSpikesToThrow.ForEach((item) =>
        {
            item.GetComponent<RotateAround>().enabled = false;
        });

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
            yield return new WaitUntil(() => Vector3.Distance(lastSpike.position, transform.position) >= 25);
        }
        yield return Helpers.GetWait(6.15f);
        yield return StartCoroutine(PrepareSpikes());

        isExpulsingSpikes = false;
    }

    // private IEnumerator ThrowSpike()
    // {
    //     if (listSpikesToThrow.Count == 0)
    //     {
    //         yield return StartCoroutine(PrepareSpikes());
    //     }

    //     yield return Helpers.GetWait(delayBetweenThrows);

    //     int[] anglesLimit = { -130, -50 };
    //     if (lookAtTarget.isFacingRight)
    //     {
    //         anglesLimit[0] = 0;
    //         anglesLimit[1] = 90;
    //     } 

    //     // listSpikesToThrow.ForEach((item) => {
    //     //     Vector2 distance = item.position - transform.position;
    //     //     float angle = Vector2.SignedAngle(transform.right, distance);

    //     //     print(angle + " " + item.name);
    //     // });

    //     Transform spike = listSpikesToThrow.Find(item =>
    //     {
    //         Vector2 distance = item.position - transform.position;
    //         float angle = Vector2.SignedAngle(transform.right, distance);

    //         return angle >= anglesLimit[0] && angle <= anglesLimit[1];
    //     });

    //     if (spike)
    //     {
    //         listSpikesToThrow.ForEach((item) =>
    //         {
    //             item.GetComponent<RotateAround>().enabled = false;
    //         });

    //         Vector3 rotateDir = lookAtTarget.isFacingRight ? Vector3.down : Vector3.up;

    //         Quaternion rotation = Quaternion.LookRotation(target.position - spike.transform.position, transform.TransformDirection(rotateDir));
    //         spike.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

    //         spike.GetComponent<RotateAround>().enabled = false;

    //         Vector3 throwDir = (target.position - spike.transform.position).normalized;
    //         spike.GetComponent<MechaBossSpike>().Throw(throwDir);

    //         yield return Helpers.GetWait(0.35f);

    //         listSpikesToThrow.Remove(spike);

    //         listSpikesToThrow.ForEach((item) =>
    //         {
    //             item.GetComponent<RotateAround>().enabled = true;
    //         });
    //     }

    //     yield return StartCoroutine(ThrowSpike());

    //     // if (listSpikesToThrow.Count == 0)
    //     // {
    //     //     StartCoroutine(Prepare());
    //     // }
    // }

    private IEnumerator ThrowSpike()
    {
        isThrowingSpike = true;

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

            return indexSpike > -1;
        });
        spike = listSpikesToThrow[indexSpike];

        if (spike)
        {
            spike.parent = null;
            listSpikesToThrow.ForEach((item) =>
            {
                item.GetComponent<RotateAround>().enabled = false;
            });

            spike.transform.rotation = GetSpikeOrientation(spike.transform.position);

            spike.GetComponent<RotateAround>().enabled = false;

            Vector3 throwDir = (target.position - spike.transform.position).normalized;
            spike.GetComponent<MechaBossSpike>().Throw(throwDir);

            yield return Helpers.GetWait(0.35f);

            listSpikesToThrow.Remove(spike);

            listSpikesToThrow.ForEach((item) =>
            {
                item.GetComponent<RotateAround>().enabled = true;
            });
        }

        var delay = delayBetweenThrows;
        // var delay = listSpikesToThrow.Count == 0 ? 0 : delayBetweenThrows * (1 / listSpikesToThrow.Count);

        yield return Helpers.GetWait(delay);

        if (spike && listSpikesToThrow.Count == 0)
        {
            yield return new WaitUntil(() => Vector3.Distance(spike.position, transform.position) >= 20);
        }

        if (listSpikesToThrow.Count == 0)
        {
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

    private Quaternion GetSpikeOrientation(Vector3 position)
    {
        Vector3 rotateDir = lookAtTarget.isFacingRight ? Vector3.down : Vector3.up;
        Quaternion rotation = Quaternion.LookRotation(target.position - position, transform.TransformDirection(rotateDir));

        return new Quaternion(0, 0, rotation.z, rotation.w);
    }

    public void StartShieldGenerationChecking()
    {
        if (isCheckingShieldGeneration) return;
        checkShieldGenerationCo = StartCoroutine(CheckShieldGeneration());
        isCheckingShieldGeneration = true;
    }

    public void StopShieldGenerationChecking()
    {
        isCheckingShieldGeneration = false;
        needsToActivateShield = false;
        if (checkShieldGenerationCo != null)
        {
            StopCoroutine(checkShieldGenerationCo);
        }
    }

    public void PrepareSpikesProxy()
    {
        // if (areSpikesPreparing || areSpikesReady) return;
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

    public void ExpulseSpikesProxy()
    {
        if (!areSpikesReady || isExpulsingSpikes) return;
        bool randVal = Random.value < 0.05f;
        if (randVal)
        {
            expulseSpikesCo = StartCoroutine(ExpulseSpikes());
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
