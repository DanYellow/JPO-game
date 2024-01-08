using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaGolemBoss : MonoBehaviour
{
    private IEnumerator checkShieldGenerationCo;
    private bool isCheckingShieldGeneration = false;

    public bool needsToActivateShield = false;
    private bool spikesReady = false;
    private LookAtTarget lookAtTarget;

    private Transform target;

    public float delayBetweenThrows = 3.5f;
    // Start is called before the first frame update

    public List<Transform> listSpikes = new List<Transform>();
    private List<Transform> listSpikesToThrow = new List<Transform>();

    private void Awake()
    {
        lookAtTarget = GetComponent<LookAtTarget>();
        target = GameObject.Find("Player").transform;

        listSpikes.ForEach((item) =>
        {
            item.gameObject.SetActive(false);
            item.GetComponent<RotateAround>().enabled = false;
        });
    }

    void Start()
    {
        checkShieldGenerationCo = CheckShieldGeneration();
        // StartCoroutine(PrepareSpikes());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(ExpulseSpikes());
            // StartCoroutine(ThrowSpike());
        }
    }

    private IEnumerator CheckShieldGeneration()
    {
        yield return Helpers.GetWait(1.75f);
        while (true)
        {
            print("shield");
            // yield return Helpers.GetWait(1f); // For tests
            yield return Helpers.GetWait(4.15f);
            bool randVal = Random.value < 0.25f;
            needsToActivateShield = randVal;
        }
    }

    private IEnumerator PrepareSpikes()
    {
        listSpikesToThrow = new List<Transform>(listSpikes);
        var length = listSpikes.Count;
        var radius = 7;
        spikesReady = true;
        // yield return Helpers.GetWait(3.95f);
        for (var i = 0; i < length; i++)
        {
            var spike = listSpikes[i];
            spike.gameObject.SetActive(true);
            var val = Mathf.Lerp(0, 2 * Mathf.PI, (float)i / length);
            var pos = spike.localPosition;

            pos.x = radius * Mathf.Cos(val);
            pos.y = radius * Mathf.Sin(val);

            spike.GetComponent<MechaBossSpike>().Reset();
            // spike.GetComponent<RotateAround>().enabled = true;

            spike.localPosition = pos;

            yield return Helpers.GetWait(0.15f);
        }

        for (var i = 0; i < length; i++)
        {
            var spike = listSpikes[i];
            spike.GetComponent<RotateAround>().enabled = true;
        }

        
        // while (listSpikesToThrow.Count > 0)
        // {
        //     yield return Helpers.GetWait(delayBetweenThrows);
        //     StartCoroutine(ThrowSpike());
        // }

        yield return null;
    }

    public IEnumerator ExpulseSpikes()
    {
        yield return null;
        for (var i = 0; i < listSpikesToThrow.Count; i++)
        {
            var item = listSpikesToThrow[i];

            Vector3 delta = (transform.position - item.position).normalized;
            Vector3 cross = Vector3.Cross(delta, transform.up);
            Vector3 rotateDir = cross.z > 0 ? Vector3.up : Vector3.down;

            item.GetComponent<RotateAround>().enabled = false;

            Quaternion rotation = Quaternion.LookRotation(transform.position - item.position, transform.TransformDirection(rotateDir));
            item.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

            Vector3 throwDir = -item.transform.right;
            item.GetComponent<MechaBossSpike>().Throw(throwDir);

            yield return Helpers.GetWait(0.15f);
        }

        listSpikesToThrow.Clear();
    }

    private IEnumerator ThrowSpike()
    {
        if (listSpikesToThrow.Count == 0) {
            yield break;
        }

        yield return Helpers.GetWait(delayBetweenThrows);
        //         while (listSpikesToThrow.Count > 0)
        // {
        //     yield return Helpers.GetWait(delayBetweenThrows);
        //     StartCoroutine(ThrowSpike());
        // }

        int[] anglesLimit = { -130, -50 };
        if (lookAtTarget.isFacingRight)
        {
            anglesLimit[0] = 0;
            anglesLimit[1] = 90;
        }

        // listSpikesToThrow.ForEach((item) => {
        //     Vector2 distance = item.position - transform.position;
        //     float angle = Vector2.SignedAngle(transform.right, distance);

        //     print(angle + " " + item.name);
        // });

        Transform spike = listSpikesToThrow.Find(item =>
        {
            Vector2 distance = item.position - transform.position;
            float angle = Vector2.SignedAngle(transform.right, distance);

            return angle >= anglesLimit[0] && angle <= anglesLimit[1];
        });

        if (spike)
        {
            listSpikesToThrow.ForEach((item) =>
            {
                item.GetComponent<RotateAround>().enabled = false;
            });

            Vector3 rotateDir = lookAtTarget.isFacingRight ? Vector3.down : Vector3.up;

            Quaternion rotation = Quaternion.LookRotation(target.position - spike.transform.position, transform.TransformDirection(rotateDir));
            spike.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

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

        yield return StartCoroutine(ThrowSpike());

        // if (listSpikesToThrow.Count == 0)
        // {
        //     StartCoroutine(Prepare());
        // }
    }

    public void StartShieldGenerationChecking()
    {
        if (isCheckingShieldGeneration) return;
        StartCoroutine(checkShieldGenerationCo);
        isCheckingShieldGeneration = true;
    }

    public void StopShieldGenerationChecking()
    {
        StopCoroutine(checkShieldGenerationCo);
        isCheckingShieldGeneration = false;
        needsToActivateShield = false;
    }

    public void PrepareSpikesProxy()
    {
        if (spikesReady) return;
        StartCoroutine(PrepareSpikes());
    }

    public void ExpulseSpikesProxy()
    {
        if (spikesReady) return;
        StartCoroutine(ExpulseSpikes());
    }
}
