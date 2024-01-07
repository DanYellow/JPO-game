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

    void Start()
    {
        checkShieldGenerationCo = CheckShieldGeneration();

        var length = listSpikes.Count;
        for (var i = 0; i < length; i++)
        {
            var val = Mathf.Lerp(0, 2 * Mathf.PI, (float) i/length);
            var pos = listSpikes[i].localPosition;
            pos.x = 4 * Mathf.Cos(val);
            pos.y = 4 * Mathf.Sin(val);

            listSpikes[i].localPosition = pos;
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
