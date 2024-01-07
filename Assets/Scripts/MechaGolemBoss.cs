using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaGolemBoss : MonoBehaviour
{
    private IEnumerator checkShieldGenerationCo;
    private bool isCheckingShieldGeneration = false;

    public bool needsToActivateShield = false;
    // Start is called before the first frame update

    public List<Transform> listSpikes = new List<Transform>();
    public List<Transform> listSpikesToThrow = new List<Transform>();

    private void Awake() {
        listSpikesToThrow = new List<Transform>(listSpikes);
    }

    void Start()
    {
        checkShieldGenerationCo = CheckShieldGeneration();

        var length = listSpikes.Count;
        var radius = 6;
        for (var i = 0; i < length; i++)
        {
            var val = Mathf.Lerp(0, 2 * Mathf.PI, (float) i/length);
            var pos = listSpikes[i].localPosition;
            pos.x = radius * Mathf.Cos(val);
            pos.y = radius * Mathf.Sin(val);

            listSpikes[i].localPosition = pos;
        }
    }

    private void Update() {
       if (Input.GetKeyDown(KeyCode.Space)) {
            ThrowSpike();
       }
    }

    private IEnumerator CheckShieldGeneration()
    {
        yield return Helpers.GetWait(1.75f);
        while (true)
        {
            // yield return Helpers.GetWait(1f); // For tests
            yield return Helpers.GetWait(4.15f);
            bool randVal = Random.value < 0.35f;
            needsToActivateShield = randVal;
        }
    }

    private void ThrowSpike() 
    {
        Transform spike = listSpikesToThrow[Random.Range(0, listSpikesToThrow.Count)];
        spike.GetComponent<MechaBossSpike>().Throw();

        listSpikesToThrow.Remove(spike);
       
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
}
